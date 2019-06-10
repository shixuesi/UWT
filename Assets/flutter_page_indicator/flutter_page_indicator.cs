using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
//using UnityEngine;

namespace BestFlutter.Page_Indicator
{
    public abstract class BasePainter : AbstractCustomPainter
    {
        public readonly PageIndicator widget;
        public readonly float page;
        public readonly int index;
        public readonly Paint _paint;

        public float lerp(float begin, float end, float progress)
        {
            return begin + (end - begin) * progress;
        }

        public BasePainter(PageIndicator widget, float page, int index, Paint _paint)
        {
            this.widget = widget; this.page = page;this.index = index; this._paint = _paint;
        }

        public abstract void draw(Canvas canvas, float space, float size, float radius);

        public virtual bool _shouldSkip(int index)
        {
            return false;
        }
        //double secondOffset = index == widget.count-1 ? radius : radius + ((index + 1) * (size + space));


        public override void paint(Canvas canvas, Size size)
        {
            _paint.color = widget.color;
            float space = widget.space;
            float _size = widget.size;
            float radius = _size / 2;
            for (int i = 0, c = widget.count; i < c; ++i)
            {
                if (_shouldSkip(i))
                {
                    continue;
                }
                canvas.drawCircle(
                    new Offset(i * (_size + space) + radius, radius), radius, _paint);
            }

            float page = this.page;
            if (page < index)
            {
                page = 0;
            }
            _paint.color = widget.activeColor;
            draw(canvas, space, _size, radius);
        }


        public override bool shouldRepaint(CustomPainter oldDelegate)
        {
            BasePainter bp = oldDelegate as BasePainter;
            return bp != null ? bp.page != page : false;//TODO:Check
        }
    }

    public class WarmPainter : BasePainter
    {
        public WarmPainter(PageIndicator widget, float page, int index, Paint paint) : base(widget, page, index, paint)
        {
        }

        public override void draw(Canvas canvas, float space, float size, float radius)
        {
            float progress = page - index;
            float distance = size + space;
            float start = index * (size + space);

            if (progress > 0.5f)
            {
                float right = start + size + distance;
                //progress=>0.5-1.0
                //left:0.0=>distance

                float left = index * distance + distance * (progress - 0.5f) * 2;
                canvas.drawRRect(
                     RRect.fromLTRBR(
                        left, 0.0f, right, size, Radius.circular(radius)),
                    _paint);
            }
            else
            {
                float right = start + size + distance * progress * 2;

                canvas.drawRRect(
                     RRect.fromLTRBR(
                        start, 0.0f, right, size, Radius.circular(radius)),
                    _paint);
            }
        }
    }

    class DropPainter : BasePainter
    {
        public DropPainter(PageIndicator widget, float page, int index, Paint paint) : base(widget, page, index, paint)
        {

        }


        public override void draw(Canvas canvas, float space, float size, float radius)
        {
            float progress = page - index;
            float dropHeight = widget.dropHeight;
            float rate = (0.5f - progress).abs() * 2;
            float scale = widget.scale;

            //lerp(begin, end, progress)

            canvas.drawCircle(
                new Offset(radius + ((page) * (size + space)),
                    radius - dropHeight * (1 - rate)),
                radius * (scale + rate * (1.0f - scale)),
                _paint);
        }
    }

    class NonePainter : BasePainter
    {
        public NonePainter(PageIndicator widget, float page, int index, Paint paint) : base(widget, page, index, paint)
        {
        }

        public override void draw(Canvas canvas, float space, float size, float radius)
        {
            float progress = page - index;
            float secondOffset = index == widget.count - 1
                ? radius
                : radius + ((index + 1) * (size + space));

            if (progress > 0.5f)
            {
                canvas.drawCircle(new Offset(secondOffset, radius), radius, _paint);
            }
            else
            {
                canvas.drawCircle(new Offset(radius + (index * (size + space)), radius),
                    radius, _paint);
            }
        }
    }

    class SlidePainter : BasePainter
    {
        public SlidePainter(PageIndicator widget, float page, int index, Paint paint) : base(widget, page, index, paint)
        {
        }


        public override void draw(Canvas canvas, float space, float size, float radius)
        {
            canvas.drawCircle(
                new Offset(radius + (page * (size + space)), radius), radius, _paint);
        }
    }

    class ScalePainter : BasePainter
    {
        public ScalePainter(PageIndicator widget, float page, int index, Paint paint) : base(widget, page, index, paint)
        {
        }

        // 连续的两个点，含有最后一个和第一个
        public override bool _shouldSkip(int i)
        {
            if (index == widget.count - 1)
            {
                return i == 0 || i == index;
            }
            return (i == index || i == index + 1);
        }

        public override void paint(Canvas canvas, Size size)
        {
            _paint.color = widget.color;
            float space = widget.space;
            float _size = widget.size;
            float radius = _size / 2;
            for (int i = 0, c = widget.count; i < c; ++i)
            {
                if (_shouldSkip(i))
                {
                    continue;
                }
                canvas.drawCircle(new Offset(i * (_size + space) + radius, radius),
                    radius * widget.scale, _paint);
            }

            _paint.color = widget.activeColor;
            draw(canvas, space, _size, radius);
        }


