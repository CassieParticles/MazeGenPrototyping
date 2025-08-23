using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    private List<EventListenerComponent> listeners;

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
}
