using Microsoft.AspNetCore.Components.Web;

namespace Liyanjie.Blazor.Gestures
{
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

        public static bool IsGestureMove(this TouchEventArgs e)
        {
            return (e.Type == "touchmove" || e.Type == "mousemove");
        }

        public static bool IsGestureEnd(this TouchEventArgs e)
        {
            return (e.Type == "touchend" || e.Type == "mouseup");
        }

        public static double CalcDistance(this TouchPoint? p1, TouchPoint? p2)
        {
            if (p1 is null || p2 is null)
                return 0;

            var x = p2.ClientX - p1.ClientX;
            var y = p2.ClientY - p1.ClientY;
            return Math.Sqrt((x * x) + (y * y));
        }

        public static double? CalcAngle(this TouchPoint? p1, TouchPoint? p2)
        {
            if (p1 is null || p2 is null)
                return default;

            return Math.Atan2(p2.ClientY - p1.ClientY, p2.ClientX - p1.ClientX) * 180 / Math.PI;
        }

        public static GestureDirection CalcDirectionFromAngle(this double? angle)
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
}
