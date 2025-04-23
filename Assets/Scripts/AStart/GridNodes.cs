using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.AStart
{
    public class GridNodes : MonoBehaviour
    {
        private int width;
        private int height;
        private Node[,] gridNodes;
        /// <summary>
        /// 构造函数初始化节点范围数组
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        public GridNodes(int width, int height)
        {
            this.width = width;
            this.height = height;
            gridNodes = new Node[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    gridNodes[i, j] = new Node(new Vector2Int(i, j));
                }
            }
        }

        public Node GetGridNode(int xPos,int yPos)
        {
            if (xPos<width&&yPos<height&&xPos>=0&&yPos>=0)
            {
                return gridNodes[xPos, yPos];
            }

            Debug.Log("超出网格范围");
            return null;
        }
    }
}
