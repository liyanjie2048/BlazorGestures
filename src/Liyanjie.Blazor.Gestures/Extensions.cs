namespace Liyanjie.Blazor.Gestures;

internal static class Extensions
{
    public static Timer SetTimeout(
        Action action,
        int delayByMillionseconds)
    {
        return new(obj =>
        {
            action();
        }, null, delayByMillionseconds, Timeout.Infinite);
    }

    public static double CalcDistance(this PointerEventArgs p1, PointerEventArgs p2)
    {
        var x = p2.ScreenX - p1.ScreenX;
        var y = p2.ScreenY - p1.ScreenY;
        return Math.Sqrt((x * x) + (y * y));
    }

    public static double CalcAngle(this PointerEventArgs p1, PointerEventArgs p2)
    {
        return Math.Atan2(p2.ScreenY - p1.ScreenY, p2.ScreenX - p1.ScreenX) * 180 / Math.PI;
    }
}
