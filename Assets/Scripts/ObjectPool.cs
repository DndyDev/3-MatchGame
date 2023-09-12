using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
   {
    private Queue<T> objects = new Queue<T>();
    public T GetObject()
    {
        if (objects.Count > 0)
        {
            T obj = objects.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnObject (T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
   }