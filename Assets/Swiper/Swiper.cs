using BestFlutter.Page_Indicator;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Stack = Unity.UIWidgets.widgets.Stack;

public delegate void SwiperOnTap(int index);
public delegate Widget SwiperDataBuilder(BuildContext context, dynamic data, int index);
public enum SwiperLayout { DEFAULT, STACK, TINDER, CUSTOM }
public class Swiper : StatefulWidget
{
    /// default auto play delay
    public const int kDefaultAutoplayDelayMs = 3000;

    ///  Default auto play transition duration (in millisecond)
    public const int kDefaultAutoplayTransactionDuration = 300;

    public const int kMaxValue = 2000000000;
    public const int kMiddleValue = 1000000000;

    public readonly bool outer;

    public readonly float itemHeight;

    public readonly float itemWidth;

    // height of the inside container,this property is valid when outer=true,otherwise the inside container size is controlled by parent widget
    public readonly float containerHeight;

    // width of the inside container,this property is valid when outer=true,otherwise the inside container size is controlled by parent widget
    public readonly float containerWidth;

    /// Build item on index
    public readonly IndexedWidgetBuilder itemBuilder;

    /// Support transform like Android PageView did
    /// `itemBuilder` and `transformItemBuilder` must have one not null
    public readonly PageTransformer transformer;

    /// count of the display items
    public readonly int itemCount;

    public readonly ValueChanged<int> onIndexChanged;

    ///auto play config
    public readonly bool autoplay;

    ///Duration of the animation between transactions (in millisecond).
    public readonly int autoplayDelay;

    ///disable auto play when interaction
    public readonly bool autoplayDisableOnInteraction;

    ///auto play transition duration (in millisecond)
    public readonly int duration;

    ///horizontal/vertical
    public readonly Axis scrollDirection;

    ///transition curve
    public readonly Curve curve;

    /// Set to false to disable continuous loop mode.
    public readonly bool loop;

    ///Index number of initial slide.
    ///If not set , the `Swiper` is 'uncontrolled', which means manage index by itself
    ///If set , the `Swiper` is 'controlled', which means the index is fully managed by parent widget.
    public readonly int index;

    ///Called when tap
    public readonly SwiperOnTap onTap;

    ///The swiper pagination plugin
    public readonly SwiperPlugin pagination;

    ///the swiper control button plugin
    public readonly SwiperPlugin control;

    ///other plugins, you can custom your own plugin
    public readonly List<SwiperPlugin> plugins;

    ///
    public readonly SwiperController controller;

    public readonly ScrollPhysics physics;

    ///
    public readonly float viewportFraction;

    /// Build in layouts
    public readonly SwiperLayout layout;

    /// this value is valid when layout == SwiperLayout.CUSTOM
    public readonly CustomLayoutOption customLayoutOption;

    // This value is valid when viewportFraction is set and < 1.0
    public readonly float scale;

    // This value is valid when viewportFraction is set and < 1.0
    public readonly float fade;

    public readonly PageIndicatorLayout indicatorLayout;

