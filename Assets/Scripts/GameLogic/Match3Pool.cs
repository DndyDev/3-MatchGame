using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class Match3Pool<T> where T : Match3Node
    {
        private Queue<T> objects = new Queue<T>();
        public T GetObject(int id,Sprite sprite, Vector2 position)
        {
            if (objects.Count > 0)
            {
                T obj = objects.Dequeue();
                obj.id = id;
                obj.sprite.sprite = sprite;
                obj.transform.position = position;
                obj.gameObject.SetActive(true);
                return obj;
            }
            return null; // Exception or Log
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            objects.Enqueue(obj);
        }
        public void Clear()
        {
            objects.Clear();
        }
    }
}
