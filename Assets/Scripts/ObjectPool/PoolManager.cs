using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefabs;
    private List<ObjectPool<GameObject>> poolEffectList=new ();

    private void Start()
    {
        CreatePool();
    }

    private void OnEnable()
    {
        EventHandler.ParticleEffectEvent += OnParticleEffectEvent;
    }

    private void OnDisable()
    {
        EventHandler.ParticleEffectEvent -= OnParticleEffectEvent;
    }

    private void OnParticleEffectEvent(ParticaleEffectType effectType, Vector3 pos)
    {
        //WORLFLOW:根据特效补全
        var objPool = effectType switch
        {
            ParticaleEffectType.LeavesFalling01 => poolEffectList[0],
            ParticaleEffectType.LeavesFalling02 => poolEffectList[1],
            ParticaleEffectType.Rock => poolEffectList[2],
            ParticaleEffectType.ReapableScenery => poolEffectList[3],
            _ => null
        };
       
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        StartCoroutine(ReleaseRountine(objPool, obj));

    }

    private IEnumerator ReleaseRountine(ObjectPool<GameObject> pool,GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);
    }

    private void CreatePool()
    {
        foreach (GameObject item in poolPrefabs)
        {
            Transform parent = new GameObject(item.name).transform;
            parent.SetParent(transform);
            var newPool = new ObjectPool<GameObject>(
                ()=>Instantiate(item,parent),
                e => { e.SetActive(true);},
                e=>{ e.SetActive(false);},
                e => { Destroy(e);}
                );
            poolEffectList.Add(newPool);
        }
        
    }
}
