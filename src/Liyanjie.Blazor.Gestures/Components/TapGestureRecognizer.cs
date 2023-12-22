namespace Liyanjie.Blazor.Gestures.Components;

public class TapGestureRecognizer : ComponentBase
{
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public int MaxTime { get; set; } = 200;
    [Parameter] public double MaxDistance { get; set; } = 10;
    [Parameter] public EventCallback<GestureTapEventArgs> OnTap { get; set; }

    [Parameter] public bool AllowDoubleTap { get; set; } = true;
    [Parameter] public double MaxDoubleTapDistance { get; set; } = 20;
    [Parameter] public EventCallback<GestureTapEventArgs> OnDoubleTap { get; set; }

    DateTime lastTapTime;
    GesturePoint? lastTapPoint;
    Timer? timer;

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

    void AwareTap(GestureEventArgs e)
    {
        if (e.Distance >= MaxDistance)
            return;

        if (AllowDoubleTap
            && (e.StartTime - lastTapTime)?.TotalMilliseconds < MaxTime
            && lastTapPoint is not null
            && lastTapPoint.CalcDistance(e.MovePoints[0]) < MaxDoubleTapDistance)
        {
            OnDoubleTap.InvokeAsync(CreateEventArgs("doubletap", e));
            lastTapPoint = null;
        }
        else if (e.Duration < MaxTime)
        {
            lastTapTime = DateTime.Now;
            lastTapPoint = e.MovePoints[0];
            timer = Extensions.SetTimeout(() => InvokeAsync(() =>
            {
                timer?.Dispose();

                OnTap.InvokeAsync(CreateEventArgs("tap", e));
                lastTapPoint = null;
            }), MaxTime);
        }
    }

    GestureTapEventArgs CreateEventArgs(string type, GestureEventArgs e)
    {
        return new(e)
        {
            Type = type,
        };
    }
}