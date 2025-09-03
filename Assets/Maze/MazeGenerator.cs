using Maze;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] RoomTemplates;
    [SerializeField] private int roomDepth;

    private Graph mazeGraph;

    private PlayerMovement player;

    private void Awake()
    {
        mazeGraph = new Graph(10, 10);

        player = FindAnyObjectByType<PlayerMovement>();

        for(int x=0;x<10;++x)
        {
            for(int y=0;y<10;++y)
            {
                GenerateRandomNode(new Vector2Int(x, y), ref mazeGraph);
            }
        }

        mazeGraph.SetRootNode(new Vector2Int(0, 0));
    }

    public void PlayerChangeRooms()
    {
        Vector2Int newGridPos = Vector2Int.zero;

        newGridPos.x = (int)player.transform.position.x / 10;
        newGridPos.y = (int)player.transform.position.z / 10;

        Debug.Log("Updating root");
        UpdateMaze(newGridPos, ref mazeGraph);
    }

    private void UpdateMaze(Vector2Int rootGridPosition, ref Graph mazeGraph)
    {
        //Set the root and update the grid with new orders
        mazeGraph.SetRootNode(rootGridPosition);

        //Breadth first traversal to remove orders too high and add new nodes on rooms too low
        List<int> visitedNodes = new List<int>();
        Queue<MazeNode> nodesToVisit = new Queue<MazeNode>();

        //Add root to the queue
        MazeNode root = mazeGraph.root;
        nodesToVisit.Enqueue(root);
        visitedNodes.Add(root.index);

        //Iterate through all nodes still yet to visit
        while(nodesToVisit.Count > 0)
        {
            //Add the current node
            MazeNode current = nodesToVisit.Dequeue();

            //Skip node if it's already visited
            if(visitedNodes.Contains(current.index))
            {
                continue;
            }

            visitedNodes.Add(current.index);

            

            //Adding children to end ensures all nodes of this order are traversed before next order
            for(int i=0;i<4;++i)
            {
                //Null check
                MazeNode neighbor = current.neighbors[i];
                if (!neighbor){ continue; }

                //Checks to see if room should be destroyed or new room added

                nodesToVisit.Enqueue((MazeNode)neighbor);
            }
        }
    }

    private MazeNode GenerateRandomNode(Vector2Int position, ref Graph mazeGraph)
    {
        //Generate bit flags for where doors are and aren't
        byte doorFlags = 0b0000000;

        MazeNode rightNode = mazeGraph.GetNode(position + Vector2Int.right);
        MazeNode upNode = mazeGraph.GetNode(position + Vector2Int.up);
        MazeNode leftNode = mazeGraph.GetNode(position + Vector2Int.left);
        MazeNode downNode = mazeGraph.GetNode(position + Vector2Int.down);

        //Right door
        if (rightNode)
        {
            doorFlags |= (rightNode.LeftDoor ? (byte)0b00001000 : (byte)0b00000000);
        }
        else
        {
            doorFlags |= (Random.value > 0.5f ? (byte)0b00001000 : (byte)0b00000000);
        }

        //Up door
        if (upNode)
        {
            doorFlags |= (upNode.DownDoor ? (byte)0b00000100 : (byte)0b00000000);
        }
        else
        {
            doorFlags |= (Random.value > 0.5f ? (byte)0b00000100 : (byte)0b00000000);
        }

        //Left door
        if (leftNode)
        {
            doorFlags |= (leftNode.RightDoor ? (byte)0b00000010 : (byte)0b00000000);
        }
        else
        {
            doorFlags |= (Random.value > 0.5f ? (byte)0b00000010 : (byte)0b00000000);
        }

        //Down door
        if (downNode)
        {
            doorFlags |= (downNode.UpDoor ? (byte)0b00000001 : (byte)0b00000000);
        }
        else
        {
            doorFlags |= (Random.value > 0.5f ? (byte)0b00000001 : (byte)0b00000000);
        }

        //Create node and return it
        return CreateRoom(doorFlags, position, ref mazeGraph);
    }

    private MazeNode CreateRoom(byte doorFlags, Vector2Int position, ref Graph mazeGraph)
    {
        //Create node and return it
        return mazeGraph.AddNode(doorFlags, position, GameObject.Instantiate(RoomTemplates[(int)doorFlags]));
    }

}
