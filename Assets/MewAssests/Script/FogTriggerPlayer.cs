using UnityEngine;

public class FogTriggerPlayer : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.GetInstace();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.OnDead(true);
            gameManager.isGameOver = true;
        }
    }
}
