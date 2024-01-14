namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record LongPressGestureEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="type"></param>
    public LongPressGestureEventArgs(GestureEventArgs e, string type)
        : base(type, e.StartTime, e.StartPoints, e.MovePoints, e.EdgeDistance) { }
}
