using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
//using UnityEngine;

public delegate void PaintCallback(Canvas canvas, Size siz);
public class ColorPainter : AbstractCustomPainter
{
    private Paint _paint;
    public readonly TransformInfo info;
    public readonly List<Color> colors;

    ColorPainter(Paint _paint, TransformInfo info, List<Color> colors)
    {
        this._paint = _paint;
    }


    public override void paint(Canvas canvas, Size size)
    {
        int index = info.fromIndex;
        _paint.color = colors[index];
        canvas.drawRect(
             Rect.fromLTWH(0.0f, 0.0f, size.width, size.height), _paint);
        if (info.done)
        {
            return;
        }
        int alpha;
        long color;
        double opacity;
        double position = info.position;
        if (info.forward)
        {
            if (index < colors.Count - 1)
            {
                color = colors[index + 1].value & 0x00ffffff;
                opacity = (position <= 0
                    ? (-position / info.viewportFraction)
                    : 1 - position / info.viewportFraction);
                if (opacity > 1)
                {
                    opacity -= 1.0;
                }
                if (opacity < 0)
                {
                    opacity += 1.0;
                }
                alpha = (int)((0xff * opacity));

                _paint.color = new Color((alpha << 24) | color);
                canvas.drawRect(
                     Rect.fromLTWH(0.0f, 0.0f, size.width, size.height), _paint);
            }
        }
        else
        {
            if (index > 0)
            {
                color = colors[index - 1].value & 0x00ffffff;
                opacity = (position > 0
                    ? position / info.viewportFraction
                    : (1 + position / info.viewportFraction));
                if (opacity > 1)
                {
                    opacity -= 1.0;
                }
                if (opacity < 0)
                {
                    opacity += 1.0;
                }
                alpha = (int)(0xff * opacity);

                _paint.color = new Color((alpha << 24) | color);
                canvas.drawRect(
                     Rect.fromLTWH(0.0f, 0.0f, size.width, size.height), _paint);
            }
        }
    }


    public override bool shouldRepaint(CustomPainter oldDelegate)
    {
        ColorPainter colP = oldDelegate as ColorPainter;
        return colP != null ? colP.info != info : false;
    }



}
