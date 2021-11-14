namespace Liyanjie.Blazor.Gestures;

public record GestureSwipeEventArgs : GestureEventArgs
{
    public double? Angle { get; init; }
    public GestureDirection Direction { get; init; }
    public double DistanceX { get; init; }
    public double DistanceY { get; init; }
    public double Factor { get; init; }
}