    public Swiper(
    
        IndexedWidgetBuilder itemBuilder,
    int itemCount,
    SwiperPlugin pagination,
    SwiperPlugin control,
    /// since v1.0.0
    float containerHeight,
    float containerWidth,
    float itemHeight,
    float itemWidth,
    float scale=1,
    float fade=1,
    CustomLayoutOption customLayoutOption = null,

    SwiperController controller = null,
    PageTransformer transformer = null,
    int index =0,
    List<SwiperPlugin> plugins=null,
    ValueChanged<int> onIndexChanged =null,
    ScrollPhysics physics =null,
    ///
    SwiperOnTap onTap=null,

    Curve curve=null,
    Key key = null,
    float viewportFraction = 1.0f,
    bool autoplay = false,
    bool loop = true,
    Axis scrollDirection = Axis.horizontal,
    SwiperLayout layout = SwiperLayout.DEFAULT,
    int autoplayDelay = kDefaultAutoplayDelayMs,
    bool autoplayDisableOnInteraction = true,
    int duration = kDefaultAutoplayTransactionDuration,
    bool outer = false,

    PageIndicatorLayout indicatorLayout = PageIndicatorLayout.NONE
  ) :
        base(key: key)
    {
        D.assert(itemBuilder != null || transformer != null, () => { return "itemBuilder and transformItemBuilder must not be both null"; });
        D.assert(
            !loop ||
                ((loop &&
                        layout == SwiperLayout.DEFAULT &&
                        (indicatorLayout == PageIndicatorLayout.SCALE ||
                            indicatorLayout == PageIndicatorLayout.COLOR ||
                            indicatorLayout == PageIndicatorLayout.NONE)) ||
                    (loop && layout != SwiperLayout.DEFAULT)),
            () => "Only support `PageIndicatorLayout.SCALE` and `PageIndicatorLayout.COLOR`when layout==SwiperLayout.DEFAULT in loop mode");

        this.itemBuilder = itemBuilder;
        this.indicatorLayout = indicatorLayout;
        this.transformer = transformer;
        this.onIndexChanged = onIndexChanged;
        this.itemCount = itemCount;
        this.index = index;
        this.onTap = onTap;
        this.plugins = plugins;
        this.control = control;
        this.physics = physics;
        this.controller = controller;
        this.pagination = pagination;
        this.customLayoutOption = customLayoutOption;
        this.containerHeight = containerHeight;
        this.containerWidth = containerWidth;
        this.viewportFraction = viewportFraction;
        this.itemHeight = itemHeight;
        this.itemWidth = itemWidth;
        this.scale = scale;
        this.fade = fade;
        this.curve = curve ?? Curves.ease;
        this.autoplay = autoplay;
        this.loop = loop;
        this.scrollDirection = scrollDirection;
        this.layout = layout;
        this.autoplayDelay = autoplayDelay;
        this.autoplayDisableOnInteraction = autoplayDisableOnInteraction;
        this.duration = duration;
        this.outer = outer;

    }


    public static Swiper Children(
        Key key,
        IndexedWidgetBuilder itemBuilder,
    PageTransformer transformer,
    int itemCount,
    ValueChanged<int> onIndexChanged,
    int index,
    SwiperOnTap onTap,
    List<SwiperPlugin> plugins,
    SwiperPlugin control,
    ScrollPhysics physics,
    SwiperController controller,
    SwiperPlugin pagination,
    CustomLayoutOption customLayoutOption,
     List<Widget> children,
    /// since v1.0.0
    float containerHeight,
    float containerWidth,
    float itemHeight,
    float itemWidth,
    float scale,
    float fade,

    ///

    Curve curve,
    float viewportFraction = 1.0f,
    bool autoplay = false,
    bool loop = true,
    Axis scrollDirection = Axis.horizontal,
    SwiperLayout layout = SwiperLayout.DEFAULT,
    int autoplayDelay = kDefaultAutoplayDelayMs,
    bool autoplayDisableOnInteraction = true,
    int duration = kDefaultAutoplayTransactionDuration,
    bool outer = false,
    PageIndicatorLayout indicatorLayout = PageIndicatorLayout.NONE,
    bool reverse = false

  )
    {
        D.assert(children != null, () => "children must not be null");

        return new Swiper(
            transformer: transformer,
            customLayoutOption: customLayoutOption,
            containerHeight: containerHeight,
            containerWidth: containerWidth,
            viewportFraction: viewportFraction,
            itemHeight: itemHeight,
            itemWidth: itemWidth,
            outer: outer,
            scale: scale,
            fade: fade,
            autoplay: autoplay,
            autoplayDelay: autoplayDelay,
            autoplayDisableOnInteraction: autoplayDisableOnInteraction,
            duration: duration,
            onIndexChanged: onIndexChanged,
            index: index,
            onTap: onTap,
            curve: curve ?? Curves.ease,
            scrollDirection: scrollDirection,
            pagination: pagination,
            control: control,
            controller: controller,
            loop: loop,
            plugins: plugins,
            physics: physics,
            key: key,
            itemBuilder: (BuildContext context, int ind) =>
            {
                return children[ind];
            },
            itemCount: children.Count);
    }


