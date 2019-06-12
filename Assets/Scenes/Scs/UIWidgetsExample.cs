using BestFlutter.Page_Indicator;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using FontStyle = Unity.UIWidgets.ui.FontStyle;

namespace UIWidgetsSample
{
    public class UIWidgetsExample : UIWidgetsPanel
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            FontManager.instance.addFont(Resources.Load<Font>(path: "MaterialIcons-Regular"), "Material Icons");
        }

        protected override Widget createWidget()
        {
            return new MyAppp(
            );
        }
    }


    class MyAppp : StatelessWidget
    {

        public override Widget build(BuildContext context)
        {
            return new MaterialApp(
              title: "Flutter Demo",
              theme: new ThemeData(
                primarySwatch: Colors.blue


              ),
              home: new MyHomePage(title: "Flutter Demo Home Page")


            );
        }
    }



    class MyHomePage : StatefulWidget
    {
        public MyHomePage(string title, Key key = null) : base(key: key)
        {
            this.title = title;
        }

        public readonly string title;

        public override State createState() => new _MyHomePageState();
    }
    class RadioGroup : StatefulWidget
    {
        public readonly List<string> titles;

        public readonly ValueChanged<int> onIndexChanged;

        public RadioGroup(List<string> titles, ValueChanged<int> onIndexChanged = null, Key key = null) : base(key: key)
        {
            this.titles = titles;
            this.onIndexChanged = onIndexChanged;

        }
        public override State createState()
        {
            return new _RadioGroupState();
        }
    }


    class _RadioGroupState : State<RadioGroup>
    {
        int _index = 1;

        Row getItem(string title, int index)
        {
            return new Row(
                      mainAxisSize: MainAxisSize.min,
                      children: new List<Widget> {
                        new Radio<string>(
                            value: index.ToString(),
                            groupValue: _index.ToString(),
                            onChanged: ( inde)=> {
                  setState(()=> {
                    _index = int.Parse(inde);
                    widget.onIndexChanged(_index);
                });
        }),
            new Text(title)
          }
        );
        }
        public override Widget build(BuildContext context)
        {
            List<Widget> list = new List<Widget>();
            for (int i = 0; i < widget.titles.Count; ++i)
            {
                int ind = i;
                list.Add(getItem(widget.titles[ind], ind));
            }

            return new Wrap(
              children: list
            );
        }
    }
    class _MyHomePageState : State<MyHomePage>
    {
        int _index = 1;

        float size = 20.0f;
        float activeSize = 30.0f;

        TransformerPageController controller;

        PageIndicatorLayout layout = PageIndicatorLayout.SLIDE;

        List<PageIndicatorLayout> layouts = new List<PageIndicatorLayout> { PageIndicatorLayout.COLOR, PageIndicatorLayout.DROP,
            PageIndicatorLayout.NONE,PageIndicatorLayout.SCALE,PageIndicatorLayout.SLIDE,PageIndicatorLayout.WARM };

        bool loop = true;


        public override void initState()
        {
            controller = new TransformerPageController(itemCount: 4, loop: false,reverse:false);
            base.initState();
        }
        public override void didUpdateWidget(StatefulWidget oldWidget)
        {
            base.didUpdateWidget(oldWidget);
        }

        Widget getItem()
        {
            return new PageIndicator(layout: layout, size: 14, activeSize: activeSize, controller: controller, space: 5.0f, count: 4);
        }
        public override Widget build(BuildContext context)
        {

            var children = new List<Widget>{
      new Container(
          width:1080,
          height:800,
        color: Colors.red
      ),
      new Container(
          width:1080,
          height:800,
        color: Colors.green
      ),
      new Container(
          width:1080,
          height:800,
        color: Colors.blueAccent
      ),
      new Container(
          width:1080,
          height:800,
        color: Colors.grey
      )
    };
            return new Scaffold(
              appBar: new AppBar(
                title: new Text(widget.title)
              ),
                body: new Column(
                    children: new List<Widget> {
                        new Row(
                            children:new List<Widget>{
                                new Checkbox(
                                    value:loop,
                                    onChanged:(va)=>{
                                        setState(()=>{
                                            if(va==true)
                                            {
                                                //controller = new TransformerPageController(itemCount:4,loop:true);
                                            }else
                                            {
                                                //controller = new PageController(initialPage:0);
                                            }
                                            loop = (bool)va;
                                        });
                                    }
                                    ),
                                new Text("loop")
                            }
                            ),
                        new RadioGroup(
                            titles:new List<string>{ "COLOR", "DROP",
            "NONE","SCALE","SLIDE","WARM" },
                            onIndexChanged:(index)=>{ setState(()=>{ _index = index;layout=layouts[index]; }); }

                            ),
                        new Container(
                            width:1080,
                            height:800,
                            child:
                        new Stack(
                                children:new List<Widget>{
                                     TransformerPageView.children(index:0,
                                        children:children,pageController:controller
                                        ),
                                    new Align(
                                        alignment:Alignment.bottomCenter,
                                        child:new Padding(
                                            padding: EdgeInsets.only(bottom:20),
                                            child:getItem()
                                                
                                            )
                                        )

                                }
                                )
                            )
                    }
                    )

          );
        }
    }
}
