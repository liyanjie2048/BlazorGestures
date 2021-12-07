using Microsoft.AspNetCore.Components.Web;

namespace Liyanjie.Blazor.Gestures;

public abstract record GestureEventArgs
{
    public string? Type { get; init; }
    public TouchPoint[]? StartPoints { get; init; }
    public TouchPoint[]? CurrentPoints { get; init; }
    public int GestureCount { get; init; }
    public int GestureDuration { get; init; }
}