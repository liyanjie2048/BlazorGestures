namespace Liyanjie.Blazor.Gestures.Components;

public class PanGestureRecognizer : ComponentBase
{
    [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public int Factor { get; set; } = 5;
    [Parameter] public EventCallback<GesturePanEventArgs> OnPan { get; set; }
    [Parameter] public EventCallback<GesturePanEventArgs> OnPanEnd { get; set; }

    bool panStart;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureMove += GestureMove;
            GestureRecognizer.GestureEnd += GestureEnd;
        }
    }

    void GestureMove(object? sender, GestureEventArgs e)
    {
        AwarePan(e);
    }
    void GestureEnd(object? sender, GestureEventArgs e)
    {
        if (panStart)
            AwarePanEnd(e);

        panStart = false;
    }

    void AwarePan(GestureEventArgs e)
    {
        panStart = true;

        OnPan.InvokeAsync(CreateEventArgs("pan", e));
    }
    void AwarePanEnd(GestureEventArgs e)
    {
        OnPanEnd.InvokeAsync(CreateEventArgs("panend", e));
    }

    GesturePanEventArgs CreateEventArgs(
        string type,
        GestureEventArgs e)
    {
        return new(e)
        {
            Type = type,
        };
    }
}