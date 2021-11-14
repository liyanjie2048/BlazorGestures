# Liyanjie.Blazor.Gestures

Blazor手势

- #### GestureArea
    声明手势区域，用于识别手势
  - Usage
    ```html
    <GestureArea AllowMouseSimulation="是否开启鼠标模拟，默认false">
        //ChildContent here
        //Recognizers here
    </GestureArea>
    ```
- #### LongPressGestureRecognizer
  - Usage
    ```html
    <GestureArea>
        //ChildContent here
        <LongPressGestureRecognizer MinTime="default 500"  //识别为LongPress的最小millionseconds
                                    MaxDistance="default 10"  //识别为Tap的最大touchmove distance
                                    OnLongPress="Your callback" />
    </GestureArea>
    ```
- #### PanGestureRecognizer
  - Usage
    ```html
    <GestureArea>
        //ChildContent here
        <PanGestureRecognizer OnPan="Your callback"
                              OnPanEnd="Your callback" />
    </GestureArea>
    ```
- #### PinchGestureRecognizer
  - Usage
    ```html
    <GestureArea>
        //ChildContent here
        <PinchGestureRecognizer MinScale="default 0"  //触发PinchIn、PinchOut的最小scale change
                                OnPinch="Your callback"
                                OnPinchEnd="Your callback"
                                OnPinchIn="Your callback"
                                OnPinchOut="Your callback" />
    </GestureArea>
    ```
- #### RotateGestureRecognizer
  - Usage
    ```html
    <GestureArea>
        //ChildContent here
        <RotateGestureRecognizer MinAngle="default 30"  //触发RotateLeft、RotateRight的最小angle change
                                 OnRotate="Your callback"
                                 OnRotateEnd="Your callback"
                                 OnRotateLeft="Your callback"
                                 OnRotateRight="Your callback" />
    </GestureArea>
    ```
- #### SwipeGestureRecognizer
  - Usage
    ```html
    <GestureArea>
        //ChildContent here
        <SwipeGestureRecognizer Direction="default Horizontal"  //可以组合：Up|Down==Vertical or Left|Right == Horizontal or Up|Down|Left|Right == Horizontal|Vertical
                                MaxTime="default 300"  //识别SwipeUp、SwipeDown、SwipeLeft、SwipeRight的最大millionseconds
                                MinDistance="default 20"  //识别为Tap的最大touchmove distance
                                OnSwipe="Your callback"
                                OnSwipeEnd="default false"
                                OnSwipeUp="Your callback"
                                OnSwipeDown="Your callback"
                                OnSwipeLeft="Your callback"
                                OnSwipeRight="Your callback" />
    </GestureArea>
    ```
- #### TapGestureRecognizer
  - Usage
    ```html
    <GestureArea>
        //ChildContent here
        <TapGestureRecognizer MaxDistance="default 10"  //识别为Tap的最大touchmove distance
                              MaxTime="default 300"  //识别DoubleTap的最大millionseconds
                              OnTap="Your callback"
                              AllowDoubleTap="default false"
                              MaxDoubleTapDistance="20"  识别为DoubleTap的最大touchstart distance
                              OnDoubleTap="Your callback" />
    </GestureArea>
    ```
