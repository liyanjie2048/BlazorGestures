namespace Liyanjie.Blazor.Gestures.Components;

/// <summary>
/// 
/// </summary>
public sealed class RotateGestureRecognizer : ComponentBase
{
    [CascadingParameter] GestureRecognizer? GestureRecognizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public double MinAngle { get; set; } = 10;

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateEnd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateCW { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateCCW { get; set; }

    bool rotateStart;
    double lastAngle;
    double angleChange;

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureStart += GestureStart;
            GestureRecognizer.GestureMove += GestureMove;
            GestureRecognizer.GestureEnd += GestureEnd;
            GestureRecognizer.GestureLeave += GestureLeave;
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

        Clear(e);
    }
    void GestureLeave(object? sender, GestureEventArgs e)
    {
        if (e.MovePoints.Count < 2)
            return;

        if (rotateStart)
            AwareRotateEnd(e);
    }
    void Clear(GestureEventArgs e)
    {
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
                OnRotateCW.InvokeAsync(CreateEventArgs("rotatecw", e));
            }
            else
            {
                OnRotateCCW.InvokeAsync(CreateEventArgs("rotateccw", e));
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