    public static Swiper List(

        PageTransformer transformer,
        IList list,
    float containerHeight,
    float containerWidth,
    float itemHeight,
    float itemWidth,
    CustomLayoutOption customLayoutOption,
    SwiperDataBuilder builder,
    ValueChanged<int> onIndexChanged,
    Key key,
    int index,
    SwiperOnTap onTap,
    ScrollPhysics physics,
    Curve curve,
    float fade,
    SwiperPlugin pagination,
    SwiperPlugin control,
    List<SwiperPlugin> plugins,
    SwiperController controller,
    float viewportFraction = 1.0f,
    bool loop = true,
    int duration = kDefaultAutoplayTransactionDuration,
    Axis scrollDirection = Axis.horizontal,
    bool autoplay = false,
    int autoplayDelay = kDefaultAutoplayDelayMs,
    bool reverse = false,
    bool autoplayDisableOnInteraction = true,
    bool outer = false,
    float scale = 1.0f
  )
    {
        return new Swiper(
            transformer: transformer,
            customLayoutOption: customLayoutOption,
            containerHeight: containerHeight,
            containerWidth: containerWidth,
            viewportFraction: viewportFraction,
            itemHeight: itemHeight,
            itemWidth: itemWidth,
            outer: outer,
            scale: scale,
            autoplay: autoplay,
            autoplayDelay: autoplayDelay,
            autoplayDisableOnInteraction: autoplayDisableOnInteraction,
            duration: duration,
            onIndexChanged: onIndexChanged,
            index: index,
            onTap: onTap,
            curve: curve ?? Curves.ease,
            key: key,
            scrollDirection: scrollDirection,
            pagination: pagination,
            control: control,
            controller: controller,
            loop: loop,
            plugins: plugins,
            physics: physics,
            fade: fade,
            itemBuilder: (BuildContext context, int ind) =>
            {
                return builder(context, list[ind], ind);
            },
            itemCount: list.Count);
    }
    public override State createState()
    {
        return new _SwiperState();
    }


}



abstract class _SwiperTimerMixin : State<Swiper>
{
    public Timer _timer;

    protected SwiperController _controller;

    public override void initState()
    {
        _controller = widget.controller;
        if (_controller == null)
        {
            _controller = new SwiperController();
        }
        _controller.addListener(_onController);
        _handleAutoplay();
        base.initState();
    }

    void _onController()
    {
        switch (_controller.Event)
        {
            case SwiperController.START_AUTOPLAY:
                {
                    if (_timer == null)
                    {
                        _startAutoplay();
                    }
                }
                break;
            case SwiperController.STOP_AUTOPLAY:
                {
                    if (_timer != null)
                    {
                        _stopAutoplay();
                    }
                }
                break;
        }
    }


    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        Swiper sp = oldWidget as Swiper;
        if (sp != null && _controller != sp.controller)
        {
            if (sp.controller != null)
            {
                sp.controller.removeListener(_onController);
                _controller = sp.controller;
                _controller.addListener(_onController);
            }
        }
        _handleAutoplay();
        base.didUpdateWidget(oldWidget);
    }

    public override void dispose()
    {
        if (_controller != null)
        {
            _controller.removeListener(_onController);
            //  _controller.dispose();
        }

        _stopAutoplay();
        base.dispose();
    }

    bool _autoplayEnabled()
    {
        //return _controller.autoplay ?? widget.autoplay;
        return _controller.autoplay;
    }

    void _handleAutoplay()
    {
        if (_autoplayEnabled() && _timer != null) return;
        _stopAutoplay();
        if (_autoplayEnabled())
        {
            _startAutoplay();
        }
    }

    readonly TimerProvider _timerProvider = new TimerProvider();
    protected void _startAutoplay()
    {
        D.assert(_timer == null, () => "Timer must be stopped before start!");
        _timer = _timerProvider.periodic(TimeSpan.FromMilliseconds(widget.autoplayDelay), _onTimer);
    }

    //void _onTimer(Timer timer) {
    //      _controller.next(animation: true);
    //  }
    void _onTimer()
    {
        _controller.next(animation: true);
    }

    protected void _stopAutoplay()
    {
        if (_timer != null)
        {
            _timer.cancel();
            _timer = null;
        }
    }
}


class _SwiperState : _SwiperTimerMixin
{
    int _activeIndex;

    TransformerPageController _pageController;

    Widget _wrapTap(BuildContext context, int index)
    {
        return new GestureDetector(
          behavior: HitTestBehavior.opaque,
          onTap: () =>
          {
              this.widget.onTap(index);
          },
      child: widget.itemBuilder(context, index)
    );
    }


