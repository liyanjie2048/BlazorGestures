namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record RotateGestureEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="type"></param>
    public RotateGestureEventArgs(GestureEventArgs e, string type)
        : base(type, e.StartTime, e.StartPoints, e.MovePoints) { }

    /// <summary>
    /// 
    /// </summary>
    public double AngleChange { get; init; }
}
