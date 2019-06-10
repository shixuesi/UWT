using BestFlutter.Page_Indicator;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
//using UnityEngine;

class FractionPaginationBuilder : SwiperPlugin
{
    ///color ,if set null , will be Theme.of(context).scaffoldBackgroundColor
    public readonly Color color;

    ///color when active,if set null , will be Theme.of(context).primaryColor
    public readonly Color activeColor;

    ////font size
    public readonly float fontSize;

    ///font size when active
    public readonly float activeFontSize;

    public readonly Key key;

    public FractionPaginationBuilder(
         Color color = null,
       Key key = null,
       Color activeColor = null,
       float fontSize = 20.0f,
       float activeFontSize = 35.0f)
    {
        this.color = color;
        this.fontSize = fontSize;
        this.key = key;
        this.activeColor = activeColor;
        this.activeFontSize = activeFontSize;
    }


    public override Widget build(BuildContext context, SwiperPluginConfig config)
    {
        ThemeData themeData = Theme.of(context);
        Color activeColor = this.activeColor ?? themeData.primaryColor;
        Color color = this.color ?? themeData.scaffoldBackgroundColor;

        if (Axis.vertical == config.scrollDirection)
        {
            return new Column(
              key: key,
              mainAxisSize: MainAxisSize.min,
              children: new List<Widget>{
                new Text(
                  data:(config.activeIndex + 1).ToString(),
                  style: new TextStyle(color: activeColor, fontSize: activeFontSize)


                ),
                new Text(
                  "/",
                  style: new TextStyle(color: color, fontSize: fontSize)


                ),
                new Text(
                  data:config.itemCount.ToString(),
                  style: new TextStyle(color: color, fontSize: fontSize)


                )
              }


            );
        }
        else
        {
            return new Row(
              key: key,
              mainAxisSize: MainAxisSize.min,
              children: new List<Widget>{
                new Text(
                  data:(config.activeIndex + 1).ToString(),
                  style: new TextStyle(color: activeColor, fontSize: activeFontSize)


                ),
                new Text(
                  " /"+config.itemCount.ToString(),
                  style: new TextStyle(color: color, fontSize: fontSize)


                )
              }


            );
        }
    }
}

class RectSwiperPaginationBuilder : SwiperPlugin
{
    ///color when current index,if set null , will be Theme.of(context).primaryColor
    public readonly Color activeColor;

    ///,if set null , will be Theme.of(context).scaffoldBackgroundColor
    public readonly Color color;

    ///Size of the rect when activate
    public readonly Size activeSize;

    ///Size of the rect
    public readonly Size size;

    /// Space between rects
    public readonly float space;

    public readonly Key key;

    public RectSwiperPaginationBuilder(

         Color color = null,
        Key key = null,
        Color activeColor = null,
       Size size = null,
       Size activeSize = null,
       float space = 3.0f)
    {
        this.color = color; this.key = key; this.activeColor = activeColor;
        this.size = size ?? new Size(10.0f, 2.0f); this.activeSize = activeSize ?? new Size(10.0f, 2.0f); this.space = space;
    }


    public override Widget build(BuildContext context, SwiperPluginConfig config)
    {
        ThemeData themeData = Theme.of(context);
        Color activeColor = this.activeColor ?? themeData.primaryColor;
        Color color = this.color ?? themeData.scaffoldBackgroundColor;

        List<Widget> list = new List<Widget>();

        if (config.itemCount > 20)
        {
            UnityEngine.Debug.LogWarning(
                "The itemCount is too big, we suggest use FractionPaginationBuilder instead of DotSwiperPaginationBuilder in this sitituation");
        }

        int itemCount = config.itemCount;
        int activeIndex = config.activeIndex;

        for (int i = 0; i < itemCount; ++i)
        {
            bool active = i == activeIndex;
            Size size = active ? this.activeSize : this.size;
            list.Add(new SizedBox(
              width: size.width,
              height: size.height,
              child: new Container(
                color: active ? activeColor : color,
                key: Key.key("pagination_$i"),
                margin: EdgeInsets.all(space)


              )


            ));
        }

        if (config.scrollDirection == Axis.vertical)
        {
            return new Column(
              key: key,
              mainAxisSize: MainAxisSize.min,
              children: list


            );
        }
        else
        {
            return new Row(
              key: key,
              mainAxisSize: MainAxisSize.min,
              children: list


            );
        }
    }
}

class DotSwiperPaginationBuilder : SwiperPlugin
{
    ///color when current index,if set null , will be Theme.of(context).primaryColor
    public readonly Color activeColor;

