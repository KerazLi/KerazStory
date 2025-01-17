using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


namespace KFarm.Inventory
{
    // 物品管理器，负责在场景中实例化和管理物品
    public class ItemManager : MonoBehaviour
    {
        // 物品的预设，用于实例化新的物品
        public Item itemPrefab;
        // 所有物品的父对象，用于组织物品在场景中的层级关系
        private Transform itemParent;
        //记录场景的Item
        private Dictionary<string, List<SceneItem>> sceneItemDict = new();

        // 当组件启用时，注册事件处理函数
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }

        // 场景加载完成后，获取物品父对象的Transform
        private void OnAfterSceneLoadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreatAllItems();
        }

        // 当组件禁用时，注销事件处理函数
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        }

        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
        }

        // 当需要在场景中实例化物品时调用此函数
        // ID: 物品的唯一标识符
        // pos: 物品在场景中的位置
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }

        /// <summary>
        /// 获取当前场景中的所有物品并存储到字典中
        /// </summary>
        private void GetAllSceneItems()
        {
            // 创建一个列表，用于存储当前场景中的所有物品
            List<SceneItem> currentSceneItems = new();
            // 遍历场景中所有的Item对象，并创建SceneItem对象来保存每个物品的信息
            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new()
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };
                currentSceneItems.Add(sceneItem);
            }
        
            // 检查字典中是否已存在当前场景的物品信息
            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                // 如果存在，则更新该场景的物品信息
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else
            {
                // 如果不存在，则将当前场景的物品信息添加到字典中
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }
        
        /// <summary>
        /// 重新创建场景中的所有物品
        /// </summary>
        private void RecreatAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //清场
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
    }
}
