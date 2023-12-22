namespace Liyanjie.Blazor.Gestures;

public record GestureRotateEventArgs : GestureEventArgs
{
    public GestureRotateEventArgs(GestureEventArgs e)
    {
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
        StartTime = e.StartTime;
    }

    public double AngleChange { get; init; }
}
