using Maze;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] RoomTemplates;

    private Graph mazeGraph;

    private void Awake()
    {
        mazeGraph = new Graph(10, 10);

        mazeGraph.AddNode(new MazeNode(), new Vector2Int(3, 4));
        mazeGraph.AddNode(new MazeNode(), new Vector2Int(3, 5));
        mazeGraph.AddNode(new MazeNode(), new Vector2Int(3, 6));
        mazeGraph.AddNode(new MazeNode(), new Vector2Int(4, 5));
        mazeGraph.AddNode(new MazeNode(), new Vector2Int(5, 5));
        mazeGraph.AddNode(new MazeNode(), new Vector2Int(3, 1));
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
