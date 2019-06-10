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

namespace Unity.UIWidgets.widgets
{
    public class PaddingTest : UIWidgetsPanel
    {
        protected override void OnEnable()
        {
            // if you want to use your own font or font icons.   
            // FontManager.instance.addFont(Resources.Load<Font>(path: "path to your font"), "font family name");

            // load custom font with weight & style. The font weight & style corresponds to fontWeight, fontStyle of 
            // a TextStyle object
            // FontManager.instance.addFont(Resources.Load<Font>(path: "path to your font"), "Roboto", FontWeight.w500, 
            //    FontStyle.italic);

            // add material icons, familyName must be "Material Icons"
            // FontManager.instance.addFont(Resources.Load<Font>(path: "path to material icons"), "Material Icons");

            base.OnEnable();
        }

        protected override Widget createWidget()
        {
            //return new WidgetsApp(
            //    home: new ExampleApp(),
            //    pageRouteBuilder: (RouteSettings settings, WidgetBuilder builder) =>
            //        new PageRouteBuilder(
            //            settings: settings,
            //            pageBuilder: (BuildContext context, Animation<float> animation,
            //                Animation<float> secondaryAnimation) => builder(context)
            //        )
            //);
            return new ExampleApp();
        }

        class TestScrollPositionWithSingleContext : ScrollPositionWithSingleContext
        {
            public TestScrollPositionWithSingleContext(
               ScrollPhysics physics = null,
               ScrollContext context = null,
               float? initialPixels = 0.0f,
               bool keepScrollOffset = true,
               ScrollPosition oldPosition = null,
               string debugLabel = null
           ) :base(physics: physics,
            context: context,
            keepScrollOffset: keepScrollOffset,
            oldPosition: oldPosition,
            debugLabel: debugLabel)
            {
                
            }
            public bool isStop;
            public override void applyUserOffset(float delta)
            {
                if (isStop)
                    return;
                base.applyUserOffset(delta);
            }

            public override void applyUserScrollOffset(float delta)
            {
                if (isStop)
                    return;
                base.applyUserScrollOffset(delta);
            }

            public override float setPixels(float newPixels)
            {
                if (isStop)
                    return 0;
                return base.setPixels(newPixels);
            }
        }

        class TestScrollController : ScrollController
        {

            public override ScrollPosition createScrollPosition(
                ScrollPhysics physics,
                ScrollContext context,
                ScrollPosition oldPosition
            )
            {
                return new TestScrollPositionWithSingleContext(
                    physics: physics,
                    context: context,
                    initialPixels: this.initialScrollOffset,
                    keepScrollOffset: this.keepScrollOffset,
                    oldPosition: oldPosition,
                    debugLabel: this.debugLabel
                );
            }
        }
        class ExampleApp : StatefulWidget
        {
            public ExampleApp(Key key = null) : base(key)
            {
            }

            public override State createState()
            {
                return new ExampleState();
            }
        }

        class ExampleState : State<ExampleApp>
        {

            public override void initState()
            {

                base.initState();
                _scrollController.addListener(OnScroll);
            }
            void OnTaped()
            {
                Debug.Log("OnTaped");
                TestScrollPositionWithSingleContext context = (_scrollController.position as TestScrollPositionWithSingleContext);
                context.isStop = !context.isStop;
                setState();
            }

            void OnScroll()
            {
                Debug.Log("OnScroll");
            }

            TestScrollController _scrollController = new TestScrollController();
            public override Widget build(BuildContext context)
            {
                return new Container(
                    child: new Container(
                             margin: EdgeInsets.all(0),
                             color: Colors.red,

                             child: new Column(

                                 children: new List<Widget> {
                                 new Container(
                                     //margin:EdgeInsets.only(bottom:200),
                                     constraints:new BoxConstraints(maxHeight:500),
                                     color:Colors.blue,
                                     child:

                              ListView.builder(
                                 //shrinkWrap : true,
                                 //margin:EdgeInsets.only(bottom:200),
                                 //padding:EdgeInsets.symmetric(200,200),
                                 itemCount : 4,
                                 itemExtent:100,
                                 //primary: false,
                                 physics:new AlwaysScrollableScrollPhysics(),
                                 controller:_scrollController,
                                 itemBuilder:getItemView
                                 )
                                 ),
                             new GestureDetector(
                                 onTap:OnTaped,
                                 child:new Container(
                                     padding:EdgeInsets.all(20),
                                     child:new Text("ddddddddddddddddddd"),
                                     color:Colors.green[600]
                                     //margin:EdgeInsets.only(top:300)
                                     )
                                 )
                                     }
                                 )

                        )
                        );
            }

            Widget getItemView(BuildContext context, int index)
            {
                return new Container(alignment: Alignment.center, color: Colors.lightBlue[100 * (index % 9) == 0 ? 50 : 100 * (index % 9)], child: new Text("list item" + index.ToString()));
            }

            Widget getItemView1(BuildContext context, int index)
            {
                return new Container(alignment: Alignment.center, color: Colors.teal[100 * (index % 9) == 0 ? 50 : 100 * (index % 9)], child: new Text("grid item" + index.ToString()));
            }
        }

    }
}


