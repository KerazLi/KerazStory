# KerazStory项目问题记录

#### 1.瓦片地图的缝隙问题

> 将Grid该为0.99解决问题

#### 2.切换场景后加载场景物体，保存原先场景数据再次加载不出现场景物品数据的问题

>ItemManager中的预制体丢失了，重新赋值就可以了
#### 3.46节生成地图数据中获取地图瓦片信息报空指针的问题，无法获取到地图瓦片信息
>事件通知方法写错了，EventHandler.AfterSceneLoadEvent -= OnAfterSceneUnloadEvent;写成了EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
> 所以一直报空指针的错误
