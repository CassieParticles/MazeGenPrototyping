using UnityEngine;

public class Movement : MonoBehaviour
{

    private void Awake()
    {
        
    }


    private void OnEnable()
    {
        Debug.Log("Movement enabled");
    }

    private void OnDisable()
    {
        Debug.Log("Movement disabled");
    }
}
