namespace Liyanjie.Blazor.Gestures.EventArgs;

/// <summary>
/// 
/// </summary>
public record GestureEventArgs(
    string Type,
    DateTime StartTime,
    IReadOnlyList<PointerEventArgs> StartPoints,
    IReadOnlyList<PointerEventArgs> MovePoints,
    int EdgeDistance)
{
    internal IEnumerable<(PointerEventArgs MovePoint, PointerEventArgs StartPoint)> CurrentPoints => MovePoints
        .Select(_ => (MovePoint: _, StartPoint: StartPoints.SingleOrDefault(__ => __.PointerId == _.PointerId)))
        .Where(_ => _.StartPoint is not null)
        .Select(_ => (_.MovePoint, _.StartPoint!));

    /// <summary>
    /// 
    /// </summary>
    public PointerEventArgs? StartPrimaryPoint => StartPoints.SingleOrDefault(_ => _.IsPrimary);

    /// <summary>
    /// 
    /// </summary>
    public PointerEventArgs? MovePrimaryPoint => MovePoints.SingleOrDefault(_ => _.IsPrimary);

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
    public GestureEdge? StartEdge(int width, int height)
    {
        if (this.EdgeDistance <= 0 || this.StartPrimaryPoint is null)
            return default;

        var edge = GestureEdge.None;
        if (this.StartPrimaryPoint.OffsetX < this.EdgeDistance)
            edge |= GestureEdge.Left;
        if (this.StartPrimaryPoint.OffsetY < this.EdgeDistance)
            edge |= GestureEdge.Top;
        if (width > this.StartPrimaryPoint.OffsetX
           && (width - this.StartPrimaryPoint.OffsetX) < this.EdgeDistance)
            edge |= GestureEdge.Right;
        if (height > this.StartPrimaryPoint.OffsetY
            && (height - this.StartPrimaryPoint.OffsetY) < this.EdgeDistance)
            edge |= GestureEdge.Bottom;
        return edge;
    }
    public GestureEdge MoveEdge(int width, int height)
    {
        if (this.EdgeDistance <= 0 || this.MovePrimaryPoint is null)
            return default;

        var edge = GestureEdge.None;
        if (this.MovePrimaryPoint.OffsetX < this.EdgeDistance)
            edge |= GestureEdge.Left;
        if (this.MovePrimaryPoint.OffsetY < this.EdgeDistance)
            edge |= GestureEdge.Top;
        if (width > this.MovePrimaryPoint.OffsetX
            && (width - this.MovePrimaryPoint.OffsetX) < this.EdgeDistance)
            edge |= GestureEdge.Right;
        if (height > this.MovePrimaryPoint.OffsetY
            && (height - this.MovePrimaryPoint.OffsetY) < this.EdgeDistance)
            edge |= GestureEdge.Bottom;
        return edge;
    }
}