    ///,if set null , will be Theme.of(context).scaffoldBackgroundColor
    public readonly Color color;

    ///Size of the dot when activate
    public readonly float activeSize;

    ///Size of the dot
    public readonly float size;

    /// Space between dots
    public readonly float space;

    public readonly Key key;

    public DotSwiperPaginationBuilder(

         Color activeColor = null,
       Color color = null,
       Key key = null,
       float size = 10.0f,
       float activeSize = 10.0f,
       float space = 3.0f)
    {
        this.activeColor = activeColor;
        this.color = color;
        this.key = key;
        this.size = size;
        this.activeSize = activeSize;
        this.space = space;
    }


    public override Widget build(BuildContext context, SwiperPluginConfig config)
    {
        if (config.itemCount > 20)
        {
            UnityEngine.Debug.LogWarning(
                "The itemCount is too big, we suggest use FractionPaginationBuilder instead of DotSwiperPaginationBuilder in this sitituation");
        }
        Color activeColor = this.activeColor;
        Color color = this.color;

        if (activeColor == null || color == null)
        {
            ThemeData themeData = Theme.of(context);
            activeColor = this.activeColor ?? themeData.primaryColor;
            color = this.color ?? themeData.scaffoldBackgroundColor;
        }

        if (config.indicatorLayout != PageIndicatorLayout.NONE &&
            config.layout == SwiperLayout.DEFAULT)
        {
            return new PageIndicator(
              count: config.itemCount,
              controller: config.pageController,
              layout: config.indicatorLayout,
              size: size,
              activeColor: activeColor,
              color: color,
              space: space
            );
        }

        List<Widget> list = new List<Widget>();

        int itemCount = config.itemCount;
        int activeIndex = config.activeIndex;

        for (int i = 0; i < itemCount; ++i)
        {
            bool active = i == activeIndex;
            list.Add(new Container(
              key: Key.key("pagination_" + i),
              margin: EdgeInsets.all(space),
              child: new ClipRRect(//TODO:check 换成圆形组件
                borderRadius: BorderRadius.circular(10f),
                child: new Container(
                  color: active ? activeColor : color,
                  width: active ? activeSize : size,
                  height: active ? activeSize : size
                )


              )


            ));
        }

        if (config.scrollDirection == Axis.vertical)
        {
            return new Column(
              key: key,
              mainAxisSize: MainAxisSize.min,
              children: list
            );
        }
        else
        {
            return new Row(
              key: key,
              mainAxisSize: MainAxisSize.min,
              children: list
            );
        }
    }
}

public delegate Widget SwiperPaginationBuilder(BuildContext context, SwiperPluginConfig config);

class SwiperCustomPagination : SwiperPlugin
{
    public readonly SwiperPaginationBuilder builder;

    SwiperCustomPagination(SwiperPaginationBuilder builder)
    {
        D.assert(builder != null);
        this.builder = builder;
    }

    public override Widget build(BuildContext context, SwiperPluginConfig config)
    {
        return builder(context, config);
    }
}

class SwiperPagination : SwiperPlugin
{
    /// dot style pagination
    public static readonly SwiperPlugin dots = new DotSwiperPaginationBuilder();

    /// fraction style pagination
    public static readonly SwiperPlugin fraction = new FractionPaginationBuilder();

    public static readonly SwiperPlugin rect = new RectSwiperPaginationBuilder();

    /// Alignment.bottomCenter by default when scrollDirection== Axis.horizontal
    /// Alignment.centerRight by default when scrollDirection== Axis.vertical
    public readonly Alignment alignment;

    /// Distance between pagination and the container
    public readonly EdgeInsets margin;

    /// Build the widet
    public readonly SwiperPlugin builder;

    public readonly Key key;

    public SwiperPagination(Alignment alignment=null, SwiperPlugin builder=null, Key key = null,  EdgeInsets margin = null)
    {
        this.alignment = alignment ?? Alignment.center;
        this.key = key;
        this.margin = margin ?? EdgeInsets.all(10.0f);
        this.builder = SwiperPagination.dots;

    }

    public override Widget build(BuildContext context, SwiperPluginConfig config)
    {
        Alignment alignment = this.alignment ??
            (config.scrollDirection == Axis.horizontal
                ? Alignment.bottomCenter
                : Alignment.centerRight);
        Widget child = new Container(
          margin: margin,
          child: this.builder.build(context, config)
        );
        if (!config.outer)
        {
            child = new Align(
              key: key,
              alignment: alignment,
              child: child


            );
        }
        return child;
    }
}
