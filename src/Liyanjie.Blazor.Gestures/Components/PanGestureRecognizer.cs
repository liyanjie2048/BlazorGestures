using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Liyanjie.Blazor.Gestures.Components;

public class PanGestureRecognizer : ComponentBase
{
    [Inject] public IJSRuntime? JS { get; set; }
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public int Factor { get; set; } = 5;
    [Parameter] public EventCallback<GesturePanEventArgs> OnPan { get; set; }
    [Parameter] public EventCallback<GesturePanEventArgs> OnPanEnd { get; set; }

    bool panStart;

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
    }
    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureRecognizer.GestureStart)
            return;

        AwarePan(e);
    }
    void GestureEnded(object? sender, TouchEventArgs e)
    {
        if (!GestureRecognizer.GestureStart)
            return;

        if (panStart)
            AwarePanEnd(e);

        panStart = false;
    }

    void AwarePan(TouchEventArgs e)
    {
        if (e.IsGestureMove())
        {
            panStart = true;
            var distance = GestureRecognizer.StartPoints[0].CalcDistance(GestureRecognizer.CurrentPoints?[0]);
            var angle = GestureRecognizer.StartPoints[0].CalcAngle(GestureRecognizer.CurrentPoints[0]);
            var direction = angle.CalcDirectionFromAngle();
            var second = GestureRecognizer.GestureDuration / 1000;
            var factor = (10 - Factor) * 10 * second * second;

            OnPan.InvokeAsync(CreateEventArgs("PAN",
                angle,
                direction,
                distance,
                (10 - Factor) * 10 * second * second
            ));
        }
    }
    void AwarePanEnd(TouchEventArgs e)
    {
        if (e.IsGestureEnd())
        {
            var distance = GestureRecognizer.StartPoints[0].CalcDistance(GestureRecognizer.CurrentPoints[0]);
            var angle = GestureRecognizer.StartPoints[0].CalcAngle(GestureRecognizer.CurrentPoints[0]);
            var direction = angle.CalcDirectionFromAngle();
            var second = GestureRecognizer.GestureDuration / 1000;

            OnPanEnd.InvokeAsync(CreateEventArgs("PANEND",
                angle,
                direction,
                distance,
                (10 - Factor) * 10 * second * second
            ));
        }
    }

    GesturePanEventArgs CreateEventArgs(
        string type,
        double? angle,
        GestureDirection direction,
        double distance,
        int factor)
    {
        return new()
        {
            Type = type,
            StartPoints = GestureRecognizer.StartPoints,
            CurrentPoints = GestureRecognizer.CurrentPoints,
            GestureCount = GestureRecognizer.StartPoints.Length,
            GestureDuration = GestureRecognizer.GestureDuration,
            Angle = angle,
            Direction = direction,
            Distance = distance,
            Factor = factor,
        };
    }
}