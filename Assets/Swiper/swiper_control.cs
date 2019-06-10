using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
//using UnityEngine;

public class SwiperControl : SwiperPlugin
{
    ///IconData for previous
    public readonly IconData iconPrevious;

    ///iconData fopr next
    public readonly IconData iconNext;

    ///icon size
    public readonly float size;

    ///Icon normal color, The theme's [ThemeData.primaryColor] by default.
    public readonly Color color;

    ///if set loop=false on Swiper, this color will be used when swiper goto the last slide.
    ///The theme's [ThemeData.disabledColor] by default.
    public readonly Color disableColor;

    public readonly EdgeInsets padding;

    public readonly Key key;

    public SwiperControl(
       Color color,
       Color disableColor,
       Key key=null,
       IconData iconPrevious = null,
       IconData iconNext = null,
       float size = 30.0f,
      EdgeInsets padding = null)
    {
        this.iconPrevious = iconPrevious ?? Icons.arrow_back_ios;
        this.iconNext = iconNext ?? Icons.arrow_forward_ios;
        this.color = color;
        this.disableColor = disableColor;
        this.key = key;
        this.size = 30.0f;
        this.padding = padding ?? EdgeInsets.all(5.0f);
    }

    Widget buildButton(SwiperPluginConfig config, Color color, IconData iconDaga,
        int quarterTurns, bool previous)
    {
        return new GestureDetector(
          behavior: HitTestBehavior.opaque,
          onTap: () =>
          {
              if (previous)
              {
                  config.controller.previous(animation: true);
              }
              else
              {
                  config.controller.next(animation: true);
              }
          },
          child: new Padding(
              padding: padding,
              child: new RotatedBox(
                  quarterTurns: quarterTurns,
                  child: new Icon(
                    iconDaga,
                    //semanticLabel: previous? "Previous" : "Next",
                    size: size,
                    color: color
                  )))
        );
    }

    //@override
    public override Widget build(BuildContext context, SwiperPluginConfig config)
    {
        ThemeData themeData = Theme.of(context);

        Color color = this.color ?? themeData.primaryColor;
        Color disableColor = this.disableColor ?? themeData.disabledColor;
        Color prevColor;
        Color nextColor;

        if (config.loop)
        {
            prevColor = nextColor = color;
        }
        else
        {
            bool next = config.activeIndex < config.itemCount - 1;
            bool prev = config.activeIndex > 0;
            prevColor = prev ? color : disableColor;
            nextColor = next ? color : disableColor;
        }
        Widget child;

        if (config.scrollDirection == Axis.horizontal)
        {
            child = new Row(
              key: key,
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: new List<Widget>{
            buildButton(config, prevColor, iconPrevious, 0, true),
            buildButton(config, nextColor, iconNext, 0, false)
              }


            );
        }
        else
        {
            child = new Column(
              key: key,
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: new List<Widget>{
            buildButton(config, prevColor, iconPrevious, -3, true),
            buildButton(config, nextColor, iconNext, -3, false)
              }


            );
        }

        return new Container(
          height: float.PositiveInfinity,
          child: child,
          width: float.PositiveInfinity
        );
    }
}