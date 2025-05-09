using System.Collections;
using System.Collections.Generic;
using MFram.CropPlant;
using UnityEngine;

namespace MFrarm.CropPlant
{
    public class ReapItem : MonoBehaviour
    {
        private CropDetails cropDetails;
        private Transform PlayerTransform => FindObjectOfType<PlayerController>().transform;

        public void InitCropData(int ID)
        {
            cropDetails = CropManager.Instance.GetCropDetails(ID);
        }

        public void SpawnHarvestItems()
        {
            for (int i = 0; i < cropDetails.producedItemID.Length; i++)
            {
                int amountToProduce;
                if (cropDetails.producedMinAmount == cropDetails.producedMaxAmount)
                {
                    amountToProduce = cropDetails.producedMinAmount[i];
                }
                else
                {
                    amountToProduce = Random.Range(cropDetails.producedMinAmount[i],
                        cropDetails.producedMaxAmount[i] + 1);
                }

                for (int j = 0; j < amountToProduce; j++)
                {
                    if (cropDetails.generateAtPlayerPosition)
                    {
                        EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);
                    }
                    else //地图上生成物品
                    {
                        var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
                        var spawnPos =
                            new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
                                transform.position.y +
                                Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y));
                        EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                    }
                }
            }
        }
    }
}
