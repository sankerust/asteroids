using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
  public static ObjectPool SharedInstance;
  public List<Pool> pools;
  public Dictionary<string, Queue<GameObject>> poolDictionary;

  [System.Serializable]
  public class Pool
  {
    public string tag;
    public GameObject prefab;
    public int size;
  }

  void Awake()
  {
    SharedInstance = this;
    poolDictionary = new Dictionary<string, Queue<GameObject>>();

    foreach (Pool pool in pools)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        poolDictionary.Add(pool.tag, objectPool);
    }
  }

  public GameObject GetPooledObject(string tag)
  {
    if (!poolDictionary.ContainsKey(tag))
    {
      Debug.LogWarning("PoolTag mismatch");
      return null;
    }

    if (poolDictionary[tag].Count == 0)
    {
      return null;
    }

    GameObject objectToGet = poolDictionary[tag].Dequeue();

    return objectToGet;
  }

  public void ReturnToPool(string tag, GameObject obj)
  {
    poolDictionary[tag].Enqueue(obj);
  }
}
