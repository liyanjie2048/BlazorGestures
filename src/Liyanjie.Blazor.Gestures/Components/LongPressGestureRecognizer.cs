using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Liyanjie.Blazor.Gestures.Components;

public class LongPressGestureRecognizer : ComponentBase
{
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public int MinTime { get; set; } = 500;
    [Parameter] public double MaxDistance { get; set; } = 10;
    [Parameter] public EventCallback<GestureTapEventArgs> OnLongPress { get; set; }

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
        timer?.Dispose(); //增加指点时用到

        AwareLongPress();
    }
    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureRecognizer!.GestureStart)
            return;

        var distance = GestureRecognizer!.StartPoints![0].CalcDistance(GestureRecognizer!.CurrentPoints![0]);
        if (distance > MinTime)
        {
            timer?.Dispose();
        }
    }
    void GestureEnded(object? sender, TouchEventArgs e)
    {
        timer?.Dispose();
    }

    void AwareLongPress()
    {
        if (!GestureRecognizer!.GestureStart)
            return;

        timer = Extensions.SetTimeout(() =>
        {
            var distance = GestureRecognizer.StartPoints![0].CalcDistance(GestureRecognizer!.CurrentPoints![0]);
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
            StartPoints = GestureRecognizer?.StartPoints,
            CurrentPoints = GestureRecognizer?.CurrentPoints,
            GestureCount = GestureRecognizer!.StartPoints!.Length,
            GestureDuration = GestureRecognizer.GestureDuration,
        };
    }
}