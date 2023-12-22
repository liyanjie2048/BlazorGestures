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
            GestureRecognizer.GestureStart += GestureStart;
            GestureRecognizer.GestureMove += GestureMove;
            GestureRecognizer.GestureEnd += GestureEnd;
        }
    }

    void GestureStart(object? sender, GestureEventArgs e)
    {
        timer?.Dispose(); //增加指点时用到

        AwareLongPress(e);
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
    }

    void AwareLongPress(GestureEventArgs e)
    {
        timer = Extensions.SetTimeout(() =>
        {
            timer?.Dispose();

            if (e.Distance < MaxDistance)
                InvokeAsync(() => OnLongPress.InvokeAsync(CreateEventArgs("longpress", e)));
        }, MinTime);
    }

    GestureTapEventArgs CreateEventArgs(string type, GestureEventArgs e)
    {
        return new(e)
        {
            Type = type,
        };
    }
}