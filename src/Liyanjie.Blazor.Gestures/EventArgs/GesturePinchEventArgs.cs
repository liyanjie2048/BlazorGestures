namespace Liyanjie.Blazor.Gestures;

public record GesturePinchEventArgs : GestureEventArgs
{
    public double StartDistance { get; init; }
    public double CurrentDistance { get; init; }
    public double Scale { get; init; }
}