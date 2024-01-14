# Liyanjie.Blazor.Gestures

Blazor����ʶ��

- #### GestureRecognizer
    ��������ʶ����������ʶ������
  - Usage
    ```razor
    <div class="gesturearea"
         @onpointerdown=@(e=>gestureRecognizer?.PointerDown(e)) @onpointerdown:preventDefault @onpointerdown:stopPropagation
         @onpointermove=@(e=>gestureRecognizer?.PointerMove(e)) @onpointermove:preventDefault @onpointermove:stopPropagation
         @onpointerup=@(e=>gestureRecognizer?.PointerUp(e)) @onpointerup:preventDefault @onpointerup:stopPropagation
         @onpointerleave=@(e=>gestureRecognizer?.PointerLeave(e)) @onpointerleave:preventDefault @onpointerleave:stopPropagation>
    </div>
    <GestureRecognizer @ref=@(gestureRecognizer)
                       EdgeDistance="default 75"                //ʶ��Ϊ��Ե�ľ���(GestureEventArgs.StartEdge(width,height))
                       Enable="default true">
        //Recognizers here
    </GestureRecognizer>
    @code{
        GestureRecognizer? gestureRecognizer;
    }
    ```
  - Also
    ```razor
    <GestureArea class="gesturearea"
                 EdgeDistance="default 75"                      //ʶ��Ϊ��Ե�ľ���(GestureEventArgs.StartEdge(width,height))
                 Enable="default true"
                 PreventDefault="default true"
                 StopPropagation="default true">
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
    ```razor
    <GestureRecognizer>
        <LongPressGestureRecognizer MinDuration="default 500"   //ʶ��ΪLongPress����Сmillionseconds
                                    MaxDistance="default 10"    //ʶ��ΪTap�����pointermove distance
                                    OnLongPress="callback" />
    </GestureRecognizer>
    ```
- #### PanGestureRecognizer
  - Usage
    ```razor
    <GestureRecognizer>
        <PanGestureRecognizer OnPan="callback"
                              OnPanEnd="callback" />
    </GestureRecognizer>
    ```
- #### PinchGestureRecognizer
  - Usage
    ```razor
    <GestureRecognizer>
        <PinchGestureRecognizer MinScale="default 0"            //����PinchIn��PinchOut����Сscale
                                OnPinch="callback"
                                OnPinchEnd="callback"
                                OnPinchIn="callback"
                                OnPinchOut="callback" />
    </GestureRecognizer>
    ```
- #### RotateGestureRecognizer
  - Usage
    ```razor
    <GestureRecognizer>
        <RotateGestureRecognizer MinAngle="default 10"          //����RotateCW��RotateCCW����Сangle
                                 OnRotate="callback"
                                 OnRotateEnd="callback"
                                 OnRotateCW="callback"
                                 OnRotateCCW="callback" />
    </GestureRecognizer>
    ```
- #### SwipeGestureRecognizer
  - Usage
    ```razor
    <GestureRecognizer>
        <SwipeGestureRecognizer Direction="default GestureDirection.Horizontal"  //������ϣ�Up|Down==Vertical or Left|Right == Horizontal or Up|Down|Left|Right == Horizontal|Vertical
                                MaxDuration="default 300"       //ʶ��SwipeUp��SwipeDown��SwipeLeft��SwipeRight�����millionseconds
                                MinDistance="default 20"        //ʶ��SwipeUp��SwipeDown��SwipeLeft��SwipeRight�����pointermove distance
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
    ```razor
    <GestureRecognizer>
        <TapGestureRecognizer MaxDuration="default 200"         //ʶ��DoubleTap�����millionseconds
                              MaxDistance="default 10"          //ʶ��ΪTap�����pointermove distance
                              OnTap="callback"
                              AllowDoubleTap="default true"
                              MaxDoubleTapDistance="default 20" //ʶ��ΪDoubleTap�����pointermove distance
                              OnDoubleTap="callback" />
    </GestureRecognizer>
    ```
- #### �Զ�������ʶ��
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