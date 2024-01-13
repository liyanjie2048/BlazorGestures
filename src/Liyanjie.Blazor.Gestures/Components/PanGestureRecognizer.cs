namespace Liyanjie.Blazor.Gestures.Components;

public sealed class PanGestureRecognizer : ComponentBase
{
    [CascadingParameter] GestureRecognizer? GestureRecognizer { get; set; }

    [Parameter] public EventCallback<PanGestureEventArgs> OnPan { get; set; }
    [Parameter] public EventCallback<PanGestureEventArgs> OnPanEnd { get; set; }

    bool panStart;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (GestureRecognizer is not null)
        {
            GestureRecognizer.GestureMove += GestureMove;
            GestureRecognizer.GestureEnd += GestureEnd;
            GestureRecognizer.GestureLeave += GestureLeave;
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

        Clear(e);
    }
    void GestureLeave(object? sender, GestureEventArgs e)
    {
        if (panStart)
            AwarePanEnd(e);

        Clear(e);
    }
    void Clear(GestureEventArgs e)
    {
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

    PanGestureEventArgs CreateEventArgs(
        string type,
        GestureEventArgs e) => new(e, type);
}