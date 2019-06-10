using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
//using UnityEngine;
public interface Build
{
    Widget build(int i, float animationValue, Widget widget);
}
abstract class TransformBuilder<T> : Build
{
    public List<T> values;
    public TransformBuilder(List<T> values)
    {
        this.values = values;
    }
    public abstract Widget build(int i, float animationValue, Widget widget);
}

class ScaleTransformBuilder : TransformBuilder<float>
{
    public readonly Alignment alignment;
    public ScaleTransformBuilder(List<float> values, Alignment alignment = null) : base(values: values)
    {
        this.alignment = alignment ?? Alignment.center;
    }


    public override Widget build(int i, float animationValue, Widget widget)
    {
        float s = Unities._getValue(values, animationValue, i);
        return Transform.scale(scale: s, child: widget);
    }
}

class OpacityTransformBuilder : TransformBuilder<float>
{
    public OpacityTransformBuilder(List<float> values) : base(values: values)
    {
    }

    public override Widget build(int i, float animationValue, Widget widget)
    {
        float v = Unities._getValue(values, animationValue, i);
        return new Opacity(
          opacity: v,
          child: widget
        );
    }
}

class RotateTransformBuilder : TransformBuilder<float>
{
    public RotateTransformBuilder(List<float> values) : base(values: values)
    {
    }

    public override Widget build(int i, float animationValue, Widget widget)
    {
        float v = Unities._getValue(values, animationValue, i);
        return Transform.rotate(
          degree: v,
          child: widget
        );
    }
}

class TranslateTransformBuilder : TransformBuilder<Offset>
{
    public TranslateTransformBuilder(List<Offset> values) : base(values: values)
    {

    }


    public override Widget build(int i, float animationValue, Widget widget)
    {
        Offset s = Unities._getOffsetValue(values, animationValue, i);
        return Transform.translate(
          offset: s,
          child: widget
        );
    }
}

public class CustomLayoutOption
{
    public List<Build> builders = new List<Build>();
    public readonly int startIndex;
    public readonly int stateCount;

    CustomLayoutOption(int stateCount, int startIndex)
    {
        //D.assert(startIndex != null, stateCount != null);
        this.stateCount = stateCount; this.startIndex = startIndex;
    }

    CustomLayoutOption addOpacity(List<float> values)
    {
        builders.Add(new OpacityTransformBuilder(values: values));
        return this;
    }

    CustomLayoutOption addTranslate(List<Offset> values)
    {
        builders.Add(new TranslateTransformBuilder(values: values));
        return this;
    }

    CustomLayoutOption addScale(List<float> values, Alignment alignment)
    {
        builders
            .Add(new ScaleTransformBuilder(values: values, alignment: alignment));
        return this;
    }

    CustomLayoutOption addRotate(List<float> values)
    {
        builders.Add(new RotateTransformBuilder(values: values));
        return this;
    }
}

class _CustomLayoutSwiper : _SubSwiper
{
    public readonly CustomLayoutOption option;

    public _CustomLayoutSwiper(

        CustomLayoutOption option,
      float itemWidth,
      bool loop,
      float itemHeight,
      ValueChanged<int> onIndexChanged,

      IndexedWidgetBuilder itemBuilder,
      Curve curve,
      int duration,
      int index,
      int itemCount,
      Axis scrollDirection,

      SwiperController controller,
      Key key = null)
      : base(
            loop: loop,
            onIndexChanged: onIndexChanged,
            itemWidth: itemWidth,
            itemHeight: itemHeight,
            key: key,
            itemBuilder: itemBuilder,
            curve: curve,
            duration: duration,
            index: index,
            itemCount: itemCount,
            controller: controller,
            scrollDirection: scrollDirection)
    {
        D.assert(option != null);
        this.option = option;
    }


    public override State createState()
    {
        return new _CustomLayoutState();
    }
}

class _CustomLayoutState : _CustomLayoutStateBase<_CustomLayoutSwiper>
{

    public override void didChangeDependencies()
    {
        base.didChangeDependencies();
        _startIndex = widget.option.startIndex;
        _animationCount = widget.option.stateCount;
    }


    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        _CustomLayoutSwiper cs = oldWidget as _CustomLayoutSwiper;
        if (cs != null)
        {
            _startIndex = widget.option.startIndex;
            _animationCount = widget.option.stateCount;
        }
        else
        {
            UnityEngine.Debug.LogError("_CustomLayoutSwiper  为null");
        }
        base.didUpdateWidget(oldWidget);
    }

    public override Widget _buildItem(int index, int realIndex, float animationValue)
    {
        List<Build> builders = widget.option.builders;

        Widget child = new SizedBox(
            width: widget.itemWidth,
            height: widget.itemHeight,
            child: widget.itemBuilder(context, realIndex));

        for (int i = builders.Count - 1; i >= 0; --i)
        {
            Build builder = builders[i];
            child = builder.build(index, animationValue, child);
        }

        return child;
    }
}



