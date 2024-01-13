namespace Liyanjie.Blazor.Gestures.Components;

public sealed class LongPressGestureRecognizer : ComponentBase
{
    [CascadingParameter] GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public int MinDuration { get; set; } = 500;
    [Parameter] public double MaxDistance { get; set; } = 10;
    [Parameter] public EventCallback<TapGestureEventArgs> OnLongPress { get; set; }

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
        timer?.Dispose(); //增加指点时用到

        AwareLongPress(e);
    }
    void GestureMove(object? sender, GestureEventArgs e)
    {
        if (e.Distance > MaxDistance)
        {
            timer?.Dispose();
        }
    }
    void GestureEnd(object? sender, GestureEventArgs e)
    {
        timer?.Dispose();
    }
    void GestureLeave(object? sender, GestureEventArgs e)
    {
        timer?.Dispose();
    }

    void AwareLongPress(GestureEventArgs e)
    {
        timer = Extensions.SetTimeout(() =>
        {
            timer?.Dispose();

            if (e.Distance <= MaxDistance)
                InvokeAsync(() => OnLongPress.InvokeAsync(CreateEventArgs("longpress", e)));
        }, MinDuration);
    }

    TapGestureEventArgs CreateEventArgs(string type, GestureEventArgs e) => new(e, type);
}