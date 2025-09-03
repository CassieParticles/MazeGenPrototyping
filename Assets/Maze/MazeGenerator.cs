using Maze;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] RoomTemplates;

    private Graph mazeGraph;

    private void Awake()
    {
        mazeGraph = new Graph(10, 10);

        mazeGraph.AddNode(0b00001111, new Vector2Int(3, 2));    //Root

        mazeGraph.AddNode(0b00001011, new Vector2Int(4, 2));    //Top and Right (connected as a loop)
        mazeGraph.AddNode(0b00000110, new Vector2Int(5, 2));
        mazeGraph.AddNode(0b00000011, new Vector2Int(5, 3));
        mazeGraph.AddNode(0b00001110, new Vector2Int(4, 3));
        mazeGraph.AddNode(0b00001001, new Vector2Int(3, 3));
        mazeGraph.AddNode(0b00000011, new Vector2Int(4, 4));
        mazeGraph.AddNode(0b00001000, new Vector2Int(3, 4));
        mazeGraph.AddNode(0b00001101, new Vector2Int(4, 1));
        mazeGraph.AddNode(0b00000110, new Vector2Int(4, 0));
        mazeGraph.AddNode(0b00001000, new Vector2Int(3, 0));

        mazeGraph.AddNode(0b00000110, new Vector2Int(3, 1));    //Bottom
        mazeGraph.AddNode(0b00001011, new Vector2Int(2, 1));
        mazeGraph.AddNode(0b00001000, new Vector2Int(1, 1));
        mazeGraph.AddNode(0b00000100, new Vector2Int(2, 0));

        mazeGraph.AddNode(0b00001100, new Vector2Int(2, 2));    //Left
        mazeGraph.AddNode(0b00000011, new Vector2Int(2, 3));
        mazeGraph.AddNode(0b00001010, new Vector2Int(1, 3));
        mazeGraph.AddNode(0b00001000, new Vector2Int(0, 3));

        GenerateRandomNode(new Vector2Int(5, 1), ref mazeGraph);

        mazeGraph.SetRootNode(new Vector2Int(3, 2));

        Debug.Log(mazeGraph.root);
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
        if(rightNode)
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
        return mazeGraph.AddNode(doorFlags, position);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
