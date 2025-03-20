using UnityEngine;

// ReSharper disable once CheckNamespace
namespace MFarm.CropPlant
{
    public class Crop : MonoBehaviour
    {
       public CropDetails cropDetails;
       private int harvestActioCount;

       public void ProcessToolAction(ItemDetails tool)
       {
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
       }
    }
}
