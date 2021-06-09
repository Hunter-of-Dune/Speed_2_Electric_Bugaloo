using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] obstacles = new GameObject[4];
    public List<GameObject> roads = new List<GameObject>();

    [SerializeField] private float xBounds;
    [SerializeField] private float zBounds;

    // Start is called before the first frame update
    void Start()
    {
        roads.AddRange(GameObject.FindGameObjectsWithTag("Road"));
        foreach(GameObject road in roads)
        {
            SpawnObstacle(road);
        }

    }

    public void SpawnObstacle(GameObject spawnLocation)
    {
        int index = Random.Range(0, 4);
        Instantiate(obstacles[index], spawnLocation.transform.position + new Vector3(Random.Range(-xBounds, xBounds), 0.2f, Random.Range(-zBounds, zBounds)), obstacles[index].transform.rotation);
    }

}
