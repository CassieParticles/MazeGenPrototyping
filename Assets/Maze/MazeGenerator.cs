using Maze;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] RoomTemplates;
    [SerializeField] private int roomDepth = 3;

    private Graph mazeGraph;

    private PlayerMovement player;

    private Vector2Int[] RULDOrder = {Vector2Int.right,Vector2Int.up,Vector2Int.left,Vector2Int.down };

    private void Awake()
    {
        mazeGraph = new Graph(10, 10);

        player = FindAnyObjectByType<PlayerMovement>();

        CreateRoom(0b00001100,new Vector2Int(0, 0));

        mazeGraph.SetRootNode(new Vector2Int(0, 0));
        UpdateMaze();
    }

    public void PlayerChangeRooms()
    {
        Vector2Int newGridPos = Vector2Int.zero;

        newGridPos.x = (int)player.transform.position.x / 10;
        newGridPos.y = (int)player.transform.position.z / 10;

        Debug.Log("Updating root");
        //Set the root and update the grid with new orders
        mazeGraph.SetRootNode(newGridPos);

        UpdateMaze();
    }

    private void UpdateMaze()
    {
        //Breadth first traversal to remove orders too high and add new nodes on rooms too low
        List<int> visitedNodes = new List<int>();
        Queue<MazeNode> nodesToVisit = new Queue<MazeNode>();

        //Add root to the queue
        MazeNode root = mazeGraph.root;
        nodesToVisit.Enqueue(root);

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

            if(current.order < roomDepth)
            {
                //Add rooms around
                for(int i=0;i<4;++i)
                {
                    //ITERATE RULD ORDER
                    if(!current.neighbors[i] && (current.doorFlags & 0b00001000 >> i) > 0)
                    {
                        GenerateRandomNode(current.position + RULDOrder[i]);
                    }
                }
            }

            //Adding children to end ensures all nodes of this order are traversed before next order
            for(int i=0;i<4;++i)
            {
                //Null check
                MazeNode neighbor = current.neighbors[i];
                if (!neighbor){ continue; }

                nodesToVisit.Enqueue((MazeNode)neighbor);
            }

            //Checks to see if node should be destroyed
            if (current.order > roomDepth)
            {
                //Remove room
                mazeGraph.RemoveNode(current);
            }
        }
    }

    private MazeNode GenerateRandomNode(Vector2Int position)
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
        return CreateRoom(doorFlags, position);
    }

    private MazeNode CreateRoom(byte doorFlags, Vector2Int position)
    {
        //Create node and return it
        return mazeGraph.AddNode(doorFlags, position, RoomTemplates[(int)doorFlags]);
    }

}
