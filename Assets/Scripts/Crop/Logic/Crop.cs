using UnityEngine;

// ReSharper disable once CheckNamespace
namespace MFarm.CropPlant
{
    public class Crop : MonoBehaviour
    {
       public CropDetails cropDetails;
       private TileDetails tileDetails;
       private int harvestActioCount;

       public void ProcessToolAction(ItemDetails tool,TileDetails tile)
       {
           tileDetails = tile;
           int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
           if (requireActionCount==-1)
           {
               return;
           }

           if (harvestActioCount<requireActionCount)
           {
               harvestActioCount++;
           }

           if (harvestActioCount>=requireActionCount)
           {
               if (cropDetails.generateAtPlayerPosition)
               {
                   Debug.Log(111);
                   SpawnHarvestItems();
               }
           }
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
