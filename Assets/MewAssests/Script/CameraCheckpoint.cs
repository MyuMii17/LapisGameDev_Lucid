using UnityEngine;

public class CameraCheckpoint : MonoBehaviour
{
    public GameObject player;
    public Vector3 playerPosition;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.GetInstace();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerPosition = player.transform.position;
            gameManager.cameraCheckpoint = this;
        }
    }
}
