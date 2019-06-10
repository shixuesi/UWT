using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;


public class TransformInfo
{
    /// The `width` of the `TransformerPageView`
    float width;

    /// The `height` of the `TransformerPageView`
    float height;

    /// The `position` of the widget pass to [PageTransformer.transform]
    ///  A `position` describes how visible the widget is.
    ///  The widget in the center of the screen' which is  full visible, position is 0.0.
    ///  The widge in the left ,may be hidden, of the screen's position is less than 0.0, -1.0 when out of the screen.
    ///  The widge in the right ,may be hidden, of the screen's position is greater than 0.0,  1.0 when out of the screen
    ///
    ///
    public float position;

    /// The `index` of the widget pass to [PageTransformer.transform]
    int index;

    /// The `activeIndex` of the PageView
    int activeIndex;

    /// The `activeIndex` of the PageView, from user start to swipe
    /// It will change when user end drag
    public int fromIndex;

    /// Next `index` is greater than this `index`
    public bool forward;

    /// User drag is done.
    public bool done;

    /// Same as [TransformerPageView.viewportFraction]
    public float viewportFraction;

    /// Copy from [TransformerPageView.scrollDirection]
    Axis scrollDirection;

    public TransformInfo(int index, float position, float width, float height, int activeIndex, int fromIndex, bool forward, bool done, float viewportFraction, Axis scrollDirection)
    {
        this.index = index;
        this.position = position;
        this.width = width;
        this.height = height;
        this.activeIndex = activeIndex;
        this.fromIndex = fromIndex;
        this.forward = forward;
        this.done = done;
        this.viewportFraction = viewportFraction;
        this.scrollDirection = scrollDirection;
    }
}

public abstract class PageTransformer
{
    ///
    public readonly bool reverse;

    public PageTransformer(bool reverse = false)
    {
        this.reverse = reverse;
    }

    /// Return a transformed widget, based on child and TransformInfo
    public abstract Widget transformm(Widget child, TransformInfo info);
}



delegate Widget PageTransformerBuilderCallback(Widget child, TransformInfo info);


class PageTransformerBuilder : PageTransformer
{
    PageTransformerBuilderCallback builder;

    PageTransformerBuilder(PageTransformerBuilderCallback builder, bool reverse = false) : base(reverse: reverse)
    {
        this.builder = builder;
    }
    public override Widget transformm(Widget child, TransformInfo info)
    {
        return builder(child, info);
    }
}
class TransformerPageController : PageController
{
    public const int kMaxValue = 2000000000;
    public const int kMiddleValue = 1000000000;

    public readonly bool loop;
    public readonly int itemCount;
    public readonly bool reverse;

    public TransformerPageController(
        int initialPage = 0,
      bool keepPage = true,
      float viewportFraction = 1.0f,
      bool loop = false,
    int itemCount = 0,
    bool reverse = false
  ) : base(initialPage: _getRealIndexFromRenderIndex(initialPage, loop, itemCount, reverse), keepPage: keepPage, viewportFraction: viewportFraction)
    {
        this.loop = loop;
        this.itemCount = itemCount;
        this.reverse = reverse;
    }

    public int getRenderIndexFromRealIndex(int index)
    {
        return _getRenderIndexFromRealIndex(index, loop, itemCount, reverse);
    }

    public int getRealItemCount()
    {
        if (itemCount == 0) return 0;
        return loop ? itemCount + kMaxValue : itemCount;
    }

    private static int _getRenderIndexFromRealIndex(
        int index, bool loop, int itemCount, bool reverse)
    {
        if (itemCount == 0) return 0;
        int renderIndex;
        if (loop)
        {
            renderIndex = index - kMiddleValue;
            renderIndex = renderIndex % itemCount;
            if (renderIndex < 0)
            {
                renderIndex += itemCount;
            }
        }
        else
        {
            renderIndex = index;
        }
        if (reverse)
        {
            renderIndex = itemCount - renderIndex - 1;
        }

        return renderIndex;
    }

    public float realPage
    {
        get
        {
            float page;
            if (position.maxScrollExtent == 0 || position.minScrollExtent == 0)
            {
                page = 0;
            }
            else
            {
                page = base.page;
            }

            return page;
        }
    }

    private static float _getRenderPageFromRealPage(float page, bool loop, int itemCount, bool reverse)
    {
        float renderPage;
        if (loop)
        {
            renderPage = page - kMiddleValue;
            renderPage = renderPage % itemCount;
            if (renderPage < 0)
            {
                renderPage += itemCount;
            }
        }
        else
        {
            renderPage = page;
        }
        if (reverse)
        {
            renderPage = itemCount - renderPage - 1;
        }

        return renderPage;
    }

