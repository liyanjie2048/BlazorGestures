namespace Liyanjie.Blazor.Gestures;

public record GestureEventArgs
{
    public string? Type { get; init; }
    public DateTime? StartTime { get; init; }
    public IReadOnlyList<GesturePoint> StartPoints { get; init; } = default!;
    public IReadOnlyList<GesturePoint> MovePoints { get; init; } = default!;
    internal IEnumerable<(GesturePoint MovePoint, GesturePoint StartPoint)> CurrentPoints => MovePoints
        .Select(_ => (MovePoint: _, StartPoint: StartPoints.SingleOrDefault(__ => __.Identifier == _.Identifier)))
        .Where(_ => _.StartPoint is not null)
        .Select(_ => (_.MovePoint, _.StartPoint!));

    public int PointerCount => MovePoints.Count;
    public double? Duration => (DateTime.Now - StartTime)?.TotalMilliseconds;
    public double DistanceX => this.CalcDistanceX();
    public double DistanceY => this.CalcDistanceY();
    public double Distance => this.CalcDistance();
    public double Angle => this.CalcAngle();
    public GestureDirection Direction => this.Angle.CalcDirectionFromAngle();
}