using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GlobalData
{
    private GlobalData()
    {
        pearls = 100;
        maxOxygen = 100;
        pipeLevel = 1;
    }
    // 使用单例模式，设置一个全局单例对象
    private static GlobalData _instance;
    public static GlobalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalData();
            }
            return _instance;
        }
    }

    public int pearls { get; set; }
    public int maxOxygen { get; set; }
    public int pipeLevel { get; set; }
}

