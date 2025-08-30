using System.Collections.Generic;
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

            //Set up connections
            MazeNode rightNode = grid.GetNode(position + Vector2Int.right);
            MazeNode upNode = grid.GetNode(position + Vector2Int.up);
            MazeNode leftNode = grid.GetNode(position + Vector2Int.left);
            MazeNode downNode = grid.GetNode(position + Vector2Int.down);

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
        MazeNode[] neighbors;   //Right, up, left, down

        public int order { get;private set; }

        public MazeNode RightNode { get => neighbors[0]; set { neighbors[0] = value; } }
        public MazeNode UpNode { get => neighbors[1]; set { neighbors[1] = value; } }
        public MazeNode LeftNode { get => neighbors[2]; set { neighbors[2] = value; } }
        public MazeNode DownNode { get => neighbors[3]; set { neighbors[3] = value; } }

        public MazeNode()
        {
            neighbors = new MazeNode[4];
        }

        public static implicit operator bool(MazeNode node) => node != null;
    }
}

