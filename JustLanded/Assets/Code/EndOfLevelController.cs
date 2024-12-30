using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelController : MonoBehaviour, Subject<EndOfLevelEvent>
{

    private List<IListener<EndOfLevelEvent>> _listeners;

    void Awake()
    {
        _listeners = new List<IListener<EndOfLevelEvent>>();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("End of level");
            Notify(new EndOfLevelEvent());
        }
    }

    public void Add(IListener<EndOfLevelEvent> listener)
    {
        _listeners.Add(listener);
    }

    public void Detach(IListener<EndOfLevelEvent> listener)
    {
        _listeners.Remove(listener);
    }

    public void Notify(EndOfLevelEvent notification)
    {
        foreach (IListener<EndOfLevelEvent> listener in _listeners)
        {
            listener.Notify(notification);
        };
    }
}
