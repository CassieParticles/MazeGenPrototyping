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
            return AddNode(node, IndexFromPosition(position));
        }

        public MazeNode AddNode(MazeNode node, int index)
        {
            return nodeArray[index] = node;
        }

        public MazeNode GetNode(Vector2Int position)
        {
            return GetNode(IndexFromPosition(position));
        }

        public MazeNode GetNode(int index)
        {
            return nodeArray[index];
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


