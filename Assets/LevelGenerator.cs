using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(ObjectPoolSystem))]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private ObjectPoolSystem poolSystem;
    [SerializeField] private float segmentInterval = 5f;


    private float timer;

    void Start()
    {
        poolSystem = GetComponent<ObjectPoolSystem>();
        timer = segmentInterval;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateSegment();
    }

    void GenerateSegment()
    {

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = segmentInterval;

            GameObject newSegment = poolSystem.GetObject();
            Debug.Log(newSegment.name);

        }

    }
}
