using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using Colors = Unity.UIWidgets.material.Colors;
using UnityEngine;
using com.unity.uiwidgets.Runtime.rendering;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.rendering;
using UIWidgetsGallery.gallery;

class ScrollConfigurationTest : ScrollConfiguration
{

    public ScrollConfigurationTest(
           Key key = null,
           ScrollBehavior behavior = null,
           Widget child = null
       ) : base(key: key, behavior:behavior, child: child)
    {
        
    }
    public override bool updateShouldNotify(InheritedWidget oldWidgetRaw)
    {
        return true;
    }
}
public class ListViewTest : UIWidgetsPanel
{
    protected override Widget createWidget()
    {
        return new MaterialApp(
            home: new  MyApp()
            );
    }
}

public class MyApp : StatefulWidget
{

    public override State createState()
    {
        return new MyState();
    }
}

public class MyState : State<MyApp>
{
    ScrollController _sc = new ScrollController();
    ScrollController _sc1 = new ScrollController();
    ListView ls = null;

    float dr = 1;
    float swdx = 1080, shdx=1920;

    public float Dx2DpWithScaleW(float dx)
    {

        return dx*(swdx/1080) / dr;
    }

    public float Dx2DpWithScaleH(float dx)
    {

        return dx * (shdx / 1920) / dr;
    }
    public override void initState()
    {
        base.initState();
        //_sc.addListener(() => 
        //Debug.Log("aaaaa")
        //);
        //_sc1.addListener(() => 
        //Debug.Log("bbbbb")
        //);

        MediaQueryData me = MediaQueryData.fromWindow(Window.instance);
        dr = me.devicePixelRatio;
        swdx = Window.instance.physicalSize.width;
        swdx = Window.instance.physicalSize.height;
    }


    public override Widget build(BuildContext context)
    {
        return GetItem2();
    }

    public Widget GetItem2()
    {
        return new CustomScrollView(
                shrinkWrap: true,
                controller: _sc1,
                scrollDirection: Axis.vertical,
                 physics: new ClampingScrollPhysics(),
                 slivers: new List<Widget>{
                     GetItem5() ,GetItem6() ,
                     GetItem5(), GetItem6(),
                     GetItem5(), GetItem6(),
                     GetItem5(), GetItem6() }
                 );
    }

    Widget GetItem6()
    {
        return new SliverFixedExtentList(

                           itemExtent: Dx2DpWithScaleH(2000),
                           del: new SliverChildBuilderDelegate(
                                   (BuildContext context, int index) =>
                                   {
                                       return new Container(
                                                alignment: Alignment.center,
                                                color: Colors.lightBlue[100 * (index % 9) == 0 ? 50 : 100 * (index % 9)]
                                        
                                               );
                                   },
                                   childCount: 1
                           )
                           );
    }
    Widget GetItem5()
    {
        return new SliverAppBar(
                    expandedHeight: Dx2DpWithScaleH (800),
                    backgroundColor: Colors.red,
                    flexibleSpace: new FlexibleSpaceBar(
                                   title: new Text("AAAA"),
                                   background: new SingleChildScrollView(
                                       physics: new ClampingScrollPhysics(),
                                       controller: new ScrollController(initialScrollOffset: 2000-1080/2),
                                       scrollDirection: Axis.horizontal,
                                       child: Unity.UIWidgets.widgets.Image.asset(
                                               "lake",
                                               fit: BoxFit.fill,
                                               height: Dx2DpWithScaleH(800),
                                               width: Dx2DpWithScaleW(2000)
                                           )
                                       )
                                   )
                               );
    }
    
    
}
