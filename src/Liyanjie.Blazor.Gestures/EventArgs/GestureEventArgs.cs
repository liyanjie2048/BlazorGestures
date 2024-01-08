namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record GestureEventArgs(
    string Type,
    DateTime StartTime,
    IReadOnlyList<PointerEventArgs> StartPoints,
    IReadOnlyList<PointerEventArgs> MovePoints)
{
    internal IEnumerable<(PointerEventArgs MovePoint, PointerEventArgs StartPoint)> CurrentPoints => MovePoints
        .Select(_ => (MovePoint: _, StartPoint: StartPoints.SingleOrDefault(__ => __.PointerId == _.PointerId)))
        .Where(_ => _.StartPoint is not null)
        .Select(_ => (_.MovePoint, _.StartPoint!));

    /// <summary>
    /// 
    /// </summary>
    public PointerEventArgs? PrimaryPoint => MovePoints.SingleOrDefault(_ => _.IsPrimary);

    /// <summary>
    /// 
    /// </summary>
    public int PointerCount => MovePoints.Count;

    /// <summary>
    /// 
    /// </summary>
    public double Duration => (DateTime.Now - StartTime).TotalMilliseconds;

    /// <summary>
    /// 
    /// </summary>
    public double DistanceX => CurrentPoints.Average(_ => _.MovePoint.ScreenX - _.StartPoint.ScreenX);

    /// <summary>
    /// 
    /// </summary>
    public double DistanceY => CurrentPoints.Average(_ => _.MovePoint.ScreenY - _.StartPoint.ScreenY);

    /// <summary>
    /// 
    /// </summary>
    public double Distance => Math.Sqrt((DistanceX * DistanceX) + (DistanceY * DistanceY));

    /// <summary>
    /// 
    /// </summary>
    public double Angle => CurrentPoints.Average(_ => _.StartPoint.CalcAngle(_.MovePoint));

    /// <summary>
    /// 
    /// </summary>
    public GestureDirection Direction => Angle switch
    {
        var v when v < -45 && v >= -135 => GestureDirection.Up,
        var v when v < -135 || v >= 135 => GestureDirection.Left,
        var v when v < 135 && v >= 45 => GestureDirection.Down,
        var v when v < 45 && v >= -45 => GestureDirection.Right,
        _ => 0,
    };
}