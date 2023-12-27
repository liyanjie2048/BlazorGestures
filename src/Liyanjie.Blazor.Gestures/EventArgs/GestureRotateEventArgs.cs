namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record GestureRotateEventArgs : GestureEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
#if NET8_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public GestureRotateEventArgs(GestureEventArgs e)
    {
        StartTime = e.StartTime;
        StartPoints = e.StartPoints;
        MovePoints = e.MovePoints;
    }

    /// <summary>
    /// 
    /// </summary>
    public double AngleChange { get; init; }
}
