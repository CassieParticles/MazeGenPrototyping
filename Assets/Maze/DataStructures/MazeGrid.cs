using System;
using UnityEditor.Rendering.Universal;
using UnityEngine;

namespace Maze
{
    public class MazeGrid
    {
        private MazeNode[] nodeArray;

        public int width { get; private set; }
        public int height{ get; private set; }

        public MazeGrid(int width, int height)
        {
            nodeArray = new MazeNode[width * height];
            this.width = width;
            this.height = height;
        }

        public MazeNode AddNode(MazeNode node, Vector2Int position)
        {
            if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height){ return null; }
            return AddNode(node, IndexFromPosition(position));
        }

        public MazeNode AddNode(MazeNode node, int index)
        {
            return nodeArray[index] = node;
        }

        public MazeNode GetNode(Vector2Int position)
        {
            if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height){ return null; }
            return GetNode(IndexFromPosition(position));
        }

        public MazeNode GetNode(int index)
        {
            return nodeArray[index];
        }

        public void RemoveNode(MazeNode node)
        {
            //Remove neighbor references
            if (node.LeftNode){ node.LeftNode.RightNode = null; }
            if (node.UpNode){ node.UpNode.DownNode = null; }
            if (node.RightNode){ node.RightNode.LeftNode = null; }
            if (node.DownNode){ node.DownNode.UpNode = null; }

            //Destroy attached gameobject
            GameObject.Destroy(nodeArray[node.index].roomObject);

            //Remove node from array
            nodeArray[node.index] = null;
        }

        public void RemoveNode(Vector2Int position)
        {
            if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height){ return; }
            RemoveNode(IndexFromPosition(position));
        }

        public void RemoveNode(int index)
        {
            RemoveNode(GetNode(index));
        }

        public int IndexFromPosition(Vector2Int position)
        {
            return position.y * width + position.x;
        }

        public Vector2Int PositionFromIndex(int index)
        {
            return new Vector2Int(index % width, index / width);
        }
    }
}


