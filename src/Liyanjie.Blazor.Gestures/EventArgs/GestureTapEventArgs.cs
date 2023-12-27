namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record GestureTapEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
#if NET8_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public GestureTapEventArgs(GestureEventArgs e)
    {
        StartTime = e.StartTime;
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
    }
}
