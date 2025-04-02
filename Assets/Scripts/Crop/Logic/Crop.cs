using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace MFarm.CropPlant
{
    public class Crop : MonoBehaviour
    { 
        private static readonly int RotateRight = Animator.StringToHash("RotateRight");
        private static readonly int RotateLeft = Animator.StringToHash("RotateLeft");
        private static readonly int FallingRight = Animator.StringToHash("FallingRight");
        private static readonly int FallingLeft = Animator.StringToHash("FallingLeft");
        public CropDetails cropDetails;
       public TileDetails tileDetails;
       private int harvestActioCount;
       private Animator animator;
       private int harvestActionCount;
       public bool CanHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;
       private Transform PlayerTransform => FindObjectOfType<PlayerController>().transform;

       // ReSharper disable Unity.PerformanceAnalysis
       public void ProcessToolAction(ItemDetails tool,TileDetails tile)
       {
           tileDetails = tile;
           animator = GetComponentInChildren<Animator>();
           int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
           if (requireActionCount==-1)
           {
               return;
           }

           if (harvestActioCount<requireActionCount)
           {
               harvestActioCount++;
               if (animator!=null && cropDetails.hasAnimation)
               {
                   if (PlayerTransform.position.x<transform.position.x)
                   {
                       animator.SetTrigger(RotateRight);
                   }
                   else
                   {
                       animator.SetTrigger(RotateLeft);
                   }
               }

               if (cropDetails.hasParticalEffect)
               {
                   //播放粒子特效
                   EventHandler.CallParticleEffectEvent(cropDetails.particaleEffectType,transform.position+cropDetails.effectPos); 
               }
               
           }

           if (harvestActioCount>=requireActionCount)
           {
               if (cropDetails.generateAtPlayerPosition|| !cropDetails.hasAnimation)
               {
                   //Debug.Log(111);
                   SpawnHarvestItems();
               }
               else if (cropDetails.hasAnimation)
               {
                   if (PlayerTransform.position.x<transform.position.x)
                   {
                       animator.SetTrigger(FallingRight);
                   }
                   else
                   {
                       animator.SetTrigger(FallingLeft);
                   }

                   StartCoroutine(HarvestAfterAnimation());
               }
           }
       }

       private IEnumerator HarvestAfterAnimation()
       {
           while (!animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
           {
               yield return null;
           }
           SpawnHarvestItems();
           //转换新物体
           if (cropDetails.transferItemID>0)
           {
               CreateTransferCrop();
           }
       }

       private void CreateTransferCrop()
       {
           tileDetails.seedItemID=cropDetails.transferItemID;
           tileDetails.daysSinceLastHarvest = -1;
           tileDetails.growthDays = 0;
           EventHandler.CallRefreshCurrentMap();
       }

       public void SpawnHarvestItems()
       {
           for (int i = 0; i < cropDetails.producedItemID.Length; i++)
           {
               int amountToProduce;
               if (cropDetails.producedMinAmount==cropDetails.producedMaxAmount)
               {
                   amountToProduce = cropDetails.producedMinAmount[i];
               }
               else
               {
                   amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i]+1);
               }

               for (int j = 0; j < amountToProduce; j++)
               {
                   if (cropDetails.generateAtPlayerPosition)
                   {
                       EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);
                   }
                   else//地图上生成物品
                   {
                       var dirX=transform.position.x>PlayerTransform.position.x?1:-1;
                       var spawnPos =
                           new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
                               transform.position.y +
                               Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y));
                       EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                   }
               }
           }

           if (tileDetails !=null)
           {
               tileDetails.daysSinceLastHarvest++;
               if (cropDetails.daysToRegrow>0 && tileDetails.daysSinceLastHarvest<cropDetails.regrowTimes)
               {
                   tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                   EventHandler.CallRefreshCurrentMap();
               }
               else
               {
                   tileDetails.daysSinceLastHarvest = -1;
                   tileDetails.seedItemID = -1;
               }

               Debug.Log(6666);
               Destroy(gameObject);
               
           }
       }
    }
}