    public override void initState()
    {
        _activeIndex = widget.index;
        if (_isPageViewLayout())
        {
            _pageController = new TransformerPageController(
                initialPage: widget.index,
                loop: widget.loop,
                itemCount: widget.itemCount,
                reverse:
                    widget.transformer == null ? false : widget.transformer.reverse,
                viewportFraction: widget.viewportFraction);
        }
        base.initState();
    }

    bool _isPageViewLayout()
    {
        return widget.layout == SwiperLayout.DEFAULT;
    }


    public override void didChangeDependencies()
    {
        base.didChangeDependencies();
    }

    bool _getReverse(Swiper widget) =>
        widget.transformer == null ? false : widget.transformer.reverse;


    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        base.didUpdateWidget(oldWidget);
        Swiper sp = oldWidget as Swiper;
        if (_isPageViewLayout() && sp != null)
        {
            if (_pageController == null ||
                (widget.index != sp.index ||
                    widget.loop != sp.loop ||
                    widget.itemCount != sp.itemCount ||
                    widget.viewportFraction != sp.viewportFraction ||
                    _getReverse(widget) != _getReverse(sp)))
            {
                _pageController = new TransformerPageController(
                    initialPage: widget.index,
                    loop: widget.loop,
                    itemCount: widget.itemCount,
                    reverse: _getReverse(widget),
                    viewportFraction: widget.viewportFraction);
            }
        }
        else
        {
            //scheduleMicrotask(() =>
            //{
            //    // So that we have a chance to do `removeListener` in child widgets.
            //    if (_pageController != null)
            //    {
            //        _pageController.dispose();
            //        _pageController = null;
            //    }
            //});
        }
        //if (widget.index != null && widget.index != _activeIndex)
        //{
        //    _activeIndex = widget.index;
        //}
        //TODO:Check widget.index != null or widget.index != 0
        if (widget.index != _activeIndex)
        {
            _activeIndex = widget.index;
        }
    }

    void _onIndexChanged(int index)
    {
        setState(() =>
        {
            _activeIndex = index;
        });
        if (widget.onIndexChanged != null)
        {
            widget.onIndexChanged(index);
        }
    }

    Widget _buildSwiper()
    {
        IndexedWidgetBuilder itemBuilder;
        if (widget.onTap != null)
        {
            itemBuilder = _wrapTap;
        }
        else
        {
            itemBuilder = widget.itemBuilder;
        }

        if (widget.layout == SwiperLayout.STACK)
        {
            return new _StackSwiper(
              loop: widget.loop,
              itemWidth: widget.itemWidth,
              itemHeight: widget.itemHeight,
              itemCount: widget.itemCount,
              itemBuilder: itemBuilder,
              index: _activeIndex,
              curve: widget.curve,
              duration: widget.duration,
              onIndexChanged: _onIndexChanged,
              controller: _controller,
              scrollDirection: widget.scrollDirection


            );
        }
        else if (_isPageViewLayout())
        {
            PageTransformer transformer = widget.transformer;
            if (widget.scale != 0 || widget.fade != 0)
            {
                transformer =
                    new ScaleAndFadeTransformer(scale: widget.scale, fade: widget.fade);
            }

            Widget child = new TransformerPageView(
              pageController: _pageController,
              loop: widget.loop,
              itemCount: widget.itemCount,
              itemBuilder: itemBuilder,
              transformer: transformer,
              viewportFraction: widget.viewportFraction,
              index: _activeIndex,
              duration: TimeSpan.FromMilliseconds(widget.duration),
              scrollDirection: widget.scrollDirection,
              onPageChanged: _onIndexChanged,
              curve: widget.curve,
              physics: widget.physics,
              controller: _controller


            );
            if (widget.autoplayDisableOnInteraction && widget.autoplay)
            {
                return new NotificationListener<ScrollNotification>(
                  child: child,
                  onNotification: (ScrollNotification notification) =>
                {
                    ScrollStartNotification ssno = notification as ScrollStartNotification;
                    if (ssno != null)
                    {
                        if (ssno.dragDetails != null)
                        {
                            //by human
                            if (_timer != null) _stopAutoplay();
                        }
                    }
                    else if (notification is ScrollEndNotification)
                    {
                        if (_timer == null) _startAutoplay();
                    }

                    return false;
                }
            );
            }

            return child;
        }
        else if (widget.layout == SwiperLayout.TINDER)
        {
            return new _TinderSwiper(
              loop: widget.loop,
              itemWidth: widget.itemWidth,
              itemHeight: widget.itemHeight,
              itemCount: widget.itemCount,
              itemBuilder: itemBuilder,
              index: _activeIndex,
              curve: widget.curve,
              duration: widget.duration,
              onIndexChanged: _onIndexChanged,
              controller: _controller,
              scrollDirection: widget.scrollDirection
            );
        }
        else if (widget.layout == SwiperLayout.CUSTOM)
        {
            return new _CustomLayoutSwiper(
              loop: widget.loop,
              option: widget.customLayoutOption,
              itemWidth: widget.itemWidth,
              itemHeight: widget.itemHeight,
              itemCount: widget.itemCount,
              itemBuilder: itemBuilder,
              index: _activeIndex,
              curve: widget.curve,
              duration: widget.duration,
              onIndexChanged: _onIndexChanged,
              controller: _controller,
              scrollDirection: widget.scrollDirection
            );
        }
        else
        {
            return new Container();
        }
    }

    SwiperPluginConfig _ensureConfig(SwiperPluginConfig config)
    {
        if (config == null)
        {
            config = new SwiperPluginConfig(
                outer: widget.outer,
                itemCount: widget.itemCount,
                layout: widget.layout,
                indicatorLayout: widget.indicatorLayout,
                pageController: _pageController,
                activeIndex: _activeIndex,
                scrollDirection: widget.scrollDirection,
                controller: _controller,
                loop: widget.loop);
        }
        return config;
    }

    List<Widget> _ensureListForStack(
        Widget swiper, List<Widget> listForStack, Widget widget)
    {
        if (listForStack == null)
        {
            listForStack = new List<Widget> { swiper, widget };
        }
        else
        {
            listForStack.Add(widget);
        }
        return listForStack;
    }


    public override Widget build(BuildContext context)
    {
        Widget swiper = _buildSwiper();
        List<Widget> listForStack = null;
        SwiperPluginConfig config = null;
        if (widget.control != null)
        {
            //Stack
            config = _ensureConfig(config);
            listForStack = _ensureListForStack(
                swiper, listForStack, widget.control.build(context, config));
        }

        if (widget.plugins != null)
        {
            config = _ensureConfig(config);
            foreach (SwiperPlugin plugin in widget.plugins)
            {
                listForStack = _ensureListForStack(
                    swiper, listForStack, plugin.build(context, config));
            }
        }
        if (widget.pagination != null)
        {
            config = _ensureConfig(config);
            if (widget.outer)
            {
                return _buildOuterPagination(
                    widget.pagination,//TODO:Check  修改了 _buildOuterPagination的第一个参数
                    listForStack == null ? swiper : new Stack(children: listForStack),
                    config);
            }
            else
            {
                listForStack = _ensureListForStack(
                    swiper, listForStack, widget.pagination.build(context, config));
            }
        }

        if (listForStack != null)
        {
            return new Stack(
              children: listForStack


            );
        }

        return swiper;
    }

    Widget _buildOuterPagination(
        SwiperPlugin pagination, Widget swiper, SwiperPluginConfig config)
    {
        List<Widget> list = new List<Widget>();
        //Only support bottom yet!
        if (widget.containerHeight != 0 || widget.containerWidth != 0)
        {
            list.Add(swiper);
        }
        else
        {
            list.Add(new Expanded(child: swiper));
        }

        list.Add(new Align(
          alignment: Alignment.center,
          child: pagination.build(context, config)
        ));

        return new Column(
          children: list,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          mainAxisSize: MainAxisSize.min
        );
    }
}


