using UnityEngine;

public class PlayerChangeRooms : MonoBehaviour
{
    [SerializeField] private GameEvent ChangeRoomEvent;
    Vector2 gridPosition;

    private void FixedUpdate()
    {
        //Changed grid position
        if(Mathf.Floor((transform.position.x+5) / 10) != gridPosition.x || Mathf.Floor((transform.position.z+5) / 10) != gridPosition.y)
        {
            Debug.Log("Change grid position");
            gridPosition.x = Mathf.Floor((transform.position.x+5) / 10);
            gridPosition.y = Mathf.Floor((transform.position.z+5) / 10);
            ChangeRoomEvent?.InvokeListeners();
        }
    }
}
