# Liyanjie.Blazor.Gestures

Blazor����ʶ��

- #### GestureRecognizer
    ��������ʶ����������ʶ������
  - Usage
    ```html
    <style>
        #section {
            -webkit-touch-callout: none !important;
            -webkit-user-select: none !important;
            -webkit-user-drag: none !important;
            touch-action: none !important;
            user-select: none !important
        }
    </style>
    <section id="section"
        @ontouchstart=@(TouchStart) @ontouchstart:preventDefault
        @ontouchmove=@(TouchMove) @ontouchmove:preventDefault
        @ontouchend=@(TouchEnd) @ontouchend:preventDefault>
    </section>
    <GestureRecognizer Active=@(true) @ref=@(gestureRecognizer)>
        //Recognizers here
    </GestureRecognizer>
    @code{
        GestureRecognizer? gestureRecognizer;

        void TouchStart(TouchEventArgs e) => gestureRecognizer?.TouchStart(e);
        void TouchMove(TouchEventArgs e) => gestureRecognizer?.TouchMove(e);
        void TouchEnd(TouchEventArgs e) => gestureRecognizer?.TouchEnd(e);
    }
    ```
- #### LongPressGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <LongPressGestureRecognizer MinTime="default 500"  //ʶ��ΪLongPress����Сmillionseconds
                                    MaxDistance="default 10"  //ʶ��ΪTap�����touchmove distance
                                    OnLongPress="Your callback" />
    </GestureRecognizer>
    ```
- #### PanGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <PanGestureRecognizer OnPan="Your callback"
                              OnPanEnd="Your callback" />
    </GestureRecognizer>
    ```
- #### PinchGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <PinchGestureRecognizer MinScale="default 0"  //����PinchIn��PinchOut����Сscale change
                                OnPinch="Your callback"
                                OnPinchEnd="Your callback"
                                OnPinchIn="Your callback"
                                OnPinchOut="Your callback" />
    </GestureRecognizer>
    ```
- #### RotateGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <RotateGestureRecognizer MinAngle="default 30"  //����RotateLeft��RotateRight����Сangle change
                                 OnRotate="Your callback"
                                 OnRotateEnd="Your callback"
                                 OnRotateLeft="Your callback"
                                 OnRotateRight="Your callback" />
    </GestureRecognizer>
    ```
- #### SwipeGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <SwipeGestureRecognizer Direction="default Horizontal"  //������ϣ�Up|Down==Vertical or Left|Right == Horizontal or Up|Down|Left|Right == Horizontal|Vertical
                                MaxTime="default 300"  //ʶ��SwipeUp��SwipeDown��SwipeLeft��SwipeRight�����millionseconds
                                MinDistance="default 20"  //ʶ��ΪTap�����touchmove distance
                                OnSwipe="Your callback"
                                OnSwipeEnd="default false"
                                OnSwipeUp="Your callback"
                                OnSwipeDown="Your callback"
                                OnSwipeLeft="Your callback"
                                OnSwipeRight="Your callback" />
    </GestureRecognizer>
    ```
- #### TapGestureRecognizer
  - Usage
    ```html
    <GestureRecognizer>
        <TapGestureRecognizer MaxDistance="default 10"  //ʶ��ΪTap�����touchmove distance
                              MaxTime="default 300"  //ʶ��DoubleTap�����millionseconds
                              OnTap="Your callback"
                              AllowDoubleTap="default false"
                              MaxDoubleTapDistance="20"  ʶ��ΪDoubleTap�����touchstart distance
                              OnDoubleTap="Your callback" />
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

        void GestureStarted(object? sender, TouchEventArgs e)
        {
            //Your code
        }
        void GestureMoved(object? sender, TouchEventArgs e)
        {
            //Your code
        }
        void GestureEnded(object? sender, TouchEventArgs e)
        {
            //Your code
        }
    }
    ```