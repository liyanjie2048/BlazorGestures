using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Liyanjie.Blazor.Gestures.Components;

public class LongPressGestureRecognizer : ComponentBase
{
    [Inject] public IJSRuntime? JS { get; set; }
    [CascadingParameter] public GestureArea? GestureArea { get; set; }

    [Parameter] public int MinTime { get; set; } = 500;
    [Parameter] public double MaxDistance { get; set; } = 10;
    [Parameter] public EventCallback<GestureTapEventArgs> OnLongPress { get; set; }

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
        timer?.Dispose(); //增加指点时用到

        AwareLongPress(e);
    }
    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureArea.GestureStart)
            return;

        var distance = GestureArea.StartPoints?[0].CalcDistance(GestureArea.CurrentPoints?[0]);
        if (distance > MinTime)
        {
            timer?.Dispose();
        }
    }
    void GestureEnded(object? sender, TouchEventArgs e)
    {
        timer?.Dispose();
    }

    void AwareLongPress(TouchEventArgs ev)
    {
        if (!GestureArea.GestureStart)
            return;

        timer = Extensions.SetTimeout(() =>
        {
            var distance = GestureArea.StartPoints?[0].CalcDistance(GestureArea.CurrentPoints?[0]);
            if (distance > MaxDistance)
                return;

            InvokeAsync(() => OnLongPress.InvokeAsync(CreateEventArgs("LONGPRESS")));
            timer?.Dispose();
        }, MinTime);
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