        public override void draw(Canvas canvas, float space, float size, float radius)
        {
            float secondOffset = index == widget.count - 1
                ? radius
                : radius + ((index + 1) * (size + space));

            float progress = page - index;
            _paint.color = Color.lerp(widget.activeColor, widget.color, progress);
            //last
            canvas.drawCircle(new Offset(radius + (index * (size + space)), radius),
                lerp(radius, radius * widget.scale, progress), _paint);
            //first
            _paint.color = Color.lerp(widget.color, widget.activeColor, progress);
            canvas.drawCircle(new Offset(secondOffset, radius),
                lerp(radius * widget.scale, radius, progress), _paint);
        }
    }


    class ColorPainter : BasePainter
    {
        public ColorPainter(PageIndicator widget, float page, int index, Paint paint) : base(widget, page, index, paint)
        { }

        // 连续的两个点，含有最后一个和第一个
        public override bool _shouldSkip(int i)
        {
            if (index == widget.count - 1)
            {
                return i == 0 || i == index;
            }
            return (i == index || i == index + 1);
        }

        public override void draw(Canvas canvas, float space, float size, float radius)
        {
            float progress = page - index;
            float secondOffset = index == widget.count - 1
                ? radius
                : radius + ((index + 1) * (size + space));

            _paint.color = Color.lerp(widget.activeColor, widget.color, progress);
            //left
            canvas.drawCircle(
                new Offset(radius + (index * (size + space)), radius), radius, _paint);
            //right
            _paint.color = Color.lerp(widget.color, widget.activeColor, progress);
            canvas.drawCircle(new Offset(secondOffset, radius), radius, _paint);
        }
    }

    public enum PageIndicatorLayout
    {
        NONE,
        SLIDE,
        WARM,
        COLOR,
        SCALE,
        DROP,
    }

    class _PageIndicatorState : State<PageIndicator>
    {
        int index = 0;
        Paint _paint = new Paint();

        BasePainter _createPainer()
        {
            switch (widget.layout)
            {
                case PageIndicatorLayout.NONE:
                    return new NonePainter(
                        widget, widget.controller.page, index, _paint);
                case PageIndicatorLayout.SLIDE:
                    return new SlidePainter(
                        widget, widget.controller.page, index, _paint);
                case PageIndicatorLayout.WARM:
                    return new WarmPainter(
                        widget, widget.controller.page, index, _paint);
                case PageIndicatorLayout.COLOR:
                    return new ColorPainter(
                        widget, widget.controller.page, index, _paint);
                case PageIndicatorLayout.SCALE:
                    return new ScalePainter(
                        widget, widget.controller.page, index, _paint);
                case PageIndicatorLayout.DROP:
                    return new DropPainter(
                        widget, widget.controller.page, index, _paint);
                default:
                    throw new Exception("Not a valid layout");
            }
        }

        public override Widget build(BuildContext context)
        {
            Widget child = new SizedBox(
              width: widget.count * widget.size + (widget.count - 1) * widget.space,
              height: widget.size,
              child: new CustomPaint(
                painter: _createPainer()
              )


            );

            if (widget.layout == PageIndicatorLayout.SCALE ||
                widget.layout == PageIndicatorLayout.COLOR)
            {
                child = new ClipRect(
                  child: child


                );
            }

            return new IgnorePointer(
              child: child


            );
        }

        private void _onController()
        {
            float page = widget.controller.page;
            index = page.floor();

            setState(() => { });
        }


        public override void initState()
        {
            widget.controller.addListener(_onController);
            base.initState();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget)
        {
            PageIndicator oldIndicator = oldWidget as PageIndicator;
            if (oldIndicator != null && widget.controller != oldIndicator.controller)
            {
                oldIndicator.controller.removeListener(_onController);
                widget.controller.addListener(_onController);
            }
            base.didUpdateWidget(oldWidget);
        }

        public override void dispose()
        {
            widget.controller.removeListener(_onController);
            base.dispose();
        }
    }
    public class PageIndicator : StatefulWidget
    {
        /// size of the dots
        public readonly float size;

        /// space between dots.
        public readonly float space;

        /// count of dots
        public readonly int count;

        /// active color
        public readonly Color activeColor;

        /// normal color
        public readonly Color color;

        /// layout of the dots,default is [PageIndicatorLayout.SLIDE]
        public readonly PageIndicatorLayout layout;

        // Only valid when layout==PageIndicatorLayout.scale
        public readonly float scale;

        // Only valid when layout==PageIndicatorLayout.drop
        public readonly float dropHeight;

        public readonly PageController controller;

        public readonly float activeSize;

        public PageIndicator(

       PageController controller,
       int count,
       Color color=null,
       Color activeColor =null,
       float size = 20.0f,
             Key key = null,
       float space = 5.0f,
       float activeSize = 20.0f,
       PageIndicatorLayout layout = PageIndicatorLayout.SLIDE,
       float scale = 0.6f,
       float dropHeight = 20.0f) : base(key: key)
        {
            D.assert(controller != null);
            this.space = space;
            this.size = size;
            this.count = count;
            this.activeColor = activeColor ?? Colors.red;
            this.activeSize = activeSize;
            this.controller = controller;
            this.color = color?? Colors.white30;
            this.layout = layout;
            this.scale = scale;
            this.dropHeight = dropHeight;
        }



        public override State createState()
        {
            return new _PageIndicatorState();
        }
    }
}
