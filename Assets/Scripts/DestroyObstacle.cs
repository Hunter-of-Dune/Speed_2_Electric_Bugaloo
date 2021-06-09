using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float yLimit = -20f;
    public float zLimit = 25f;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Vehicle");
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy obstacles behind the player or fallen off the edge
        if(transform.position.y < yLimit | player.transform.position.z - transform.position.z > zLimit)
        {
            Destroy(gameObject);
        }
    }
}
