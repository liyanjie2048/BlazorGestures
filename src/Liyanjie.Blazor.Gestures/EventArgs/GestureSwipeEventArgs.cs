namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record GestureSwipeEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
#if NET8_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public GestureSwipeEventArgs(GestureEventArgs e)
    {
        StartTime = e.StartTime;
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
    }
}