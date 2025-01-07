using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGearController : MonoBehaviour, Subject<GearCollectedEvent>
{

    [SerializeField] float value = 1f;

    private List<IListener<GearCollectedEvent>> _listeners;

    void Awake()
    {
        _listeners = new List<IListener<GearCollectedEvent>>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Notify(new GearCollectedEvent(value));
            Destroy(gameObject);
        }
    }

    public void Add(IListener<GearCollectedEvent> listener)
    {
        _listeners.Add(listener);
    }

    public void Detach(IListener<GearCollectedEvent> listener)
    {
        _listeners.Remove(listener);
    }

    public void Notify(GearCollectedEvent notification)
    {
        foreach (IListener<GearCollectedEvent> listener in _listeners)
        {
            listener.Notify(notification);
        }
    }
}
