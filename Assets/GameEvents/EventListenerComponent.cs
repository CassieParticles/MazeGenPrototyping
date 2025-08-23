using UnityEngine;
using UnityEngine.Events;

public class EventListenerComponent : MonoBehaviour
{
    [SerializeField] private GameEvent eventSignal;
    [SerializeField] private UnityEvent events;

    private void OnEnable()
    {
        eventSignal.RegisterCallback(this);
    }

    private void OnDisable()
    {
        eventSignal.UnregisterCallback(this);
    }

    public void InvokeListener()
    {
        events.Invoke();
    }
}
