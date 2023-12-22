# Liyanjie.Blazor.Gestures

Blazor����ʶ��

- #### GestureRecognizer
    ��������ʶ����������ʶ������
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
         @onpointerdown=@(e=>gestureRecognizer?.PointerStart(e)) @onpointerdown:preventDefault
         @onpointermove=@(e=>gestureRecognizer?.PointerMove(e)) @onpointermove:preventDefault
         @onpointerup=@(e=>gestureRecognizer?.PointerEnd(e)) @onpointerup:preventDefault>
    </div>
    <GestureRecognizer @ref=@(gestureRecognizer) Enable=@(true)>
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
    <GestureArea @ref=@(gestureArea) Enable=@(true) class="gesture-area">
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
        <LongPressGestureRecognizer MinTime="default 500"  //ʶ��ΪLongPress����Сmillionseconds
                                    MaxDistance="default 10"  //ʶ��ΪTap�����touchmove distance
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
        <PinchGestureRecognizer MinScale="default 0"  //����PinchIn��PinchOut����Сscale
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
        <RotateGestureRecognizer MinAngle="default 30"  //����RotateLeft��RotateRight����Сangle
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
        <SwipeGestureRecognizer Direction="default Horizontal"  //������ϣ�Up|Down==Vertical or Left|Right == Horizontal or Up|Down|Left|Right == Horizontal|Vertical
                                MaxTime="default 300"  //ʶ��SwipeUp��SwipeDown��SwipeLeft��SwipeRight�����millionseconds
                                MinDistance="default 20"  //ʶ��ΪTap�����touchmove distance
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
        <TapGestureRecognizer MaxDistance="default 10"  //ʶ��ΪTap�����touchmove distance
                              MaxTime="default 300"  //ʶ��DoubleTap�����millionseconds
                              OnTap="callback"
                              AllowDoubleTap="default true"
                              MaxDoubleTapDistance="default 20"  ʶ��ΪDoubleTap�����touchstart distance
                              OnDoubleTap="callback" />
    </GestureRecognizer>
    ```
- #### �Զ�������ʶ��
    ```csharp
    public class CustomGestureRecognizer : ComponentBase
    {
        //Parent GestureRecognizer node
        [CascadingParameter] public GestureRecognizer? GestureRecognizer { get; set; }

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

        void GestureStarted(object? sender, GestureEventArgs e)
        {
            //Code here
        }
        void GestureMoved(object? sender, GestureEventArgs e)
        {
            //Code here
        }
        void GestureEnded(object? sender, GestureEventArgs e)
        {
            //Code here
        }
    }
    ```