    public new float page
    {
        get
        {
            return loop
                ? _getRenderPageFromRealPage(realPage, loop, itemCount, reverse)
                : realPage;
        }
    }

    public int getRealIndexFromRenderIndex(int index)
    {
        return _getRealIndexFromRenderIndex(index, loop, itemCount, reverse);
    }

    static int _getRealIndexFromRenderIndex(
        int index, bool loop, int itemCount, bool reverse)
    {
        int result = reverse ? (itemCount - index - 1) : index;
        if (loop)
        {
            result += kMiddleValue;
        }
        return result;
    }
}
class TransformerPageView : StatefulWidget
{
    public const int kDefaultTransactionDuration = 300;
    /// Create a `transformed` widget base on the widget that has been passed to  the [PageTransformer.transform].
    /// See [TransformInfo]
    ///
    public PageTransformer transformer;

    /// Same as [PageView.scrollDirection]
    ///
    /// Defaults to [Axis.horizontal].
    public Axis scrollDirection;

    /// Same as [PageView.physics]
    public ScrollPhysics physics;

    /// Set to false to disable page snapping, useful for custom scroll behavior.
    /// Same as [PageView.pageSnapping]
    public bool pageSnapping;

    /// Called whenever the page in the center of the viewport changes.
    /// Same as [PageView.onPageChanged]
    public ValueChanged<int> onPageChanged;

    public IndexedWidgetBuilder itemBuilder;

    // See [IndexController.mode],[IndexController.next],[IndexController.previous]
    public IndexController controller;

    /// Animation duration
    public TimeSpan duration;

    /// Animation curve
    public Curve curve;

    public TransformerPageController pageController;

    /// Set true to open infinity loop mode.
    public readonly bool loop;

    /// This value is only valid when `pageController` is not set,
    public readonly int itemCount;

    /// This value is only valid when `pageController` is not set,
    public float viewportFraction;

    /// If not set, it is controlled by this widget.
    public readonly int index;

    /// Creates a scrollable list that works page by page using widgets that are
    /// created on demand.
    ///
    /// This constructor is appropriate for page views with a large (or infinite)
    /// number of children because the builder is called only for those children
    /// that are actually visible.
    ///
    /// Providing a non-null [itemCount] lets the [PageView] compute the maximum
    /// scroll extent.
    ///
    /// [itemBuilder] will be called only with indices greater than or equal to
    /// zero and less than [itemCount].
    public TransformerPageView(

      int index,
    Curve curve,
    ScrollPhysics physics,
    ValueChanged<int> onPageChanged,
    IndexController controller,
    PageTransformer transformer,
    IndexedWidgetBuilder itemBuilder,
    TransformerPageController pageController,
    int itemCount,
    TimeSpan? duration=null,
    Key key = null,
    float viewportFraction = 1,
    Axis scrollDirection = Axis.horizontal,
    bool loop = false,
    bool pageSnapping = true
  ) : base(key: key)
    {
        this.index = index;
        this.curve = curve == null ? Curves.ease : curve;
        this.loop = loop;
        this.scrollDirection = scrollDirection;
        this.physics = physics;
        this.onPageChanged = onPageChanged;
        this.controller = controller;
        this.transformer = transformer;
        this.itemBuilder = itemBuilder;
        this.pageController = pageController;
        this.itemCount = itemCount;
        this.viewportFraction = viewportFraction;
        this.scrollDirection = scrollDirection;
        this.loop = loop;
        this.pageSnapping = pageSnapping;
        this.duration = duration ?? TimeSpan.FromMilliseconds(kDefaultTransactionDuration);
    }

    public static TransformerPageView children(

          int index,
          TimeSpan? duration=null,
          Curve curve = null,
          Key key = null,
          ScrollPhysics physics = null,
          ValueChanged<int> onPageChanged = null,
          IndexController controller = null,
          TransformerPageController pageController = null,
          PageTransformer transformer = null,
           List<Widget> children = null,
          float viewportFraction = 1,
          bool pageSnapping = true,
          bool loop = false,
          Axis scrollDirection = Axis.horizontal
        )
    {

        return new TransformerPageView(
          itemCount: children.Count,
          itemBuilder: (BuildContext context, int ind) =>
          {
              return children[ind];
          },
          pageController: pageController,
          transformer: transformer,
          pageSnapping: pageSnapping,
          key: key,
          index: index,
          duration: duration,
          curve: curve ?? Curves.ease,
          viewportFraction: viewportFraction,
          scrollDirection: scrollDirection,
          physics: physics,
          onPageChanged: onPageChanged,
          controller: controller
        );
    }


