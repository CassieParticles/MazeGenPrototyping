using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] RoomTemplates;

    private Maze.Graph graph;

    private void Awake()
    {
        graph = new Maze.Graph(10, 10, Vector2Int.zero);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
