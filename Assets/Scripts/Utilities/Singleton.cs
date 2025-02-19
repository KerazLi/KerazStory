using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* @Program:Singleton.cs
* @Author: Keraz
* @Description:单例类
* @Date: 2025年02月19日 星期三 20:02:07
*/

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // 存储单例实例的静态字段
    private static T instance;

    /// <summary>
    /// 获取单例实例的属性
    /// </summary>
    /// <value>返回单例实例</value>
    public static T Instance
    {
        get => instance;
    }

    /// <summary>
    /// 在游戏对象被初始化时调用，确保单例实例的正确创建
    /// </summary>
    protected virtual void Awake()
    {
        // 如果当前实例为空，则将当前对象转换为单例实例
        if (instance == null)
        {
            instance = (T)this;
        }
        else
        {
            // 如果实例已存在，则销毁当前游戏对象，以保证单例的唯一性
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 在游戏对象被销毁时调用，用于清理单例实例
    /// </summary>
    protected virtual void OnDestroy()
    {
        // 如果当前实例与被销毁的对象相同，则将实例设置为null
        if (instance == this)
        {
            instance = null;
        }
    }
}
