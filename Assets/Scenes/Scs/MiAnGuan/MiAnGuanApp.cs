/*****************************************
   @author   : shixuesi
   @date     : 2019/06/10
******************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace MiAnGuanUI
{
    /// <summary>
    /// 入口类
    /// </summary>
    public class MiAnGuanApp : UIWidgetsPanel
    {
        protected override void OnEnable()
        {
            FontManager.instance.addFont(Resources.Load<Font>("MaterialIcons-Regular"), "Material Icons");
            FontManager.instance.addFont(Resources.Load<Font>("GalleryIcons"), "GalleryIcons");
            base.OnEnable();
        }
        protected override Widget createWidget()
        {
            return new MiAnGuanMain();
        }
    }

    /// <summary>
    /// 主界类 (包括底部三个按钮（冒险旅行，排行榜，个人中心），和对应的界面)
    /// </summary>
    public class MiAnGuanMain : StatefulWidget
    {
        public override State createState()
        {
            return new MiAnGuanMainState();
        }
    }

    
    public class MiAnGuanMainState : State<MiAnGuanMain>
    {
        List<BottomNavigationBarItem> items = new List<BottomNavigationBarItem> {
            new BottomNavigationBarItem(activeIcon: new Icon(Icons.cloud),icon: new Icon(Icons.cloud_queue),title:new Text("冒险旅行")),
            new BottomNavigationBarItem(activeIcon: new Icon(Icons.favorite),icon: new Icon(Icons.favorite_border),title:new Text("排行榜单")),
            new BottomNavigationBarItem(activeIcon: new Icon(Icons.favorite),icon: new Icon(Icons.favorite_border),title:new Text("个人档案"))
        };

        int currindex;


        void OnItemTap(int index)
        {
            setState(()=> { currindex = index; });
        }
        public override void initState()
        {
            base.initState();

        }
        public override Widget build(BuildContext context)
        {
            return new MaterialApp(
                home: new Scaffold(
                body: new Container(color:Colors.yellow),
                bottomNavigationBar:new Container(
                    height:84,
                    width:375,
                    decoration:new BoxDecoration(
                        color:Colors.white,
                        boxShadow:new List<BoxShadow> {
                            new BoxShadow(color:new Color(0x1A000000),blurRadius:1,offset:Offset.zero)
                        }
                        ),
                    child: new BottomNavigationBar(
                        items:items,
                        currentIndex: currindex,
                        onTap: OnItemTap
                        )
                    )
                )
                
                );
            
        }
    }
}

