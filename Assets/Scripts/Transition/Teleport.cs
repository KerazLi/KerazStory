using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* @Program:Teleport.cs
* @Author: Keraz
* @Description:切换场景人物切换不同场景不同位置
* @Date: 2025年02月19日 星期三 19:48:08
*/

namespace MFarm.Transition
{
    public class Teleport : MonoBehaviour
    {
        /// <summary>
        /// 目标场景名称，玩家将要被瞬移到的场景。
        /// </summary>
        public string sceneToGo;
        
        /// <summary>
        /// 目标位置，玩家在新场景中的瞬移目的地点。
        /// </summary>
        public Vector3 positionToGo;
    
        /// <summary>
        /// 当玩家进入瞬移点的触发器区域时，该方法会被调用。
        /// 它会检查碰撞对象是否为玩家，并触发场景切换或位置变化事件。
        /// </summary>
        /// <param name="other">发生碰撞的另一个对象的Collider2D组件。</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            // 检查碰撞对象是否标记为"Player"，以确定是否需要执行瞬移操作
            if (other.CompareTag("Player"))
            {
                // 调用事件处理函数，传递目标场景名称和瞬移位置，以执行场景切换或位置变化
                EventHandler.CallTransitionEvent(sceneToGo, positionToGo);
            }
        }
    }
}
