using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* @Program:TriggerItemFader.cs
* @Author: Keraz
* @Description:类用于处理当物体进入或退出触发器区域时，对其子物体中的 ItemFader 组件进行淡入淡出操作。
* @Date: 2025年02月19日 星期三 19:39:52
*/

public class TriggerItemFader : MonoBehaviour
{
    /// <summary>
    /// 当其他物体进入此触发器区域时，获取其所有子物体中的 ItemFader 组件，并调用它们的 FadeOut 方法进行淡出操作。
    /// </summary>
    /// <param name="other">进入触发器区域的物体的 Collider2D 组件。</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 获取进入触发器区域的物体及其子物体中的所有 ItemFader 组件
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();
        // 遍历每个 ItemFader 组件并调用其 FadeOut 方法进行淡出操作
        foreach (ItemFader fader in faders)
        {
            fader.FadeOut();
        }
    }

    /// <summary>
    /// 当其他物体退出此触发器区域时，获取其所有子物体中的 ItemFader 组件，并调用它们的 FadeIn 方法进行淡入操作。
    /// </summary>
    /// <param name="other">退出触发器区域的物体的 Collider2D 组件。</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // 获取退出触发器区域的物体及其子物体中的所有 ItemFader 组件
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();
        // 遍历每个 ItemFader 组件并调用其 FadeIn 方法进行淡入操作
        foreach (ItemFader fader in faders)
        {
            fader.FadeIn();
        }
    }
}

