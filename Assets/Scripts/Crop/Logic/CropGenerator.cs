using System;
using System.Collections;
using System.Collections.Generic;
using MFarm.Map;
using UnityEngine;

namespace MFrarm.CropPlant
{


    public class CropGenerator : MonoBehaviour
    {
        private Grid currentGrid;
        public int seedItemID;
        public int growthDays;

        private void Awake()
        {
            currentGrid = FindObjectOfType<Grid>();
        }

        private void OnEnable()
        {
            EventHandler.GenerateCropEvent += GenerateGrop;
        }

        private void OnDisable()
        {
            EventHandler.GenerateCropEvent -= GenerateGrop;
        }


        private void GenerateGrop()
        {
            Vector3Int cropGridPos = currentGrid.WorldToCell(transform.position);
            Debug.Log(cropGridPos.x+" "+cropGridPos.y);
            if (seedItemID != 0)
            {
                TileDetails tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(cropGridPos);
                if (tile == null)
                {
                    tile = new TileDetails();
                }

                tile.daysSinceWatered = -1;
                tile.seedItemID = seedItemID;
                tile.growthDays = growthDays;
                GridMapManager.Instance.UpdateTileDetails(tile);
            }
        }

    }
}
