using UnityEngine;

namespace Maze
{
    public struct Node
    {
        public Node(Vector2Int position, byte connectionFlags, ref MazeGrid mazeGrid)
        {
            connections = new int[4];
            this.connectionFlags = connectionFlags;
        }

        private byte connectionFlags;
        public bool RightConnection { get => (connectionFlags & 0b10000000) == 0b10000000;}
        public bool UpConnection { get => (connectionFlags & 0b01000000) == 0b01000000;}
        public bool LeftConnection { get => (connectionFlags & 0b00100000) == 0b00100000;}
        public bool DownConnection { get => (connectionFlags & 0b00010000) == 0b00010000;}

        private int[] connections;  //Right, up, left, down
        public int RightNode { get => connections[0]; set { connections[0] = value; } }
        public int UpNode { get => connections[1]; set { connections[1] = value; } }
        public int LeftNode { get => connections[2]; set { connections[2] = value; } }
        public int DownNode { get => connections[3]; set { connections[3] = value; } }

    }

    public class MazeGrid
    {
        private Node[] nodeGrid;

        public int width { get; private set; }
        public int height { get; private set; }

        public MazeGrid(int width, int height)
        {
            nodeGrid = new Node[width * height];
        }

        public ref Node GetNode(Vector2Int position)
        {
            return ref GetNode(GetIndexFromPosition(position));
        }

        public ref Node GetNode(int index)
        {
            return ref nodeGrid[index];
        }

        public void SetNode(Node node, Vector2Int position)
        {
            SetNode(node, GetIndexFromPosition(position));
        }

        public void SetNode(Node node, int index)
        {
            nodeGrid[index] = node;
        }

        public int GetIndexFromPosition(Vector2Int position)
        {
            return position.y * width + position.x;
            
        }

        public Vector2Int GetPositionFromIndex(int index)
        {
            return new Vector2Int(index % width, index / width);
        }
    }
}


