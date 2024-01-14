namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record PinchGestureEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="type"></param>
    public PinchGestureEventArgs(GestureEventArgs e, string type)
        : base(type, e.StartTime, e.StartPoints, e.MovePoints, e.EdgeDistance) { }

    /// <summary>
    /// 
    /// </summary>
    public double Scale { get; init; }
}