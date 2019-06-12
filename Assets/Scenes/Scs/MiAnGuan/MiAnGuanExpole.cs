using BestFlutter.Page_Indicator;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Stack = Unity.UIWidgets.widgets.Stack;

namespace MiAnGuanUI
{
    public class MiAnGuanExplore : StatefulWidget
    {
        public override State createState()
        {
            return new MiAnGuanState();
        }
    }


    public class MiAnGuanState : State<MiAnGuanExplore>
    {
        private Widget topSp = null;
        public override void initState()
        {
            var ls = new List<string> { "http://k.zol-img.com.cn/sjbbs/7692/a7691515_s.jpg",
                "http://k.zol-img.com.cn/sjbbs/7692/a7691515_s.jpg",
            "http://k.zol-img.com.cn/sjbbs/7692/a7691515_s.jpg" };
            topSp = BulidTopSwiper(ls);
            base.initState();
        }
        public Widget BulidTopSwiper(List<string> urls)
        {
            List<Widget> children = new List<Widget>();
            foreach (var item in urls)
            {
                Widget it = Image.network(item,fit:BoxFit.fill);
                children.Add(it);
            }
            TransformerPageController controller = new TransformerPageController(initialPage: 0,itemCount:urls.Count);
            return new Container(
                        height:270,
                        width:375,
                        child:new Stack(

                                children: new List<Widget>{
                                     TransformerPageView.children(index:0,
                                        children:children,pageController:controller
                                        ),
                                    new Align(
                                        alignment:Alignment.bottomCenter,
                                        child:new Padding(
                                            padding: EdgeInsets.only(bottom:20),
                                            child:new PageIndicator(layout: PageIndicatorLayout.WARM, size: 7, activeSize: 7, controller: controller, space: 5.0f, count: urls.Count
                                            ,color:new Unity.UIWidgets.ui.Color(0xFFEAEAEA),activeColor:new Unity.UIWidgets.ui.Color(0xFF404759))

                                            )
                                        )

                                }
                                )
                                ) ;
        }
        public override Widget build(BuildContext context)
        {
            return topSp;
        }
    }
}
