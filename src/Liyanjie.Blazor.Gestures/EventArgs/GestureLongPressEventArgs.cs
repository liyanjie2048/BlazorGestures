namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record GestureLongPressEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
#if NET8_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public GestureLongPressEventArgs(GestureEventArgs e)
    {
        StartTime = e.StartTime;
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
    }
}
