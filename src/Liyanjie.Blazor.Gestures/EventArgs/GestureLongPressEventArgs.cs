namespace Liyanjie.Blazor.Gestures;

public record GestureLongPressEventArgs : GestureEventArgs
{
    public GestureLongPressEventArgs(GestureEventArgs e)
    {
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
        StartTime = e.StartTime;
    }
}