abstract class _CustomLayoutStateBase<T> : SingleTickerProviderStateMixin<T> where T : _SubSwiper
{
    protected float _swiperWidth;
    protected float _swiperHeight;
    Animation<float> _animation;
    AnimationController _animationController;
    protected int _startIndex;
    protected int _animationCount;

    public override void initState()
    {
        if (widget.itemWidth == null)
        {
            throw new Exception(
                "==============\n\nwidget.itemWith must not be null when use stack layout.\n========\n");
        }

        _createAnimationController();
        widget.controller.addListener(_onController);
        base.initState();
    }

    void _createAnimationController()
    {
        _animationController = new AnimationController(vsync: this, value: 0.5f);
        Tween<float> tween = new FloatTween(begin: 0.0f, end: 1.0f);
        _animation = tween.animate(_animationController);
    }


    public override void didChangeDependencies()
    {
        WidgetsBinding.instance.addPostFrameCallback(_getSize);
        base.didChangeDependencies();
    }

    void _getSize(TimeSpan sp)
    {
        afterRender();
    }

    //@mustCallSuper
    public virtual void afterRender()
    {
        RenderObject renderObject = context.findRenderObject();
        Size size = renderObject.paintBounds.size;
        _swiperWidth = size.width;
        _swiperHeight = size.height;
        setState(() => { });
    }

    //@override
    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        _SubSwiper sw = oldWidget as _SubSwiper;
        if (sw != null && widget.controller != sw.controller)
        {
            sw.controller.removeListener(_onController);
            widget.controller.addListener(_onController);
        }

        if (widget.loop != sw.loop)
        {
            if (!widget.loop)
            {
                _currentIndex = _ensureIndex(_currentIndex);
            }
        }

