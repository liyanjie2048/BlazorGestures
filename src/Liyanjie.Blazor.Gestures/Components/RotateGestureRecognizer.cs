using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Liyanjie.Blazor.Gestures.Components;

public class RotateGestureRecognizer : ComponentBase
{
    [Inject] public IJSRuntime? JS { get; set; }
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public double MinAngle { get; set; } = 30;
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotate { get; set; }
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateEnd { get; set; }
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateLeft { get; set; }
    [Parameter] public EventCallback<GestureRotateEventArgs> OnRotateRight { get; set; }

    double __rotation;
    double startAngle;
    bool rotateStart;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureStarted += GestureStarted;
            GestureRecognizer.GestureMoved += GestureMoved;
            GestureRecognizer.GestureEnded += GestureEnded;
        }
    }

    void GestureStarted(object? sender, TouchEventArgs e)
    {
        if (e.Touches.Length >= 2)
        {
            startAngle = GetAngle180(GestureRecognizer.StartPoints[0], GestureRecognizer.StartPoints[1]);
        }
    }
    void GestureMoved(object? sender, TouchEventArgs e)
    {
        if (!GestureRecognizer.GestureStart)
            return;

        AwareRotate(e);
    }
    void GestureEnded(object? sender, TouchEventArgs e)
    {
        if (!GestureRecognizer.GestureStart)
            return;

        if (rotateStart)
            AwareRotateEnd(e);

        rotateStart = false;
        startAngle = 0;
    }

    void AwareRotate(TouchEventArgs e)
    {
        if (GestureRecognizer.CurrentPoints.Length < 2)
            return;

        if (e.IsGestureMove())
        {
            rotateStart = true;

            var currentAngle = GetAngle180(GestureRecognizer.CurrentPoints[0], GestureRecognizer.CurrentPoints[1]);
            var angleChange = GetAngleChange(currentAngle);

            OnRotate.InvokeAsync(CreateEventArgs("ROTATE",
                currentAngle,
                angleChange,
                angleChange > 0 ? GestureDirection.Right : GestureDirection.Left));
        }
    }
    void AwareRotateEnd(TouchEventArgs e)
    {
        if (GestureRecognizer.CurrentPoints.Length < 2)
            return;

        if (e.IsGestureEnd())
        {
            var currentAngle = GetAngle180(GestureRecognizer.CurrentPoints[0], GestureRecognizer.CurrentPoints[1]);
            var angleChange = GetAngleChange(currentAngle);

            OnRotateEnd.InvokeAsync(CreateEventArgs("ROTATEEND",
                currentAngle,
                angleChange,
                angleChange > 0 ? GestureDirection.Right : GestureDirection.Left));

            if (Math.Abs(angleChange) > MinAngle)
            {
                if (angleChange > 0)
                {
                    OnRotateRight.InvokeAsync(CreateEventArgs("ROTATE_RIGHT",
                        currentAngle,
                        angleChange,
                        GestureDirection.Right));
                }
                else
                {
                    OnRotateLeft.InvokeAsync(CreateEventArgs("ROTATE_LEFT",
                        currentAngle,
                        angleChange,
                        GestureDirection.Left));
                }
            }
        }
    }

    double GetAngleChange(double currentAngle)
    {
        var diff = startAngle - currentAngle;
        //var count = 0;

        //while (Math.Abs(diff - __rotation) > 90 && count++ < 50)
        //{
        //    if (__rotation < 0)
        //    {
        //        diff -= 180;
        //    }
        //    else
        //    {
        //        diff += 180;
        //    }
        //}
        __rotation = diff;
        return __rotation;
    }

    GestureRotateEventArgs CreateEventArgs(string type,
        double currentAngle,
        double angleChange,
        GestureDirection direction)
    {
        return new()
        {
            Type = type,
            StartPoints = GestureRecognizer.StartPoints,
            CurrentPoints = GestureRecognizer.CurrentPoints,
            GestureCount = GestureRecognizer.StartPoints.Length,
            GestureDuration = GestureRecognizer.GestureDuration,
            CurrentAngle = currentAngle,
            AngleChange = angleChange,
            Direction = direction,
        };
    }

    static double GetAngle180(TouchPoint p1, TouchPoint p2)
    {
        var angle = Math.Atan((p2.ClientY - p1.ClientY) * -1 / (p2.ClientX - p1.ClientX)) * (180 / Math.PI);
        return (angle < 0 ? (angle + 180) : angle);
    }
}