using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Maze
{
    public class Graph
    {
        //Internal grid to store nodes
        public MazeGrid grid { get; private set; }

        public MazeNode root { get; private set; }

        public Graph(int width, int height)
        {
            grid = new MazeGrid(width, height);

            root = null;
        }

        public MazeNode AddNode(MazeNode node, Vector2Int position)
        {
            return AddNode(node, grid.IndexFromPosition(position));
        }

        public MazeNode AddNode(MazeNode node, int index)
        {
            Vector2Int position = grid.PositionFromIndex(index);

            //Add node
            node = grid.AddNode(node, index);

            //Set up connections (only if those doors exist)
            MazeNode rightNode = node.RightDoor ? grid.GetNode(position + Vector2Int.right) : null;
            MazeNode upNode = node.UpDoor ? grid.GetNode(position + Vector2Int.up) : null;
            MazeNode leftNode = node.LeftDoor ? grid.GetNode(position + Vector2Int.left) : null;
            MazeNode downNode = node.DownDoor ? grid.GetNode(position + Vector2Int.down) : null;

            if(rightNode)
            {
                node.RightNode = rightNode;
                rightNode.LeftNode = node;
            }
            if (upNode)
            {
                node.UpNode = upNode;
                upNode.DownNode = node;
            }
            if (leftNode)
            {
                node.LeftNode = leftNode;
                leftNode.RightNode = node;
            }
            if (downNode)
            {
                node.DownNode = downNode;
                downNode.UpNode = node;
            }

            //Return the node
            return node;
        }

        public MazeNode GetNode(Vector2Int position)
        {
            return grid.GetNode(position);
        }

        public MazeNode GetNode(int index)
        {
            return grid.GetNode(index);
        }
    }



    public class MazeNode
    {
        //Neighbors it's connected to (only ones that exist)
        MazeNode[] neighbors;   //Right, up, left, down

        public MazeNode RightNode { get => neighbors[0]; set { neighbors[0] = value; } }
        public MazeNode UpNode { get => neighbors[1]; set { neighbors[1] = value; } }
        public MazeNode LeftNode { get => neighbors[2]; set { neighbors[2] = value; } }
        public MazeNode DownNode { get => neighbors[3]; set { neighbors[3] = value; } }

        //Doors to other rooms (connections that could be made)
        private byte doorFlags; //Layout 0b0000RULD

        public bool RightDoor { get => (doorFlags & 0b00001000) == 0b00001000; }
        public bool UpDoor { get => (doorFlags & 0b00000100) == 0b00000100; }
        public bool LeftDoor { get => (doorFlags & 0b00000010) == 0b00000010; }
        public bool DownDoor { get => (doorFlags & 0b00000001) == 0b00000001; }

        public int order { get; private set; }

        public MazeNode(byte doorFlags)
        {
            neighbors = new MazeNode[4];
            this.doorFlags = doorFlags;
        }

        public static implicit operator bool(MazeNode node) => node != null;
    }
}

