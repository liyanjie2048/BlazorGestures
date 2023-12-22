namespace Liyanjie.Blazor.Gestures.Components;

public class RotateGestureRecognizer : ComponentBase
{
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public double MinAngle { get; set; } = 30;
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotate { get; set; }
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateEnd { get; set; }
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateLeft { get; set; }
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateRight { get; set; }

    bool rotateStart;
    double lastAngle;
    double angleChange;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureStart += GestureStart;
            GestureRecognizer.GestureMove += GestureMove;
            GestureRecognizer.GestureEnd += GestureEnd;
        }
    }

    void GestureStart(object? sender, GestureEventArgs e)
    {
        if (e.StartPoints.Count < 2)
            return;

        lastAngle = e.MovePoints[0].CalcAngle(e.MovePoints[1]);
        rotateStart = true;

    }
    void GestureMove(object? sender, GestureEventArgs e)
    {
        if (e.MovePoints.Count < 2)
            return;

        if (rotateStart)
            AwareRotate(e);
    }
    void GestureEnd(object? sender, GestureEventArgs e)
    {
        if (e.MovePoints.Count < 2)
            return;

        if (rotateStart)
            AwareRotateEnd(e);

        rotateStart = false;
        lastAngle = 0;
        angleChange = 0;
    }

    void AwareRotate(GestureEventArgs e)
    {
        var moveAngle = e.MovePoints[0].CalcAngle(e.MovePoints[1]);
        angleChange += GetAngleChange(moveAngle);

        OnRotate.InvokeAsync(CreateEventArgs("rotate", e));
    }
    void AwareRotateEnd(GestureEventArgs e)
    {
        OnRotateEnd.InvokeAsync(CreateEventArgs("rotateend", e));

        if (Math.Abs(angleChange) > MinAngle)
        {
            if (angleChange > 0)
            {
                OnRotateRight.InvokeAsync(CreateEventArgs("rotateright", e));
            }
            else
            {
                OnRotateLeft.InvokeAsync(CreateEventArgs("rotateleft", e));
            }
        }
    }

    double GetAngleChange(double moveAngle)
    {
        var value = moveAngle - lastAngle;
        lastAngle = moveAngle;

        if (value > 180)
            value = 360 - value;
        else if (value < -180)
            value = 360 + value;
        Console.WriteLine(value);
        return value;
    }

    GestureRotateEventArgs CreateEventArgs(
        string type,
        GestureEventArgs e)
    {
        return new(e)
        {
            Type = type,
            AngleChange = angleChange,
        };
    }
}