using BestFlutter.Page_Indicator;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
//using UnityEngine;

/// plugin to display swiper components
///
public abstract class SwiperPlugin
{
    //SwiperPlugin();

    public abstract Widget build(BuildContext context, SwiperPluginConfig config);
}

public class SwiperPluginConfig
{
    public readonly int activeIndex;
    public readonly int itemCount;
    public readonly PageIndicatorLayout indicatorLayout;
    public readonly Axis scrollDirection;
    public readonly bool loop;
    public readonly bool outer;
    public readonly PageController pageController;
    public readonly SwiperController controller;
    public readonly SwiperLayout layout;

    public SwiperPluginConfig(
      int activeIndex,
      int itemCount,
      PageIndicatorLayout indicatorLayout,
      bool outer,
      Axis scrollDirection,
      SwiperController controller,
      PageController pageController,
      SwiperLayout layout,
      bool loop)
    {
        D.assert(scrollDirection != null);
        D.assert(controller != null);
        this.activeIndex = activeIndex;
        this.itemCount = itemCount;
        this.indicatorLayout = indicatorLayout;
        this.outer = outer;
        this.scrollDirection = scrollDirection;
        this.controller = controller;
        this.pageController = pageController;
        this.layout = layout;
        this.loop = loop;
    }
}

class SwiperPluginView : StatelessWidget
{
    public readonly SwiperPlugin plugin;
    public readonly SwiperPluginConfig config;

    SwiperPluginView(SwiperPlugin plugin, SwiperPluginConfig config)
    {
        this.plugin = plugin;
        this.config = config;
    }


    public override Widget build(BuildContext context)
    {
        return plugin.build(context, config);
    }
}
