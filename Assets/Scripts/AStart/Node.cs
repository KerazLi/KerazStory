using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector2Int gridPosition;//网格坐标
    public int gCost=0;//距离start格子的距离
    public int hCost=0;//距离end格子的距离
    public int fCost=>gCost+hCost;//总距离
    public bool isObstacle=false;//是否是障碍物
    public Node parentNode;//父节点
    public Node(Vector2Int gridPosition)
    {
        this.gridPosition=gridPosition;
        parentNode=null;
    }

    public int CompareTo(Node other)
    {
        int result=fCost.CompareTo(other.fCost);
        if (result == 0)
        {
            result=hCost.CompareTo(other.gCost);
        }
        return result;
    }
}
