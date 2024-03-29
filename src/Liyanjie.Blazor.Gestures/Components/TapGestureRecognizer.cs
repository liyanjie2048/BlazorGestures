﻿namespace Liyanjie.Blazor.Gestures.Components;

public sealed class TapGestureRecognizer : ComponentBase
{
    [CascadingParameter] GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public int MaxDuration { get; set; } = 200;
    [Parameter] public double MaxDistance { get; set; } = 10;
    [Parameter] public EventCallback<TapGestureEventArgs> OnTap { get; set; }
    [Parameter] public bool AllowDoubleTap { get; set; } = true;
    [Parameter] public double MaxDoubleTapDistance { get; set; } = 20;
    [Parameter] public EventCallback<TapGestureEventArgs> OnDoubleTap { get; set; }

    DateTime lastTapTime;
    PointerEventArgs? lastTapPoint;
    Timer? timer;

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
        timer?.Dispose();
    }
    void GestureMove(object? sender, GestureEventArgs e)
    {
        if (e.Distance >= MaxDistance)
        {
            timer?.Dispose();
        }
    }
    void GestureEnd(object? sender, GestureEventArgs e)
    {
        timer?.Dispose();

        AwareTap(e);
    }
    void GestureLeave(object? sender, GestureEventArgs e)
    {
        timer?.Dispose();
    }

    void AwareTap(GestureEventArgs e)
    {
        if (e.Distance > MaxDistance)
            return;

        if (AllowDoubleTap
            && (e.StartTime - lastTapTime).TotalMilliseconds < MaxDuration
            && lastTapPoint is not null
            && lastTapPoint.CalcDistance(e.MovePoints[0]) < MaxDoubleTapDistance)
        {
            OnDoubleTap.InvokeAsync(CreateEventArgs("doubletap", e));
            lastTapPoint = null;
        }
        else if (e.Duration < MaxDuration)
        {
            lastTapTime = DateTime.Now;
            lastTapPoint = e.MovePoints[0];
            timer = Extensions.SetTimeout(() => InvokeAsync(() =>
            {
                timer?.Dispose();

                OnTap.InvokeAsync(CreateEventArgs("tap", e));
                lastTapPoint = null;
            }), MaxDuration);
        }
    }

    TapGestureEventArgs CreateEventArgs(string type, GestureEventArgs e) => new(e, type);
}