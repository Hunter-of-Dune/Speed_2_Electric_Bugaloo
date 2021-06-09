using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRoad : MonoBehaviour
{
    private GameObject player;
    private SpawnController spawnController;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Vehicle");
        spawnController = FindObjectOfType<SpawnController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.z - gameObject.transform.position.z > 25.0f)
        {
            gameObject.transform.position += new Vector3(0f, 0f,  205f);
            spawnController.SpawnObstacle(gameObject);
        }
    }
}
