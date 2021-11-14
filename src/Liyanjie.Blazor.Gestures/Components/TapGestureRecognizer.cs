using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Liyanjie.Blazor.Gestures.Components;

public class TapGestureRecognizer : ComponentBase
{
    [Inject] public IJSRuntime? JS { get; set; }
    [CascadingParameter] public GestureArea? GestureArea { get; set; }

    [Parameter] public int MaxTime { get; set; } = 300;
    [Parameter] public double MaxDistance { get; set; } = 10;
    [Parameter] public EventCallback<GestureTapEventArgs> OnTap { get; set; }

    [Parameter] public bool AllowDoubleTap { get; set; } = false;
    [Parameter] public double MaxDoubleTapDistance { get; set; } = 20;
    [Parameter] public EventCallback<GestureTapEventArgs> OnDoubleTap { get; set; }

    DateTime lastTapTime;
    TouchPoint? lastTapPoint;
    Timer? timer;

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
        timer?.Dispose();
    }
    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureArea.GestureStart)
            return;

        var distance = GestureArea.StartPoints?[0].CalcDistance(GestureArea.CurrentPoints?[0]);
        if (distance > MaxDistance)
        {
            timer?.Dispose();
        }
    }
    void GestureEnded(object? sender, TouchEventArgs e)
    {
        if (!GestureArea.GestureStart)
            return;

        timer?.Dispose();

        AwareTap(e);
    }

    void AwareTap(TouchEventArgs e)
    {
        var distance = GestureArea.StartPoints?[0].CalcDistance(GestureArea.CurrentPoints?[0]);
        if (distance < MaxDistance)
        {
            bool isDoubleTap()
            {
                if (AllowDoubleTap)
                {
                    if ((GestureArea.GestureStartTime - lastTapTime).TotalMilliseconds < MaxTime)
                        return lastTapPoint is not null && lastTapPoint.CalcDistance(GestureArea.StartPoints?[0]) < MaxDoubleTapDistance;
                }
                return false;
            }

            if (isDoubleTap())
            {
                OnDoubleTap.InvokeAsync(CreateEventArgs("DOUBLETAP"));
                lastTapPoint = null;
            }
            else if (GestureArea.GestureDuration < MaxTime)
            {
                lastTapTime = DateTime.Now;
                lastTapPoint = GestureArea.StartPoints?[0];
                timer = Extensions.SetTimeout(() => InvokeAsync(() =>
                {
                    OnTap.InvokeAsync(CreateEventArgs("TAP"));
                    lastTapPoint = null;
                }), MaxTime);
            }
        }
    }

    GestureTapEventArgs CreateEventArgs(string type)
    {
        return new()
        {
            Type = type,
            StartPoints = GestureArea.StartPoints,
            CurrentPoints = GestureArea.CurrentPoints,
            GestureCount = GestureArea.StartPoints.Length,
            GestureDuration = GestureArea.GestureDuration,
        };
    }
}