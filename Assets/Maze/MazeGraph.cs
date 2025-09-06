using UnityEngine;

public class Room
{
    //Iterable directions, 
    public static readonly Vector2Int[] RULDOrder = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

    public Room(Vector2Int position, int index, byte roomFlags)
    {
        this.position = position;
        this.index = index;

        this.roomFlags = roomFlags;
        neighbors = new Room[4];
    }

    public Vector2Int position { get; private set; }
    public int index { get; private set; }

    public byte roomFlags { get; private set; }
    public Room[] neighbors;
}

public class RoomGrid
{
    
}

public class RoomGraph
{
    
}
