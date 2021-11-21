using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Liyanjie.Blazor.Gestures.Components;

public class TapGestureRecognizer : ComponentBase
{
    [Inject] public IJSRuntime? JS { get; set; }
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

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

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureStarted += GestureStarted;
            GestureRecognizer.GestureMoved += GestureMoved;
            GestureRecognizer.GestureEnded += GestureEnded;
        }
    }

    void GestureStarted(object? sender, TouchEventArgs e)
    {
        timer?.Dispose();
    }
    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureRecognizer.GestureStart)
            return;

        var distance = GestureRecognizer.StartPoints?[0].CalcDistance(GestureRecognizer.CurrentPoints?[0]);
        if (distance > MaxDistance)
        {
            timer?.Dispose();
        }
    }
    void GestureEnded(object? sender, TouchEventArgs e)
    {
        if (!GestureRecognizer.GestureStart)
            return;

        timer?.Dispose();

        AwareTap(e);
    }

    void AwareTap(TouchEventArgs e)
    {
        var distance = GestureRecognizer.StartPoints?[0].CalcDistance(GestureRecognizer.CurrentPoints?[0]);
        if (distance < MaxDistance)
        {
            bool isDoubleTap()
            {
                if (AllowDoubleTap)
                {
                    if ((GestureRecognizer.GestureStartTime - lastTapTime).TotalMilliseconds < MaxTime)
                        return lastTapPoint is not null && lastTapPoint.CalcDistance(GestureRecognizer.StartPoints?[0]) < MaxDoubleTapDistance;
                }
                return false;
            }

            if (isDoubleTap())
            {
                OnDoubleTap.InvokeAsync(CreateEventArgs("DOUBLETAP"));
                lastTapPoint = null;
            }
            else if (GestureRecognizer.GestureDuration < MaxTime)
            {
                lastTapTime = DateTime.Now;
                lastTapPoint = GestureRecognizer.StartPoints?[0];
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
            StartPoints = GestureRecognizer.StartPoints,
            CurrentPoints = GestureRecognizer.CurrentPoints,
            GestureCount = GestureRecognizer.StartPoints.Length,
            GestureDuration = GestureRecognizer.GestureDuration,
        };
    }
}