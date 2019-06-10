using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<int> t1 = new List<int>();
        List<int> t2 = new List<int>();
        List<int> t3 = new List<int>();
        List<int> t4 = new List<int>();
        char[] ilgNum = new char[] { '1', '7', '4' };

        for (int i = 100; i<= 999; i++)
        {
            int b = i / 100; int s = i / 10 % 10; int g = i % 10;
            if ((b == 2 && s != 4 && g != 6) || (b != 2 && s == 4 && g != 6)|| (b != 2 && s != 4 && g == 6))
            {
                t1.Add(i);
                if (CheckOneOfThreeWithDiffPositin(i))
                {
                    t2.Add(i);
                    if (CheckTwoOfThreeWithDiffPositin(i))
                    {
                        t3.Add(i);
                        if (i.ToString().IndexOfAny(ilgNum) == -1)
                        {
                            t4.Add(i);
                            if (CheckOneOfThreeWithDiffPositin2(i))
                            {
                                string str1 = "第一层筛选剩余：";
                                string str2 = "第二层筛选剩余：";
                                string str3 = "第三层筛选剩余：";
                                string str4 = "第四层筛选剩余：";
                                foreach (var item in t1)
                                {
                                    str1 += item + ",";
                                }
                                foreach (var item in t2)
                                {
                                    str2 += item + ",";
                                }
                                foreach (var item in t3)
                                {
                                    str3 += item + ",";
                                }
                                foreach (var item in t4)
                                {
                                    str4 += item + ",";
                                }
                                Debug.Log(str1);
                                Debug.Log(str2);
                                Debug.Log(str3);
                                Debug.Log(str4);
                                Debug.Log("密码是：" + i);
                            }
                        }
                    }
                }
            }
        }
    }


    bool CheckOneOfThreeWithDiffPositin(int num)
    {
        string c2 = "258";
        string temp = num.ToString();
        int b1 = c2.IndexOf(temp[0]);
        int b2 = c2.IndexOf(temp[1]);
        int b3 = c2.IndexOf(temp[2]);

        bool d1 = b1 > 0 && b2 < 0 && b3 < 0 ;
        bool d2 = b2 >= 0&& b2!= 1 && b1 < 0 && b3 < 0;
        bool d3 = b3 >= 0&& b3!=2 && b1 < 0 && b2 < 0;

        return d1 || d2 || d3;
    }

    bool CheckOneOfThreeWithDiffPositin2(int num)
    {
        string c2 = "419";
        string temp = num.ToString();
        int b1 = c2.IndexOf(temp[0]);
        int b2 = c2.IndexOf(temp[1]);
        int b3 = c2.IndexOf(temp[2]);

        bool d1 = b1 > 0 && b2 < 0 && b3 < 0;
        bool d2 = b2 >= 0 && b2 != 1 && b1 < 0 && b3 < 0;
        bool d3 = b3 >= 0 && b3 != 2 && b1 < 0 && b2 < 0;

        return d1 || d2 || d3;
    }
    bool CheckTwoOfThreeWithDiffPositin(int num)
    {
        string c2 = "692";
        string temp = num.ToString();
        int b1 = c2.IndexOf(temp[0]);
        int b2 = c2.IndexOf(temp[1]);
        int b3 = c2.IndexOf(temp[2]);

        bool d1 = b1 > 0 && b2 >= 0&&b2!=1 && b3 < 0;
        bool d2 = b2 >= 0 && b2 != 1 && b1 < 0 && b3 >= 0&& b3!=2;
        bool d3 = b3 >= 0 && b3 != 2 && b1 > 0 && b2 < 0;

        return d1 || d2 || d3;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