abstract class _SubSwiper : StatefulWidget
{
    public readonly IndexedWidgetBuilder itemBuilder;
    public readonly int itemCount;
    public readonly int index;
    public readonly ValueChanged<int> onIndexChanged;
    public readonly SwiperController controller;
    public readonly int duration;
    public readonly Curve curve;
    public readonly float itemWidth;
    public readonly float itemHeight;
    public readonly bool loop;
    public readonly Axis scrollDirection;

    public _SubSwiper(
        Key key,
      bool loop,
      float itemHeight,
      float itemWidth,
      int duration,
      Curve curve,
      IndexedWidgetBuilder itemBuilder,
      SwiperController controller,
      int index,
      int itemCount,
      ValueChanged<int> onIndexChanged,
      Axis scrollDirection = Axis.horizontal)
      : base(key: key)
    {
        this.loop = loop;
        this.itemHeight = itemHeight;
        this.itemWidth = itemWidth;
        this.duration = duration;
        this.curve = curve ?? Curves.ease;
        this.itemBuilder = itemBuilder;
        this.controller = controller;
        this.index = index;
        this.itemCount = itemCount;
        this.onIndexChanged = onIndexChanged;
        this.scrollDirection = scrollDirection;
    }




    public int getCorrectIndex(int indexNeedsFix)
    {
        if (itemCount == 0) return 0;
        int value = indexNeedsFix % itemCount;
        if (value < 0)
        {
            value += itemCount;
        }
        return value;
    }
}


