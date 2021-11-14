namespace Liyanjie.Blazor.Gestures;

public record GesturePanEventArgs : GestureEventArgs
{
    public double? Angle { get; init; }
    public GestureDirection Direction { get; init; }
    public double Distance { get; init; }
    public double Factor { get; init; }
}