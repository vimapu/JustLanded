using System.Collections.Generic;

public interface Subject<T>
{

    void Add(IListener<T> listener);


    void Detach(IListener<T> listener);


    void Notify(T notification);


}