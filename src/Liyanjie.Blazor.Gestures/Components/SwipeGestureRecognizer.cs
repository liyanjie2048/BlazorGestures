namespace Liyanjie.Blazor.Gestures.Components;

/// <summary>
/// 
/// </summary>
public sealed class SwipeGestureRecognizer : ComponentBase
{
    [CascadingParameter] GestureRecognizer? GestureRecognizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public GestureDirection Direction { get; set; } = GestureDirection.Horizontal;

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public double MaxDuration { get; set; } = 300;

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public double MinDistance { get; set; } = 20;

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<SwipeGestureEventArgs> OnSwipe { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<SwipeGestureEventArgs> OnSwipeEnd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<SwipeGestureEventArgs> OnSwipeLeft { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<SwipeGestureEventArgs> OnSwipeRight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<SwipeGestureEventArgs> OnSwipeUp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback<SwipeGestureEventArgs> OnSwipeDown { get; set; }

    bool swipeStart;

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureMove += GestureMove;
            GestureRecognizer.GestureEnd += GestureEnd;
            GestureRecognizer.GestureLeave += GestureLeave;
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

        Clear(e);
    }
    void GestureLeave(object? sender, GestureEventArgs e)
    {
        if (swipeStart)
            AwareSwipeEnd(e);

        Clear(e);
    }
    void Clear(GestureEventArgs e)
    {
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

        if (e.Duration < MaxDuration)
        {
            if (Math.Abs(e.DistanceY) >= MinDistance)
            {
                _ = e.Direction switch
                {
                    GestureDirection.Up => OnSwipeUp.InvokeAsync(CreateEventArgs("swipeup", e)),
                    GestureDirection.Down => OnSwipeDown.InvokeAsync(CreateEventArgs("swipedown", e)),
                    _ => Task.CompletedTask,
                };
            }
            if (Math.Abs(e.DistanceX) >= MinDistance)
            {
                _ = e.Direction switch
                {
                    GestureDirection.Left => OnSwipeLeft.InvokeAsync(CreateEventArgs("swipeleft", e)),
                    GestureDirection.Right => OnSwipeRight.InvokeAsync(CreateEventArgs("swiperight", e)),
                    _ => Task.CompletedTask,
                };
            }
        }
    }

    SwipeGestureEventArgs CreateEventArgs(
        string type,
        GestureEventArgs e) => new(e, type);
}