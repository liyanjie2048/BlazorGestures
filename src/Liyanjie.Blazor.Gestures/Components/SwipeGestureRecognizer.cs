namespace Liyanjie.Blazor.Gestures.Components;

public class SwipeGestureRecognizer : ComponentBase
{
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public GestureDirection Direction { get; set; } = GestureDirection.Horizontal;
    [Parameter] public double MaxTime { get; set; } = 300;
    [Parameter] public double MinDistance { get; set; } = 20;
    [Parameter] public EventCallback<GestureSwipeEventArgs> OnSwipe { get; set; }
    [Parameter] public EventCallback<GestureSwipeEventArgs> OnSwipeEnd { get; set; }
    [Parameter] public EventCallback<GestureSwipeEventArgs> OnSwipeLeft { get; set; }
    [Parameter] public EventCallback<GestureSwipeEventArgs> OnSwipeRight { get; set; }
    [Parameter] public EventCallback<GestureSwipeEventArgs> OnSwipeUp { get; set; }
    [Parameter] public EventCallback<GestureSwipeEventArgs> OnSwipeDown { get; set; }

    bool swipeStart;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureMove += GestureMove;
            GestureRecognizer.GestureEnd += GestureEnd;
        }
    }

    void GestureMove(object? sender, GestureEventArgs e)
    {
        AwareSwipe(e);
    }
    void GestureEnd(object? sender, GestureEventArgs e)
    {
        if (swipeStart)
            AwareSwipeEnd(e);

        swipeStart = false;
    }

    void AwareSwipe(GestureEventArgs e)
    {
        if (e.Direction == 0 || e.Direction != (Direction & e.Direction))
            return;

        swipeStart = true;

        OnSwipe.InvokeAsync(CreateEventArgs("swipe", e));
    }
    void AwareSwipeEnd(GestureEventArgs e)
    {
        if (e.Direction == 0 || e.Direction != (Direction & e.Direction))
            return;

        OnSwipeEnd.InvokeAsync(CreateEventArgs("swipeend", e));

        if (e.Duration < MaxTime && e.Direction switch
        {
            GestureDirection.Up or GestureDirection.Down or GestureDirection.Vertical => Math.Abs(e.DistanceY) >= MinDistance,
            GestureDirection.Left or GestureDirection.Right or GestureDirection.Horizontal => Math.Abs(e.DistanceX) >= MinDistance,
            _ => false
        })
        {
            var type = e.Direction switch
            {
                GestureDirection.Up => "swipeup",
                GestureDirection.Down => "swipedown",
                GestureDirection.Left => "swipeleft",
                GestureDirection.Right => "swiperight",
                _ => string.Empty,
            };
            var eventArgs = CreateEventArgs(type, e);
            _ = e.Direction switch
            {
                GestureDirection.Up => OnSwipeUp.InvokeAsync(eventArgs),
                GestureDirection.Down => OnSwipeDown.InvokeAsync(eventArgs),
                GestureDirection.Left => OnSwipeLeft.InvokeAsync(eventArgs),
                GestureDirection.Right => OnSwipeRight.InvokeAsync(eventArgs),
                _ => Task.CompletedTask,
            };
        }
    }

    GestureSwipeEventArgs CreateEventArgs(
        string type,
        GestureEventArgs e)
    {
        return new(e)
        {
            Type = type,
        };
    }
}