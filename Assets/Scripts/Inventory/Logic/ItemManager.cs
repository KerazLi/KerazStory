using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace KFarm.Inventory
{
    // 物品管理器，负责在场景中实例化和管理物品
    public class ItemManager : MonoBehaviour
    {
        // 物品的预设，用于实例化新的物品
        public Item itemPrefab;
        // 所有物品的父对象，用于组织物品在场景中的层级关系
        private Transform itemParent;

        // 当组件启用时，注册事件处理函数
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }

        // 场景加载完成后，获取物品父对象的Transform
        private void OnAfterSceneLoadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }

        // 当组件禁用时，注销事件处理函数
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        }

        // 当需要在场景中实例化物品时调用此函数
        // ID: 物品的唯一标识符
        // pos: 物品在场景中的位置
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }
    }
}