class _TinderSwiper : _SubSwiper
{
    public _TinderSwiper(

        Curve curve,
    int duration,
    SwiperController controller,
    ValueChanged<int> onIndexChanged,
      float itemHeight,
      float itemWidth,
    IndexedWidgetBuilder itemBuilder,
    int index,
      bool loop,
      int itemCount,
    Axis scrollDirection,
      Key key = null
  ) : base(
            loop: loop,
            key: key,
            itemWidth: itemWidth,
            itemHeight: itemHeight,
            itemBuilder: itemBuilder,
            curve: curve,
            duration: duration,
            controller: controller,
            index: index,
            onIndexChanged: onIndexChanged,
            itemCount: itemCount,
            scrollDirection: scrollDirection)
    {
        //D.assert(itemWidth != null && itemHeight != null);
    }


    public override State createState()
    {
        return new _TinderState();
    }
}


class _StackSwiper : _SubSwiper
{
    public _StackSwiper(
        Curve curve,
    int duration,
    SwiperController controller,
    ValueChanged<int> onIndexChanged,
      float itemHeight,
      float itemWidth,
    IndexedWidgetBuilder itemBuilder,
    int index,
      bool loop,
      int itemCount,
    Axis scrollDirection,
        Key key = null
  ) : base(
            loop: loop,
            key: key,
            itemWidth: itemWidth,
            itemHeight: itemHeight,
            itemBuilder: itemBuilder,
            curve: curve,
            duration: duration,
            controller: controller,
            index: index,
            onIndexChanged: onIndexChanged,
            itemCount: itemCount,
            scrollDirection: scrollDirection)
    {
    }


    public override State createState()
    {
        return new _StackViewState();
    }
}


class _TinderState : _CustomLayoutStateBase<_TinderSwiper>
{
    List<float> scales;
    List<float> offsetsX;
    List<float> offsetsY;
    List<float> opacity;
    List<float> rotates;

    float getOffsetY(float scale)
    {
        return widget.itemHeight - widget.itemHeight * scale;
    }


    public override void didChangeDependencies()
    {
        base.didChangeDependencies();
    }


    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        _updateValues();
        base.didUpdateWidget(oldWidget);
    }


    public override void afterRender()
    {
        base.afterRender();

        _startIndex = -3;
        _animationCount = 5;
        opacity = new List<float> { 0.0f, 0.9f, 0.9f, 1.0f, 0.0f, 0.0f };
        scales = new List<float> { 0.80f, 0.80f, 0.85f, 0.90f, 1.0f, 1.0f, 1.0f };
        rotates = new List<float> { 0.0f, 0.0f, 0.0f, 0.0f, 20.0f, 25.0f };
        _updateValues();
    }

    void _updateValues()
    {
        if (widget.scrollDirection == Axis.horizontal)
        {
            offsetsX = new List<float> { 0.0f, 0.0f, 0.0f, 0.0f, _swiperWidth, _swiperWidth };
            offsetsY = new List<float> {
              0.0f,
              0.0f,
              -5.0f,
              -10.0f,
              -15.0f,
              -20.0f,
            };
        }
        else
        {
            offsetsX = new List<float> {
              0.0f,
              0.0f,
              5.0f,
              10.0f,
              15.0f,
              20.0f,
            };

            offsetsY = new List<float> { 0.0f, 0.0f, 0.0f, 0.0f, _swiperHeight, _swiperHeight };
        }
    }


    public override Widget _buildItem(int i, int realIndex, float animationValue)
    {
        float s = Unities._getValue(scales, animationValue, i);
        float f = Unities._getValue(offsetsX, animationValue, i);
        float fy = Unities._getValue(offsetsY, animationValue, i);
        float o = Unities._getValue(opacity, animationValue, i);
        float a = Unities._getValue(rotates, animationValue, i);

        Alignment alignment = widget.scrollDirection == Axis.horizontal
            ? Alignment.bottomCenter
            : Alignment.centerLeft;

        return new Opacity(
          opacity: o,
          child: Transform.rotate(
            degree: a / 180.0f,
            child: Transform.translate(
              key: new ValueKey<int>(_currentIndex + i),
              offset: new Offset(f, fy),
              child: Transform.scale(
                scale: s,
                alignment: alignment,
                child: new SizedBox(
                  width: widget.itemWidth,
                  height: widget.itemHeight,
                  child: widget.itemBuilder(context, realIndex)
                )


              )


            )


          )


        );
    }
}



