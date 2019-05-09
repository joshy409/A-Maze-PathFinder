using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// various helper classes 
namespace TowerDefense
{
    // game stages
    public enum Stage { build, defend };

    // tile types
    public enum TileType { ground, wall }; //turret, spawn, objective };

    public struct Index
    {
        public int y, x;

        public Index(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    public class Node : IComparable
    {
        public float g;
        public float h;
        public float f;
        public float traversalCost;
        public Node previousNode;
        public List<Node> nextNodes;
        public Index idx;

        public Node(int cost, Index _idx)
        {
            previousNode = null;
            nextNodes = new List<Node>();
            g = h = f = Int32.MaxValue;
            traversalCost = cost;

            idx = _idx;
        }

        private Node() { }

        public int CompareTo(object obj) // sort by g for dijkstra!!
        {
            Node b = (Node)obj; // run priority queue off Fs
            if (g < b.g) { return -1; }
            if (b.g > g) { return 1; }
            return 0;
        }

        // manhattan distance
        public float CalculateH(Node goalNode)
        {
            h = Math.Abs(goalNode.idx.x - idx.x) + Math.Abs(goalNode.idx.y - idx.y);
            // Debug.Log(idx.x + ", " + idx.y + ": " + h);
            return h;
        }
    }

}
