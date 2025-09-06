using UnityEngine;

public class Room
{
    //Iterable directions, useful for neighbor calculation using positions
    public static readonly Vector2Int[] RULDOrder = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

    public Room(Vector2Int position, int index, byte roomFlags)
    {
        this.position = position;
        this.index = index;

        this.roomFlags = roomFlags;
        neighbors = new Room[4];
    }

    //Position within the world, useful for a room to have this
    public Vector2Int position { get; private set; }
    public int index { get; private set; }

    //Data on graph connections, if connection is possible, and the room connected to
    public byte roomFlags { get; private set; }
    public Room[] neighbors;

    //Accessors, making accessing specific directions easier
    public bool RightConnection { get { return (roomFlags & 0b00001000) > 0; } }
    public bool UpConnection { get { return (roomFlags & 0b00000100) > 0; } }
    public bool LeftConnection { get { return (roomFlags & 0b00000010) > 0; } }
    public bool DownConnection { get { return (roomFlags & 0b00000001) > 0; } }

    public Room RightRoom { get { return neighbors[0]; } set { neighbors[0] = value; } }
    public Room UpRoom { get { return neighbors[1]; } set { neighbors[1] = value; } }
    public Room LeftRoom { get { return neighbors[2]; } set { neighbors[2] = value; } }
    public Room DownRoom { get { return neighbors[3]; } set { neighbors[3] = value; } }
}

public class RoomGrid
{
    
}

public class RoomGraph
{
    
}
