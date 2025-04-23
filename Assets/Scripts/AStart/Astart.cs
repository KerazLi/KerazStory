using System.Collections;
using System.Collections.Generic;
using MFarm.AStart;
using MFarm.Map;
using UnityEngine;

public class Astart : MonoBehaviour
{
    private GridNodes gridNodes;
    private Node startNode;
    private Node targetNode;
    private int gridWidth;
    private int gridHeight;
    private int originX;
    private int originY;
    private List<Node> openNodeList;
    private HashSet<Node> closeNodeList;
    private bool pathFound;
    private bool GenerateGridNode;
    private List<Node> path;

    public void BuildPath(string sceneName,Vector2Int startPos,Vector2Int endPos)
    {
        pathFound = false;
        if (GenerateGridNodes(sceneName,startPos,endPos))
        {
            //查找最短路径
            if (FindShortestPath())
            {
                //构建NPC移动路径
            }
        }
    }
    

    /// <summary>
    /// 构建网格节点信息，初始化两个列表
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="startPos">起点</param>
    /// <param name="endPos">终点</param>
    /// <returns></returns>
    private bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
    {
        if (GridMapManager.Instance.GetGridDimensions(sceneName, out Vector2Int gridDimensions,
                out Vector2Int gridOrigin))
        {
            gridNodes=new GridNodes(gridDimensions.x,gridDimensions.y);
            gridWidth = gridDimensions.x;
            gridHeight = gridDimensions.y;
            originX = gridOrigin.x;
            originY = gridOrigin.y;
            openNodeList = new List<Node>();
            closeNodeList = new HashSet<Node>();
        }
        else
        {
            return false;
        }
        startNode=gridNodes.GetGridNode(startPos.x-originX,startPos.y-originY);
        targetNode=gridNodes.GetGridNode(endPos.x-originX,endPos.y-originY);
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                //TODO:错误写法，以后会改
                Vector3Int tilePos = new Vector3Int(i+originX, j+originY, 0);
                TileDetails tileDetails = GridMapManager.Instance.GetTileDetailsOnMousePosition(tilePos);
                if (tileDetails!=null)
                {
                    Node node = gridNodes.GetGridNode(i, j);
                    if (tileDetails.isNPCObstacle)
                    {
                        node.isObstacle = true;
                    }
                }
            }
        }
        return true;
    }
    /// <summary>
    /// 找到最短路径node添加到closeNodeList列表
    /// </summary>
    /// <returns></returns>
    
    private bool FindShortestPath()
    {   
        openNodeList.Add(startNode);
        while (openNodeList.Count > 0)
        {
            openNodeList.Sort();
            Node closNode = openNodeList[0];
            openNodeList.RemoveAt(0);
            closeNodeList.Add(closNode);
            if (closNode == targetNode)
            {
                pathFound = true;
                break;
            }
            //计算周围8个节点
            EvaluateNeighbourNodes(closNode);
        }
        return pathFound;
    }
    /// <summary>
    /// 评估周围八个点并得到对应的消耗值
    /// </summary>
    /// <param name="currentNode"></param>
    private void EvaluateNeighbourNodes(Node currentNode)
    {
        Vector2Int currentNodePos = currentNode.gridPosition;
        Node validNeighbourNode;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                validNeighbourNode = GetValidNeighbourNode(currentNodePos.x + i, currentNodePos.y + j);
                if (validNeighbourNode != null)
                {
                    if (!openNodeList.Contains(validNeighbourNode))
                    {
                        validNeighbourNode.gCost=currentNode.gCost+GetDistance(currentNode,validNeighbourNode);
                        validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                        //链接父节点
                        validNeighbourNode.parentNode = currentNode;
                        openNodeList.Add(validNeighbourNode);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 相邻两点的距离
    /// </summary>
    /// <param name="nodeA">起始点</param>
    /// <param name="nodeB">目标点</param>
    /// <returns></returns>
    private int GetDistance(Node nodeA,Node nodeB)
    {
        int xDistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int yDistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);
        if (xDistance > yDistance)
        {
            return 14 * yDistance + 10 * (xDistance - yDistance);
        }
        return 10 * (yDistance - xDistance)+14*xDistance;
    }
    /// <summary>
    /// 找到有效的Node，非故障，非已选择
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Node GetValidNeighbourNode(int x, int y)
    {
        if (x>=gridWidth||y>=gridHeight||x<0||y<0)
        {
            return null;
        }
        Node neighbourNode = gridNodes.GetGridNode(x, y);
        if (neighbourNode.isObstacle||closeNodeList.Contains(neighbourNode))
        {
            return null;
        }
        else
        {
            return neighbourNode;
        }
    }
}
