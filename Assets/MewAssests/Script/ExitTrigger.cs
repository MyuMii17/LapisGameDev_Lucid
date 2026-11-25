using System.Collections;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
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
            gameManager.fadeIn.Play();
            StartCoroutine(OnMenu());
        }
    }

    IEnumerator OnMenu()
    {
        yield return new WaitForSeconds(0.25f);
        gameManager.OnWake(true);
        gameManager.isGameOver = true;
    }
}
