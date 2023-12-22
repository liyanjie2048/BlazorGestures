namespace Liyanjie.Blazor.Gestures;

public record GestureSwipeEventArgs : GestureEventArgs
{
    public GestureSwipeEventArgs(GestureEventArgs e)
    {
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
        StartTime = e.StartTime;
    }
}