using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectPoolSystem : MonoBehaviour
{
    // singleton pattern
    public static ObjectPoolSystem Instance;

    public List<GameObject> objectPrefabs = new List<GameObject>();
    public List<GameObject> pool = new List<GameObject>();

    public int poolSize = 10;

    private GameObject debugObj;

    private void Start()
    {
        // also singleton code
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }


        // initialize the object pool
        for (int i = 0; i < poolSize; i++)
        {
            int randomIndex = Random.Range(0, objectPrefabs.Count);
            GameObject newObj = Instantiate(objectPrefabs[randomIndex]);
            newObj.SetActive(false);
            pool.Add(newObj);
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
            int randIndex = Random.Range(0, pool.Count);
            GameObject newObj = pool[randIndex];
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
        pool.Add(go);
    }
}
