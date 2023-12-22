namespace Liyanjie.Blazor.Gestures;

public record GesturePinchEventArgs : GestureEventArgs
{
    public GesturePinchEventArgs(GestureEventArgs e)
    {
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
        StartTime = e.StartTime;
    }

    public double Scale { get; init; }
}