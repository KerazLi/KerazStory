using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

/*
* @Program:SwitchBounds.cs
* @Author: Keraz
* @Description:不同场景转换不同的摄像机的边界
* @Date: 2025年02月19日 星期三 20:02:18
*/

public class SwitchBounds : MonoBehaviour
{
    // 获取当前对象上的CinemachineConfiner组件
    public CinemachineConfiner confiner;
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SwitchConfinerShape;
    }

    /*private void Start()
    {
        SwitchConfinerShape();
    }*/

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SwitchConfinerShape;
    }

    private void SwitchConfinerShape()
    {
        // 获取标记为"BoundsConfiner"的游戏对象上的PolygonCollider2D组件，用作边界限制形状
        PolygonCollider2D confinerShape =
            GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();
        Debug.Log(confinerShape.GameObject().name);
        
        // 设置CinemachineConfiner的边界形状为之前获取的PolygonCollider2D
        confiner.m_BoundingShape2D = confinerShape;

        // 使路径缓存无效，以触发重新计算边界限制路径
        confiner.InvalidatePathCache();
    }
}
