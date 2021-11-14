using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Liyanjie.Blazor.Gestures.Components;

public class SwipeGestureRecognizer : ComponentBase
{
    [Inject] public IJSRuntime? JS { get; set; }
    [CascadingParameter] public GestureArea? GestureArea { get; set; }

    [Parameter] public GestureDirection Direction { get; set; } = GestureDirection.Horizontal;
    [Parameter] public double MaxTime { get; set; } = 300;
    [Parameter] public double MinDistance { get; set; } = 20;
    [Parameter] public int Factor { get; set; } = 5;
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

        if (GestureArea is not null)
        {
            GestureArea.GestureStarted += GestureStarted;
            GestureArea.GestureMoved += GestureMoved;
            GestureArea.GestureEnded += GestureEnded;
        }
    }

    void GestureStarted(object? sender, TouchEventArgs e)
    {
    }
    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureArea.GestureStart)
            return;

        AwareSwipe(e);
    }
    void GestureEnded(object? sender, TouchEventArgs e)
    {
        if (!GestureArea.GestureStart)
            return;

        if (swipeStart)
            AwareSwipeEnd(e);

        swipeStart = false;
    }

    void AwareSwipe(TouchEventArgs e)
    {
        if (e.IsGestureMove())
        {
            swipeStart = true;

            var distanceX = GestureArea.CurrentPoints[0].ClientX - GestureArea.StartPoints[0].ClientX;
            var distanceY = GestureArea.CurrentPoints[0].ClientY - GestureArea.StartPoints[0].ClientY;
            var angle = GestureArea.StartPoints[0].CalcAngle(GestureArea.CurrentPoints[0]);
            var direction = angle.CalcDirectionFromAngle();
            var second = GestureArea.GestureDuration / 1000;

            if (direction > 0 && (Direction & direction) != direction)
                return;

            OnSwipe.InvokeAsync(CreateEventArgs("SWIPE",
                angle,
                direction,
                distanceX,
                distanceY,
                (10 - Factor) * 10 * second * second
            ));
        }
    }
    void AwareSwipeEnd(TouchEventArgs e)
    {
        if (e.IsGestureEnd())
        {
            var distanceX = GestureArea.CurrentPoints[0].ClientX - GestureArea.StartPoints[0].ClientX;
            var distanceY = GestureArea.CurrentPoints[0].ClientY - GestureArea.StartPoints[0].ClientY;
            var angle = GestureArea.StartPoints[0].CalcAngle(GestureArea.CurrentPoints[0]);
            var direction = angle.CalcDirectionFromAngle();
            var second = GestureArea.GestureDuration / 1000;
            var factor = (10 - Factor) * 10 * second * second;

            if (direction > 0 && (Direction & direction) != direction)
                return;

            OnSwipeEnd.InvokeAsync(CreateEventArgs("SWIPEEND",
                angle,
                direction,
                distanceX,
                distanceY,
                factor
            ));

            if (GestureArea.GestureDuration < MaxTime && direction switch
            {
                GestureDirection.Up or GestureDirection.Down or GestureDirection.Vertical => Math.Abs(distanceY) > MinDistance,
                GestureDirection.Left or GestureDirection.Right or GestureDirection.Horizontal => Math.Abs(distanceX) > MinDistance,
                _ => false
            })
            {
                var eventArgs = CreateEventArgs(direction switch
                {
                    GestureDirection.Up => "SWIPE_UP",
                    GestureDirection.Down => "SWIPE_DOWN",
                    GestureDirection.Left => "SWIPE_LEFT",
                    GestureDirection.Right => "SWIPE_RIGHT",
                    _ => string.Empty,
                },
                    angle,
                    direction,
                    distanceX,
                    distanceY,
                    factor);
                _ = direction switch
                {
                    GestureDirection.Up => OnSwipeUp.InvokeAsync(eventArgs),
                    GestureDirection.Down => OnSwipeDown.InvokeAsync(eventArgs),
                    GestureDirection.Left => OnSwipeLeft.InvokeAsync(eventArgs),
                    GestureDirection.Right => OnSwipeRight.InvokeAsync(eventArgs),
                    _ => Task.CompletedTask,
                };
            }
        }
    }

    GestureSwipeEventArgs CreateEventArgs(
        string type,
        double? angle,
        GestureDirection direction,
        double distanceX,
        double distanceY,
        double factor)
    {
        return new()
        {
            Type = type,
            StartPoints = GestureArea.StartPoints,
            CurrentPoints = GestureArea.CurrentPoints,
            GestureCount = GestureArea.StartPoints.Length,
            GestureDuration = GestureArea.GestureDuration,
            Angle = angle,
            Direction = direction,
            DistanceX = distanceX,
            DistanceY = distanceY,
            Factor = factor
        };
    }
}