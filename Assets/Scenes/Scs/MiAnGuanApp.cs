/*****************************************
   @author   : shixuesi
   @date     : 2019/06/10
******************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace MiAnGuanUI
{
    /// <summary>
    /// 入口类
    /// </summary>
    public class MiAnGuanApp : UIWidgetsPanel
    {
        protected override Widget createWidget()
        {
            return new MiAnGuanMain();
        }
    }

    /// <summary>
    /// 主界类 (包括底部三个按钮（冒险旅行，排行榜，个人中心），和对应的)
    /// </summary>
    public class MiAnGuanMain : StatefulWidget
    {
        public override State createState()
        {
            throw new System.NotImplementedException();
        }
    }
}

