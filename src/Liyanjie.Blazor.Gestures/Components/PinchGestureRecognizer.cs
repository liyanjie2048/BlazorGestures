namespace Liyanjie.Blazor.Gestures.Components;

/// <summary>
/// 
/// </summary>
public sealed class PinchGestureRecognizer : ComponentBase
{
    [CascadingParameter] GestureRecognizer? GestureRecognizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public double MinScale { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinch { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinchEnd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinchIn { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<GesturePinchEventArgs> OnPinchOut { get; set; }

    double startDistance;
    double scale = 1;
    bool pinchStart;

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

        Clear(e);
    }
    void GestureLeave(object? sender, GestureEventArgs e)
    {
        if (e.MovePoints.Count < 2)
            return;

        if (pinchStart)
            AwarePinchEnd(e);

        Clear(e);
    }
    void Clear(GestureEventArgs e)
    {
        pinchStart = false;
        startDistance = 0;
    }

    void AwarePinch(GestureEventArgs e)
    {
        scale = (e.MovePoints[0].CalcDistance(e.MovePoints[1])) / startDistance;

        OnPinch.InvokeAsync(CreateEventArgs("pinch", e));
    }
    void AwarePinchEnd(GestureEventArgs e)
    {
        scale = (e.MovePoints[0].CalcDistance(e.MovePoints[1])) / startDistance;

        OnPinchEnd.InvokeAsync(CreateEventArgs("pinchend", e));

        if (Math.Abs(1 - scale) > MinScale)
        {
            if (scale > 1) //手势放大, 触发pinchout事件
            {
                OnPinchOut.InvokeAsync(CreateEventArgs("pinchout", e));
            }
            else if (scale < 1) //手势缩小,触发pinchin事件
            {
                OnPinchIn.InvokeAsync(CreateEventArgs("pinchin", e));
            }
        }
    }

    GesturePinchEventArgs CreateEventArgs(
        string type,
        GestureEventArgs e)
    {
        return new(e)
        {
            Type = type,
            Scale = scale,
        };
    }
}