class _StackViewState : _CustomLayoutStateBase<_StackSwiper>
{
    List<float> scales;
    List<float> offsets;
    List<float> opacity;

    public override void didChangeDependencies()
    {
        base.didChangeDependencies();
    }

    void _updateValues()
    {
        if (widget.scrollDirection == Axis.horizontal)
        {
            float space = (_swiperWidth - widget.itemWidth) / 2;
            offsets = new List<float> { -space, -space / 3 * 2, -space / 3, 0.0f, _swiperWidth };
        }
        else
        {
            float space = (_swiperHeight - widget.itemHeight) / 2;
            offsets = new List<float> { -space, -space / 3 * 2, -space / 3, 0.0f, _swiperHeight };
        }
    }


    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        _updateValues();
        base.didUpdateWidget(oldWidget);
    }


    public override void afterRender()
    {
        base.afterRender();

        //length of the values array below
        _animationCount = 5;

        //Array below this line, '0' index is 1.0 ,witch is the first item show in swiper.
        _startIndex = -3;
        scales = new List<float> { 0.7f, 0.8f, 0.9f, 1.0f, 1.0f };
        opacity = new List<float> { 0.0f, 0.5f, 1.0f, 1.0f, 1.0f };

        _updateValues();
    }


    public override Widget _buildItem(int i, int realIndex, float animationValue)
    {
        float s = Unities._getValue(scales, animationValue, i);
        float f = Unities._getValue(offsets, animationValue, i);
        float o = Unities._getValue(opacity, animationValue, i);

        Offset offset = widget.scrollDirection == Axis.horizontal
            ? new Offset(f, 0.0f)
            : new Offset(0.0f, f);

        Alignment alignment = widget.scrollDirection == Axis.horizontal
            ? Alignment.centerLeft
            : Alignment.topCenter;

        return new Opacity(
          opacity: o,
          child: Transform.translate(
            key: new ValueKey<int>(_currentIndex + i),
            offset: offset,
            child: Transform.scale(
              scale: s,
              alignment: alignment,
              child: new SizedBox(
                width: widget.itemWidth,
                height: widget.itemHeight,
                child: widget.itemBuilder(context, realIndex)


              )


            )


          )


        );
    }
}

class ScaleAndFadeTransformer : PageTransformer
{
    private readonly float _scale;
    private readonly float _fade;

    public ScaleAndFadeTransformer(float fade = 0.3f, float scale = 0.8f)
    {
        _fade = fade;
        _scale = scale;
    }

    public override Widget transformm(Widget item, TransformInfo info)
    {
        float position = info.position;
        Widget child = item;
        if (_scale != null)
        {
            float scaleFactor = (1 - UnityEngine.Mathf.Abs(position)) * (1 - _scale);
            float scale = _scale + scaleFactor;

            child = Transform.scale(
              scale: scale,
              child: item


            );
        }

        if (_fade != null)
        {
            float fadeFactor = (1 - UnityEngine.Mathf.Abs(position)) * (1 - _fade);
            float opacity = _fade + fadeFactor;
            child = new Opacity(
              opacity: opacity,
              child: child


            );
        }

        return child;
    }
}