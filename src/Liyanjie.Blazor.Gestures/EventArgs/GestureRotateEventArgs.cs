namespace Liyanjie.Blazor.Gestures;

public record GestureRotateEventArgs : GestureEventArgs
{
    public double StartAngle { get; init; }
    public double CurrentAngle { get; init; }
    public GestureDirection Direction { get; init; }
    public double AngleChange { get; init; }
}
