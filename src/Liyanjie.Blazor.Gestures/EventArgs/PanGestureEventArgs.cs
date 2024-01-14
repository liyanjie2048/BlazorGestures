namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record PanGestureEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="type"></param>
    public PanGestureEventArgs(GestureEventArgs e, string type)
        : base(type, e.StartTime, e.StartPoints, e.MovePoints, e.EdgeDistance) { }
}