namespace Liyanjie.Blazor.Gestures;

public record GestureTapEventArgs : GestureEventArgs
{
    public GestureTapEventArgs(GestureEventArgs e)
    {
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
        StartTime = e.StartTime;
    }
}
