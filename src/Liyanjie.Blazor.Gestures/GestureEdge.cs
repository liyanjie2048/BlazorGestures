namespace Liyanjie.Blazor.Gestures;

public enum GestureEdge
{
    /// <summary>
    /// None
    /// </summary>
    None = 0,

    /// <summary>
    /// Top = 1
    /// </summary>
    Top = 1,

    /// <summary>
    /// Bottom = 2
    /// </summary>
    Bottom = 2,

    /// <summary>
    /// Left = 4
    /// </summary>
    Left = 4,

    /// <summary>
    /// Right = 8
    /// </summary>
    Right = 8,

    /// <summary>
    /// TopLeft = 5 = Top|Left
    /// </summary>
    TopLeft = 5,

    /// <summary>
    /// TopRight = 9 = Top|Right
    /// </summary>
    TopRight = 9,

    /// <summary>
    /// BottomLeft = 6 = Bottom|Left
    /// </summary>
    BottomLeft = 6,

    /// <summary>
    /// BottomRight = 10 = Bottom|Right
    /// </summary>
    BottomRight = 10,
}
