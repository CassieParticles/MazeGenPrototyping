using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Maze
{
    public class Graph
    {
        //Internal grid to store nodes
        public MazeGrid grid { get; private set; }

        public MazeNode root { get; private set; }

        //Breadth first order of nodes in graph
        public List<MazeNode> BFG { get; private set; }
        //Where each order starts
        public List<int> orderIndices{ get; private set; }

        public Graph(int width, int height)
        {
            grid = new MazeGrid(width, height);

            root = null;

            BFG = new List<MazeNode>();
            orderIndices = new List<int>();
        }

        public void SetRootNode(MazeNode node)
        {
            List<int> checkedNodes = new List<int>();

            root = node;
        }

        public void SetRootNode(Vector2Int position)
        {
            if(position.x < 0 || position.x >= grid.width || position.y < 0 || position.y >= grid.height){ return; }
            SetRootNode(grid.IndexFromPosition(position));
        }

        public void SetRootNode(int index)
        {
            SetRootNode(grid.GetNode(index));
        }

        //Update the BFG (Breadth first graph) of the maze graph
        public void UpdateBFG()
        {
            //Clear old BFG
            BFG.Clear();
            orderIndices.Clear();

            //Perform a breadth first traversal of the graph
            Queue<MazeNode> NextOrderNodes = new Queue<MazeNode>();
            Queue<MazeNode> ThisOrderNodes = new Queue<MazeNode>();

            List<int> nodesVisited = new List<int>();

            int currentOrder = 0;

            //Queue the root as the first node to visit
            ThisOrderNodes.Enqueue(root);

            //Visit nodes untill all nodes are visited
            while (ThisOrderNodes.Count > 0 || NextOrderNodes.Count > 0)
            {
                //Visit all nodes of this order
                while (ThisOrderNodes.Count > 0)
                {
                    MazeNode current = ThisOrderNodes.Dequeue();
                    //Skip nodes that have already been visited
                    if (nodesVisited.Contains(current.index))
                    {
                        continue;
                    }

                    //Add node to BFG
                    nodesVisited.Add(current.index);
                    BFG.Add(current);
                    current.order = currentOrder;

                    //Add children to end of queue (will be visited after all nodes of current order)
                    for (int i = 0; i < 4; ++i)
                    {
                        //Null check
                        MazeNode neighbor = current.neighbors[i];
                        if (!neighbor || nodesVisited.Contains(neighbor.index))
                        { continue; }

                        //Enqueue neighbor
                        NextOrderNodes.Enqueue(neighbor);
                    }
                }

                //Swap the queues, and clear the new Next order nodes (reduces allocations)
                Queue<MazeNode> temp = ThisOrderNodes;
                ThisOrderNodes = NextOrderNodes;
                NextOrderNodes = temp;

                //Add the index the next order starts at
                orderIndices.Add(BFG.Count);

                currentOrder++;

                NextOrderNodes.Clear();
            }
        }

        public MazeNode AddNode(byte doorFlags, Vector2Int position, GameObject roomObject)
        {
            if (position.x < 0 || position.x >= grid.width || position.y < 0 || position.y >= grid.height){ return null; }
            return AddNode(doorFlags, grid.IndexFromPosition(position),roomObject);
        }
        public MazeNode AddNode(byte doorFlags, int index, GameObject roomObject)
        {
            //Don't override node
            if (grid.GetNode(index)){ return grid.GetNode(index); }

            Vector2Int position = grid.PositionFromIndex(index);

            //Add node
            MazeNode node = grid.AddNode(new MazeNode(doorFlags,grid.PositionFromIndex(index),index,roomObject), index);

            //Set up connections (only if those doors exist)
            MazeNode rightNode = node.RightDoor ? grid.GetNode(position + Vector2Int.right) : null;
            MazeNode upNode = node.UpDoor ? grid.GetNode(position + Vector2Int.up) : null;
            MazeNode leftNode = node.LeftDoor ? grid.GetNode(position + Vector2Int.left) : null;
            MazeNode downNode = node.DownDoor ? grid.GetNode(position + Vector2Int.down) : null;

            int minOrder = 9999;

            if(rightNode)
            {
                node.RightNode = rightNode;
                rightNode.LeftNode = node;
                minOrder = Mathf.Min(minOrder, rightNode.order);
            }
            if (upNode)
            {
                node.UpNode = upNode;
                upNode.DownNode = node;
                minOrder = Mathf.Min(minOrder, upNode.order);
            }
            if (leftNode)
            {
                node.LeftNode = leftNode;
                leftNode.RightNode = node;
                minOrder = Mathf.Min(minOrder, leftNode.order);
            }
            if (downNode)
            {
                node.DownNode = downNode;
                downNode.UpNode = node;
                minOrder = Mathf.Min(minOrder, downNode.order);
            }


            node.order = minOrder + 1;

            //Insert the node into the BFG
            if (orderIndices.Count <= node.order)
            {
                orderIndices.Add(BFG.Count);
                BFG.Add(node);
            }
            else
            {
                BFG.Insert(orderIndices[node.order], node);
            }


            //Update the order indices to reflect where each order starts
            for(int i=node.order;i<orderIndices.Count;++i)
            {
                orderIndices[i]++;
            }

            //Return the node
            return node;
        }

        public void RemoveNode(MazeNode node)
        {
            //Remove node from BFG
            BFG.Remove(node);

            //Iterate through later orders to reflect where they now start
            for(int i=0;i<node.order;++i)
            {
                orderIndices[i]--;
            }

            grid.RemoveNode(node);
        }

        public void RemoveNode(Vector2Int position)
        {
            RemoveNode(grid.GetNode(position));
        }

        public void RemoveNode(int index)
        {
            RemoveNode(grid.GetNode(index));
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
        public MazeNode[] neighbors { get; private set; }   //Right, up, left, down

        //The game object for the room in the scene
        public GameObject roomObject { get; private set; }

        public MazeNode RightNode { get => neighbors[0]; set { neighbors[0] = value; } }
        public MazeNode UpNode { get => neighbors[1]; set { neighbors[1] = value; } }
        public MazeNode LeftNode { get => neighbors[2]; set { neighbors[2] = value; } }
        public MazeNode DownNode { get => neighbors[3]; set { neighbors[3] = value; } }

        //Doors to other rooms (connections that could be made)
        public byte doorFlags { get; private set; } //Layout 0b0000RULD

        public Vector2Int position { get; private set; }
        public int index { get;private set; }

        public bool RightDoor { get => (doorFlags & 0b00001000) == 0b00001000; }
        public bool UpDoor { get => (doorFlags & 0b00000100) == 0b00000100; }
        public bool LeftDoor { get => (doorFlags & 0b00000010) == 0b00000010; }
        public bool DownDoor { get => (doorFlags & 0b00000001) == 0b00000001; }

        public int order;

        public MazeNode(byte doorFlags,Vector2Int position, int index, GameObject roomObject)
        {
            neighbors = new MazeNode[4];
            this.doorFlags = doorFlags;

            this.position = position;
            this.index = index;

            this.roomObject = GameObject.Instantiate(roomObject);

            this.roomObject.transform.position = new Vector3(position.x * 10, 0, position.y * 10);
        }

        ~MazeNode()
        {
            Debug.Log("Node destroyed");
        }

        public static implicit operator bool(MazeNode node) => node != null;
    }
}

