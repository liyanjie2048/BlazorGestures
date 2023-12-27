# Liyanjie.Blazor.Gestures

Blazor手势识别

- #### GestureRecognizer
    声明手势识别器，用于识别手势
  - Usage
    ```html
    <style>
        .gesture-aArea {
            -webkit-touch-callout: none !important;
            -webkit-user-select: none !important;
            -webkit-user-drag: none !important;
            touch-action: none !important;
            user-select: none !important
        }
    </style>
    <div class="gesture-area"
         @onpointerdown=@(e=>gestureRecognizer?.PointerDown(e)) @onpointerdown:preventDefault @onpointerdown:stopPropagation
         @onpointermove=@(e=>gestureRecognizer?.PointerMove(e)) @onpointermove:preventDefault @onpointermove:stopPropagation
         @onpointerup=@(e=>gestureRecognizer?.PointerUp(e)) @onpointerup:preventDefault @onpointerup:stopPropagation
         @onpointerleave=@(e=>gestureRecognizer?.PointerLeave(e)) @onpointerleave:preventDefault @onpointerleave:stopPropagation>
    </div>
    <GestureRecognizer @ref=@(gestureRecognizer)>
        //Recognizers here
    </GestureRecognizer>
    @code{
        GestureRecognizer? gestureRecognizer;
    }
    ```
  - Also
    ```html
    <style>
        .gesture-area {
            -webkit-touch-callout: none !important;
            -webkit-user-select: none !important;
            -webkit-user-drag: none !important;
            touch-action: none !important;
            user-select: none !important
        }
    </style>
    <GestureArea @ref=@(gestureArea) class="gesture-area">
        <ChildContent>
            //ChildContent here
        </ChildContent>
        <Recognizers>
            //Recognizers here
        </Recognizers>
    </GestureArea>
    ```
- #### LongPressGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <LongPressGestureRecognizer MinDuration="default 500"  //识别为LongPress的最小millionseconds
                                    MaxDistance="default 10"  //识别为Tap的最大pointermove distance
                                    OnLongPress="callback" />
    </GestureRecognizer>
    ```
- #### PanGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <PanGestureRecognizer OnPan="callback"
                              OnPanEnd="callback" />
    </GestureRecognizer>
    ```
- #### PinchGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <PinchGestureRecognizer MinScale="default 0"  //触发PinchIn、PinchOut的最小scale
                                OnPinch="callback"
                                OnPinchEnd="callback"
                                OnPinchIn="callback"
                                OnPinchOut="callback" />
    </GestureRecognizer>
    ```
- #### RotateGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <RotateGestureRecognizer MinAngle="default 10"  //触发RotateLeft、RotateRight的最小angle
                                 OnRotate="callback"
                                 OnRotateEnd="callback"
                                 OnRotateLeft="callback"
                                 OnRotateRight="callback" />
    </GestureRecognizer>
    ```
- #### SwipeGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <SwipeGestureRecognizer Direction="default GestureDirection.Horizontal"  //可以组合：Up|Down==Vertical or Left|Right == Horizontal or Up|Down|Left|Right == Horizontal|Vertical
                                MaxDuration="default 300"  //识别SwipeUp、SwipeDown、SwipeLeft、SwipeRight的最大millionseconds
                                MinDistance="default 20"  //识别SwipeUp、SwipeDown、SwipeLeft、SwipeRight的最大pointermove distance
                                OnSwipe="callback"
                                OnSwipeEnd="callback"
                                OnSwipeUp="callback"
                                OnSwipeDown="callback"
                                OnSwipeLeft="callback"
                                OnSwipeRight="callback" />
    </GestureRecognizer>
    ```
- #### TapGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <TapGestureRecognizer MaxDuration="default 200"  //识别DoubleTap的最大millionseconds
                              MaxDistance="default 10"  //识别为Tap的最大pointermove distance
                              OnTap="callback"
                              AllowDoubleTap="default true"
                              MaxDoubleTapDistance="default 20"  识别为DoubleTap的最大pointermove distance
                              OnDoubleTap="callback" />
    </GestureRecognizer>
    ```
- #### 自定义手势识别
    ```csharp
    public class CustomGestureRecognizer : ComponentBase
    {
        //Parent GestureRecognizer node
        [CascadingParameter] GestureRecognizer? GestureRecognizer { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (GestureRecognizer is not null)
            {
                GestureRecognizer.GestureStart += GestureStart;
                GestureRecognizer.GestureMove += GestureMove;
                GestureRecognizer.GestureEnd += GestureEnd;
                GestureRecognizer.GestureLeave += GestureLeave;
            }
        }

        void GestureStart(object? sender, GestureEventArgs e)
        {
            //Code here
        }
        void GestureMove(object? sender, GestureEventArgs e)
        {
            //Code here
        }
        void GestureEnd(object? sender, GestureEventArgs e)
        {
            //Code here
        }
        void GestureLeave(object? sender, GestureEventArgs e)
        {
            //Code here
        }
    }
    ```