        base.didUpdateWidget(oldWidget);
    }

    int _ensureIndex(int index)
    {
        index = index % widget.itemCount;
        if (index < 0)
        {
            index += widget.itemCount;
        }
        return index;
    }


    public override void dispose()
    {
        widget.controller.removeListener(_onController);
        _animationController?.dispose();
        base.dispose();
    }

    public abstract Widget _buildItem(int i, int realIndex, float animationValue);

    Widget _buildContainer(List<Widget> list)
    {
        return new Unity.UIWidgets.widgets.Stack(
          children: list
        );
    }

    Widget _buildAnimation(BuildContext context, Widget w)
    {
        List<Widget> list = new List<Widget>();

        float animationValue = _animation.value;

        for (int i = 0; i < _animationCount; ++i)
        {
            int realIndex = _currentIndex + i + _startIndex;
            realIndex = realIndex % widget.itemCount;
            if (realIndex < 0)
            {
                realIndex += widget.itemCount;
            }

            list.Add(_buildItem(i, realIndex, animationValue));
        }

        return new GestureDetector(
          behavior: HitTestBehavior.opaque,
          onPanStart: _onPanStart,
          onPanEnd: _onPanEnd,
          onPanUpdate: _onPanUpdate,
          child: new ClipRect(
            child: new Center(
              child: _buildContainer(list)

            )

          )

        );
    }


    public override Widget build(BuildContext context)
    {
        if (_animationCount == null)
        {
            return new Container();
        }
        return new AnimatedBuilder(
            animation: _animationController, builder: _buildAnimation);
    }

    float _currentValue;
    float _currentPos;

    bool _lockScroll = false;

    void _move(float position, int nextIndex) //async //TODO:修改async
    {
        if (_lockScroll) return;
        try
        {
            _lockScroll = true;
            /*await*/
            _animationController.animateTo(position,
      duration: TimeSpan.FromMilliseconds(widget.duration),
      curve: widget.curve).Done(() =>
      {
          if (nextIndex != null)
          {
              widget.onIndexChanged(widget.getCorrectIndex(nextIndex));
          }
          if (nextIndex != null)
          {
              try
              {
                  _animationController.setValue(0.5f);
              }
              catch (Exception e)
              {
                  UnityEngine.Debug.LogError(e);
              }

              _currentIndex = nextIndex;
          }
          _lockScroll = false;
      });
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
            if (nextIndex != null)
            {
                try
                {
                    _animationController.setValue(0.5f);
                }
                catch (Exception e2)
                {
                    UnityEngine.Debug.LogError(e2);
                }

                _currentIndex = nextIndex;
            }
            _lockScroll = false;
        }
        //finally
        //{
        //    if (nextIndex != null)
        //    {
        //        try
        //        {
        //            _animationController.setValue(0.5f);
        //        }
        //        catch (Exception e)
        //        {
        //            UnityEngine.Debug.LogError(e);
        //        }

        //        _currentIndex = nextIndex;
        //    }
        //    _lockScroll = false;
        //}
    }

    int _nextIndex()
    {
        int index = _currentIndex + 1;
        if (!widget.loop && index >= widget.itemCount - 1)
        {
            return widget.itemCount - 1;
        }
        return index;
    }

    int _prevIndex()
    {
        int index = _currentIndex - 1;
        if (!widget.loop && index < 0)
        {
            return 0;
        }
        return index;
    }

    void _onController()
    {
        switch (widget.controller.Event)
        {
            case IndexController.PREVIOUS:
                int prevIndex = _prevIndex();
                if (prevIndex == _currentIndex) return;
                _move(1.0f, nextIndex: prevIndex);
                break;
            case IndexController.NEXT:
                int nextIndex = _nextIndex();
                if (nextIndex == _currentIndex) return;
                _move(0.0f, nextIndex: nextIndex);
                break;
            case IndexController.MOVE:
                throw new Exception(
                    "Custom layout does not support SwiperControllerEvent.MOVE_INDEX yet!");
            case SwiperController.STOP_AUTOPLAY:
            case SwiperController.START_AUTOPLAY:
                break;
        }
    }

    void _onPanEnd(DragEndDetails details)
    {
        if (_lockScroll) return;

        float velocity = widget.scrollDirection == Axis.horizontal
            ? details.velocity.pixelsPerSecond.dx
            : details.velocity.pixelsPerSecond.dy;

        if (_animationController.value >= 0.75f || velocity > 500.0f)
        {
            if (_currentIndex <= 0 && !widget.loop)
            {
                return;
            }
            _move(1.0f, nextIndex: _currentIndex - 1);
        }
        else if (_animationController.value < 0.25f || velocity < -500.0f)
        {
            if (_currentIndex >= widget.itemCount - 1 && !widget.loop)
            {
                return;
            }
            _move(0.0f, nextIndex: _currentIndex + 1);
        }
        else
        {
            _move(0.5f, _currentIndex);
        }
    }

    void _onPanStart(DragStartDetails details)
    {
        if (_lockScroll) return;
        _currentValue = _animationController.value;
        _currentPos = widget.scrollDirection == Axis.horizontal
            ? details.globalPosition.dx
            : details.globalPosition.dy;
    }
    protected int _currentIndex = 0;
    void _onPanUpdate(DragUpdateDetails details)
    {
        if (_lockScroll) return;

        float value = _currentValue +
            ((widget.scrollDirection == Axis.horizontal
                        ? details.globalPosition.dx
                        : details.globalPosition.dy) -
                    _currentPos) /
                _swiperWidth /
                2;
        // no loop ?
        if (!widget.loop)
        {
            if (_currentIndex >= widget.itemCount - 1)
            {
                if (value < 0.5f)
                {
                    value = 0.5f;
                }
            }
            else if (_currentIndex <= 0)
            {
                if (value > 0.5f)
                {
                    value = 0.5f;
                }
            }
            _animationController.setValue(0.5f);
        }

    }
}



public static class Unities
{
    public static float _getValue(List<float> values, float animationValue, int index)
    {
        float s = values[index];
        if (animationValue >= 0.5)
        {
            if (index < values.Count - 1)
            {
                s = s + (values[index + 1] - s) * (animationValue - 0.5f) * 2.0f;
            }
        }
        else
        {
            if (index != 0)
            {
                s = s - (s - values[index - 1]) * (0.5f - animationValue) * 2.0f;
            }
        }
        return s;
    }

    public static Offset _getOffsetValue(List<Offset> values, float animationValue, int index)
    {
        Offset s = values[index];
        float dx = s.dx;
        float dy = s.dy;
        if (animationValue >= 0.5f)
        {
            if (index < values.Count - 1)
            {
                dx = dx + (values[index + 1].dx - dx) * (animationValue - 0.5f) * 2.0f;
                dy = dy + (values[index + 1].dy - dy) * (animationValue - 0.5f) * 2.0f;
            }
        }
        else
        {
            if (index != 0)
            {
                dx = dx - (dx - values[index - 1].dx) * (0.5f - animationValue) * 2.0f;
                dy = dy - (dy - values[index - 1].dy) * (0.5f - animationValue) * 2.0f;
            }
        }
        return new Offset(dx, dy);
    }
}
