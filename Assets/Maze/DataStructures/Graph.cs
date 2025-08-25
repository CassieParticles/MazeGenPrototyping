using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Graph
    {
        public MazeGrid grid { get; private set; }  //Grid that all nodes are stored in

        private int rootNode;   //Node the player is in

        public Graph(int mazeWidth, int mazeHeight, Vector2Int rootPosition)
        {
            MazeGrid grid = new MazeGrid(mazeWidth,mazeHeight);
            rootNode = grid.GetIndexFromPosition(rootPosition);
        }
    }
}

