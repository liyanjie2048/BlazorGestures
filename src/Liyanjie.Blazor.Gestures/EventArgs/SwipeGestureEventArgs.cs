namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record SwipeGestureEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="type"></param>
    public SwipeGestureEventArgs(GestureEventArgs e, string type)
        : base(type, e.StartTime, e.StartPoints, e.MovePoints, e.EdgeDistance) { }
}