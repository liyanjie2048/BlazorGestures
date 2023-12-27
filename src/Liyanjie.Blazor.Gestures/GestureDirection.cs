namespace Liyanjie.Blazor.Gestures;

/// <summary>
/// 
/// </summary>
[Flags]
public enum GestureDirection
{
    /// <summary>
    /// Up = 1
    /// </summary>
    Up = 1,

    /// <summary>
    /// Down = 2
    /// </summary>
    Down = 2,

    /// <summary>
    /// Left = 4
    /// </summary>
    Left = 4,

    /// <summary>
    /// Right = 8
    /// </summary>
    Right = 8,

    /// <summary>
    /// Vertical = 3 = Up|Down
    /// </summary>
    Vertical = 3,

    /// <summary>
    /// Horizontal = 12 = Left|Right
    /// </summary>
    Horizontal = 12,
}
