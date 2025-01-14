using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 执行淡入效果，使精灵渲染器的颜色变为完全不透明。
    /// </summary>
    public void FadeIn()
    {
        // 定义目标颜色为完全不透明的白色
        Color targetColor = new Color(1, 1, 1, 1);
        // 使用 DOTween 插件，使精灵渲染器的颜色在设定的持续时间内过渡到目标颜色
        spriteRenderer.DOColor(targetColor, Setting.fadeDuration);
    }
    
    /// <summary>
    /// 执行淡出效果，使精灵渲染器的颜色变为部分透明。
    /// </summary>
    public void FadeOut()
    {
        // 定义目标颜色为具有一定透明度的白色，透明度由设置中的目标阿尔法值决定
        Color targetColor = new Color(1, 1, 1, Setting.targetAlpha);
        // 使用 DOTween 插件，使精灵渲染器的颜色在设定的持续时间内过渡到目标颜色
        spriteRenderer.DOColor(targetColor, Setting.fadeDuration);
    }
}