    public override State createState()
    {
        return new _TransformerPageViewState();
    }

    public static int getRealIndexFromRenderIndex(bool reverse, int index, int itemCount, bool loop)
    {
        int initPage = reverse ? (itemCount - index - 1) : index;
        if (loop)
        {
            initPage += TransformerPageController.kMiddleValue;
        }
        return initPage;
    }

    static PageController createPageController(

    bool reverse,
        int index,
        int itemCount,
        bool loop,
        float viewportFraction)
    {
        return new PageController(
            initialPage: getRealIndexFromRenderIndex(
                reverse: reverse, index: index, itemCount: itemCount, loop: loop),
            viewportFraction: viewportFraction);
    }

}

class _TransformerPageViewState : State<TransformerPageView>
{
    Size _size;
    int _activeIndex;
    float _currentPixels;
    bool _done = false;

    ///This value will not change until user end drag.
    int _fromIndex;

    PageTransformer _transformer;

    TransformerPageController _pageController;

    Widget _buildItemNormal(BuildContext context, int index)
    {
        int renderIndex = _pageController.getRenderIndexFromRealIndex(index);
        Widget child = widget.itemBuilder(context, renderIndex);
        return child;
    }

    Widget _buildItem(BuildContext context, int index)
    {
        return new AnimatedBuilder(
            animation: _pageController,
            builder: (BuildContext c, Widget w) =>
            {
                int renderIndex = _pageController.getRenderIndexFromRealIndex(index);
                Widget child = null;
                if (widget.itemBuilder != null)
                {
                    child = widget.itemBuilder(context, renderIndex);
                }
                if (child == null)
                {
                    child = new Container();
                }
                if (_size == null)
                {
                    return child ?? new Container();
                }

                float position;

                float page = _pageController.realPage;

                if (_transformer.reverse)
                {
                    position = page - index;
                }
                else
                {
                    position = index - page;
                }
                position *= widget.viewportFraction;

                TransformInfo info = new TransformInfo(
                    index: renderIndex,
                    width: _size.width,
                    height: _size.height,
                    position: Mathf.Clamp(position, -1.0f, 1.0f),
                    activeIndex:
                        _pageController.getRenderIndexFromRealIndex(_activeIndex),
                    fromIndex: _fromIndex,
                    forward: _pageController.position.pixels - _currentPixels >= 0,
                    done: _done,
                    scrollDirection: widget.scrollDirection,
                    viewportFraction: widget.viewportFraction);
                return _transformer.transformm(child, info);
            });
    }

    float _calcCurrentPixels()
    {
        _currentPixels = _pageController.getRenderIndexFromRealIndex(_activeIndex) *
            _pageController.position.viewportDimension *
            widget.viewportFraction;

        //  print("activeIndex:$_activeIndex , pix:$_currentPixels");

        return _currentPixels;
    }


    public override Widget build(BuildContext context)
    {
        IndexedWidgetBuilder builder;
        if (_transformer == null)
            builder = _buildItemNormal;
        else
            builder = _buildItem;
        Widget child = new PageView(
      itemBuilder: builder,
      itemCount: _pageController.getRealItemCount(),
      onPageChanged: _onIndexChanged,
      controller: _pageController,
      scrollDirection: widget.scrollDirection,
      physics: widget.physics,
      pageSnapping: widget.pageSnapping,
      reverse: _pageController.reverse
    );
        if (_transformer == null)
        {
            return child;
        }
        return new NotificationListener<ScrollNotification>(
            onNotification: (ScrollNotification notification) =>
        {
            if (notification is ScrollStartNotification)
            {
                _calcCurrentPixels();
                _done = false;
                _fromIndex = _activeIndex;
            }
            else if (notification is ScrollEndNotification)
            {
                _calcCurrentPixels();
                _fromIndex = _activeIndex;
                _done = true;
            }

            return false;
        },
            child: child);
    }

    void _onIndexChanged(int index)
    {
        _activeIndex = index;
        if (widget.onPageChanged != null)
        {
            widget.onPageChanged(_pageController.getRenderIndexFromRealIndex(index));
        }
    }

