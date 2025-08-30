using Maze;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] RoomTemplates;

    private Graph mazeGraph;

    private void Awake()
    {
        mazeGraph = new Graph(10, 10);

        mazeGraph.AddNode(new MazeNode(0b00000111), new Vector2Int(2, 2));  //Left
        mazeGraph.AddNode(new MazeNode(0b00001101), new Vector2Int(3, 2));  //Right
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
