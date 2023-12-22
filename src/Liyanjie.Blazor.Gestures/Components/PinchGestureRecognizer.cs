namespace Liyanjie.Blazor.Gestures.Components;

public class PinchGestureRecognizer : ComponentBase
{
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public double MinScale { get; set; } = 0;
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinch { get; set; }
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinchEnd { get; set; }
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinchIn { get; set; }
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinchOut { get; set; }

    double startDistance;
    double lastScale = 1;
    bool pinchStart;

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

        startDistance = e.StartPoints[0].CalcDistance(e.StartPoints[1]);
        pinchStart = true;
    }

    void GestureMove(object? sender, GestureEventArgs e)
    {
        if (e.MovePoints.Count < 2)
            return;

        if (pinchStart)
            AwarePinch(e);
    }

    void GestureEnd(object? sender, GestureEventArgs e)
    {
        if (e.MovePoints.Count < 2)
            return;

        if (pinchStart)
            AwarePinchEnd(e);

        pinchStart = false;
        startDistance = 0;
    }

    void AwarePinch(GestureEventArgs e)
    {
        var moveDistance = e.MovePoints[0].CalcDistance(e.MovePoints[1]);
        var scale = moveDistance / startDistance;

        OnPinch.InvokeAsync(CreateEventArgs("pinch", e, scale));
    }
    void AwarePinchEnd(GestureEventArgs e)
    {
        var moveDistance = e.MovePoints[0].CalcDistance(e.MovePoints[1]);
        var scale = moveDistance / startDistance;

        OnPinchEnd.InvokeAsync(CreateEventArgs("pinchend", e, scale));

        if (Math.Abs(1 - scale) > MinScale)
        {
            var scale_diff = 0.00000000001; //防止touchend的scale与__scale_last_rate相等，不触发事件的情况。
            if (scale > lastScale) //手势放大, 触发pinchout事件
            {
                lastScale = scale - scale_diff;
                OnPinchOut.InvokeAsync(CreateEventArgs("pinchout", e, scale));
            }
            else if (scale < lastScale) //手势缩小,触发pinchin事件
            {
                lastScale = scale + scale_diff;
                OnPinchIn.InvokeAsync(CreateEventArgs("pinchin", e, scale));
            }

            lastScale = 1;
        }
    }

    GesturePinchEventArgs CreateEventArgs(
        string type,
        GestureEventArgs e,
        double scale)
    {
        return new(e)
        {
            Type = type,
            Scale = scale,
        };
    }
}