using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SegmentController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float recycleDelay;
    [SerializeField] private List<Transform> possibleSpawnPoints = new List<Transform>();
    [SerializeField] private int minObstacleCount;
    [SerializeField] private int maxObstacleCount;
    [SerializeField] private List<GameObject> possibleObstacles = new List<GameObject>();

    private Rigidbody rb;
    private List<GameObject> obstacles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Invoke("Recycle", recycleDelay);


    }

    private void OnEnable()
    {
        GenerateObstacles();
    }

    private void OnDisable()
    {
        ClearObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position + Vector3.back * Time.deltaTime * moveSpeed;
        rb.MovePosition(newPos);

    }

    void Recycle()
    {
        ObjectPoolSystem.Instance.ReturnObject(gameObject);
    }

    void GenerateObstacles()
    {
        int obstacleCount = Random.Range(minObstacleCount, maxObstacleCount);

        for (int i = 0; i < obstacleCount; i++)
        {
            int randIndex = Random.Range(0, possibleObstacles.Count);
            GameObject newObstacle = Instantiate(possibleObstacles[randIndex], this.transform);

            int randomSpawnPoint = Random.Range(0, possibleSpawnPoints.Count);
            Debug.Log("random spawn point index: " + randomSpawnPoint);
            Debug.Log("random pos: " + possibleSpawnPoints[randomSpawnPoint].transform.position);
            newObstacle.transform.position = possibleSpawnPoints[randomSpawnPoint].transform.position;
        }

    }

    void ClearObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            Destroy(obstacles[i]);
        }

        obstacles.Clear();
    }
}
