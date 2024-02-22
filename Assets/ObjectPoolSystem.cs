using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectPoolSystem : MonoBehaviour
{
    public GameObject objectPrefab;
    public Queue<GameObject> pool = new Queue<GameObject>();

    public int poolSize = 10;

    private GameObject debugObj;

    private void Start()
    {
        // initialize the object pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObj = Instantiate(objectPrefab);
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }
    }

    [Button("Get Object")]
    public void DebugGetObject()
    {
        debugObj = GetObject();
    }

    public GameObject GetObject()
    {

        if (pool.Count > 0)
        {
            GameObject newObj = pool.Dequeue();
            newObj.SetActive(true);
            return newObj;
        }
        else
        {
            return null;
        }
    }

    [Button("Return Object")]
    public void DebugReturnObject()
    {
        if (debugObj)
            ReturnObject(debugObj);
    }

    public void ReturnObject(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}
