using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Liyanjie.Blazor.Gestures.Components;

public class PinchGestureRecognizer : ComponentBase
{
    [Inject] public IJSRuntime? JS { get; set; }
    [CascadingParameter] public GestureArea? GestureArea { get; set; }

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

        if (GestureArea is not null)
        {
            GestureArea.GestureStarted += GestureStarted;
            GestureArea.GestureMoved += GestureMoved;
            GestureArea.GestureEnded += GestureEnded;
        }
    }

    void GestureStarted(object? sender, TouchEventArgs e)
    {
        if (e.Touches.Length >= 2)
        {
            startDistance = GestureArea.StartPoints[0].CalcDistance(GestureArea.StartPoints[1]);
        }
    }

    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureArea.GestureStart)
            return;

        if (e.Touches.Length >= 2)
            AwarePinch(e);
    }

    void GestureEnded(object? sender, TouchEventArgs e)
    {
        if (!GestureArea.GestureStart)
            return;

        if (pinchStart)
            AwarePinchEnd(e);

        pinchStart = false;
        startDistance = 0;
    }

    void AwarePinch(TouchEventArgs e)
    {
        if (GestureArea.CurrentPoints.Length < 2)
            return;

        if (e.IsGestureMove())
        {
            pinchStart = true;

            var currentDistance = (GestureArea.CurrentPoints[0]).CalcDistance(GestureArea.CurrentPoints[1]);
            var scale = currentDistance / startDistance;

            OnPinch.InvokeAsync(CreateEventArgs("PINCH",
                currentDistance,
                scale));
        }
    }
    void AwarePinchEnd(TouchEventArgs e)
    {
        if (GestureArea.CurrentPoints.Length < 2)
            return;

        if (e.IsGestureEnd())
        {
            var currentDistance = (GestureArea.CurrentPoints[0]).CalcDistance(GestureArea.CurrentPoints[1]);
            var scale = currentDistance / startDistance;

            OnPinchEnd.InvokeAsync(CreateEventArgs("PINCHEND",
                currentDistance,
                scale));

            if (Math.Abs(1 - scale) > MinScale)
            {
                var scale_diff = 0.00000000001; //防止touchend的scale与__scale_last_rate相等，不触发事件的情况。
                if (scale > lastScale) //手势放大, 触发pinchout事件
                {
                    lastScale = scale - scale_diff;
                    OnPinchOut.InvokeAsync(CreateEventArgs("PINCH_OUT",
                        currentDistance,
                        scale));
                }
                else if (scale < lastScale) //手势缩小,触发pinchin事件
                {
                    lastScale = scale + scale_diff;
                    OnPinchIn.InvokeAsync(CreateEventArgs("PINCH_IN",
                        currentDistance,
                        scale));
                }

                lastScale = 1;
            }
        }
    }

    GesturePinchEventArgs CreateEventArgs(string type,
        double currentDistance,
        double scale)
    {
        return new()
        {
            Type = type,
            StartPoints = GestureArea.StartPoints,
            CurrentPoints = GestureArea.CurrentPoints,
            GestureCount = GestureArea.StartPoints.Length,
            GestureDuration = GestureArea.GestureDuration,
            StartDistance = startDistance,
            CurrentDistance = currentDistance,
            Scale = scale,
        };
    }
}