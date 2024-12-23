using System.Collections.Generic;

public abstract class Subject<T>
{
    public abstract List<IListener<T>> Listeners { get; set; }

    public void Add(IListener<T> listener)
    {
        if(Listeners == null) {
            Listeners = new List<IListener<T>>();
        }
        Listeners.Add(listener);
    }

    public void Detach(IListener<T> listener)
    {
        Listeners.Remove(listener);
    }

    public void Notify(T notification)
    {
        foreach (IListener<T> listener in Listeners)
        {
            listener.Notify(notification);
        }
    }

}