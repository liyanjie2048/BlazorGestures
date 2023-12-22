namespace Liyanjie.Blazor.Gestures;

public record GesturePanEventArgs : GestureEventArgs
{
    public GesturePanEventArgs(GestureEventArgs e)
    {
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
        StartTime = e.StartTime;
    }
}