namespace Liyanjie.Blazor.Gestures;

internal static class Extensions
{
    public static Timer SetTimeout(
        Action action,
        int delay_millionseconds)
    {
        return new(obj =>
        {
            action();
        }, null, delay_millionseconds, Timeout.Infinite);
    }

    public static double CalcDistanceX(this GestureEventArgs e)
    {
        return e.CurrentPoints.Average(_ => _.MovePoint.X - _.StartPoint.X);
    }

    public static double CalcDistanceY(this GestureEventArgs e)
    {
        return e.CurrentPoints.Average(_ => _.MovePoint.Y - _.StartPoint.Y);
    }

    public static double CalcDistance(this GestureEventArgs e)
    {
        var x = e.CalcDistanceX();
        var y = e.CalcDistanceY();
        return Math.Sqrt((x * x) + (y * y));
    }

    public static double CalcDistance(this GesturePoint p1, GesturePoint p2)
    {
        var x = p2.X - p1.X;
        var y = p2.Y - p1.Y;
        return Math.Sqrt((x * x) + (y * y));
    }

    public static double CalcAngle(this GestureEventArgs e)
    {
        return e.CurrentPoints.Average(_ => _.StartPoint.CalcAngle(_.MovePoint));
    }

    public static double CalcAngle(this GesturePoint p1, GesturePoint p2)
    {
        return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180 / Math.PI;
    }

    public static GestureDirection CalcDirectionFromAngle(this double angle)
    {
        return angle switch
        {
            var v when v < -45 && v > -135 => GestureDirection.Up,
            var v when v >= 45 && v < 135 => GestureDirection.Down,
            var v when v >= 135 || v <= -135 => GestureDirection.Left,
            var v when v >= -45 && v <= 45 => GestureDirection.Right,
            _ => 0,
        };
    }
}
