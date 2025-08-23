using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    private List<EventListenerComponent> listeners;

    //For getting events without using serialized fields
    [SerializeField] string eventName;
    private static Dictionary<string, GameEvent> events;

    private void Awake()
    {
        events.Add(eventName, this);
        Debug.Log(eventName);
    }
    public void RegisterCallback(EventListenerComponent listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterCallback(EventListenerComponent listener)
    {
        listeners.Remove(listener);
    }

    public void InvokeListeners()
    {
        foreach (EventListenerComponent listener in listeners)
        {
            listener.InvokeListener();
        }
    }
    public static GameEvent GetEvent(string name)
    {
        return events[name];
    }

}