    void _onGetSize(TimeSpan span)
    {
        //TODO:Check
        Size size = null;
        if (context == null)
        {
            onGetSize(size);
            return;
        }
        RenderObject renderObject = context.findRenderObject();
        if (renderObject != null)
        {
            Unity.UIWidgets.ui.Rect bounds = renderObject.paintBounds;
            if (bounds != null)
            {
                size = bounds.size;
            }
        }
        _calcCurrentPixels();
        onGetSize(size);
    }

    void onGetSize(Size size)
    {
        if (mounted)
        {
            setState(() =>
            {
                _size = size;
            });
        }

    }


    public override void initState()
    {
        _transformer = widget.transformer;
        //  int index = widget.index ?? 0;
        _pageController = widget.pageController;
        if (_pageController == null)
        {
            _pageController = new TransformerPageController(
                initialPage: widget.index,
                itemCount: widget.itemCount,
                loop: widget.loop,
                reverse:
                    widget.transformer == null ? false : widget.transformer.reverse);
        }
        // int initPage = _getRealIndexFromRenderIndex(index);
        // _pageController = new PageController(initialPage: initPage,viewportFraction: widget.viewportFraction);
        _fromIndex = _activeIndex = _pageController.initialPage;

        _controller = getNotifier();
        if (_controller != null)
        {
            _controller.addListener(onChangeNotifier);
        }
        base.initState();
    }


    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        _transformer = widget.transformer;
        int index = widget.index;
        bool created = false;
        if (_pageController != widget.pageController)
        {
            if (widget.pageController != null)
            {
                _pageController = widget.pageController;
            }
            else
            {
                created = true;
                _pageController = new TransformerPageController(
                    initialPage: widget.index,
                    itemCount: widget.itemCount,
                    loop: widget.loop,
                    reverse: widget.transformer == null
                        ? false
                        : widget.transformer.reverse);
            }
        }

        if (_pageController.getRenderIndexFromRealIndex(_activeIndex) != index)
        {
            _fromIndex = _activeIndex = _pageController.initialPage;
            if (!created)
            {
                int initPage = _pageController.getRealIndexFromRenderIndex(index);
                _pageController.animateToPage(initPage,
                    duration: widget.duration, curve: widget.curve);
            }
        }
        if (_transformer != null)
            WidgetsBinding.instance.addPostFrameCallback(_onGetSize);

        if (_controller != getNotifier())
        {
            if (_controller != null)
            {
                _controller.removeListener(onChangeNotifier);
            }
            _controller = getNotifier();
            if (_controller != null)
            {
                _controller.addListener(onChangeNotifier);
            }
        }
        base.didUpdateWidget(oldWidget);
    }


    public override void didChangeDependencies()
    {
        if (_transformer != null)
            WidgetsBinding.instance.addPostFrameCallback(_onGetSize);
        base.didChangeDependencies();
    }

    ChangeNotifier getNotifier()
    {
        return widget.controller;
    }

    int _calcNextIndex(bool next)
    {
        int currentIndex = _activeIndex;
        if (_pageController.reverse)
        {
            if (next)
            {
                currentIndex--;
            }
            else
            {
                currentIndex++;
            }
        }
        else
        {
            if (next)
            {
                currentIndex++;
            }
            else
            {
                currentIndex--;
            }
        }

        if (!_pageController.loop)
        {
            if (currentIndex >= _pageController.itemCount)
            {
                currentIndex = 0;
            }
            else if (currentIndex < 0)
            {
                currentIndex = _pageController.itemCount - 1;
            }
        }

        return currentIndex;
    }

    void onChangeNotifier()
    {
        int even = widget.controller.Event;
        int index;
        switch (even)
        {
            case IndexController.MOVE:
                {
                    index = _pageController.getRealIndexFromRenderIndex(widget.controller.index);
                }
                break;
            case IndexController.PREVIOUS:
            case IndexController.NEXT:
                {
                    index = _calcNextIndex(even == IndexController.NEXT);
                }
                break;
            default:
                //ignore this event
                return;
        }
        if (widget.controller.animation)
        {
            _pageController
                .animateToPage(index,
                    duration: widget.duration, curve: widget.curve ?? Curves.ease)
                .Done(widget.controller.complete);
        }
        else
        {
            _pageController.jumpToPage(index);
            widget.controller.complete();
        }
    }

    ChangeNotifier _controller;

    public override void dispose()
    {
        base.dispose();
        if (_controller != null)
        {
            _controller.removeListener(onChangeNotifier);
        }
    }
}

