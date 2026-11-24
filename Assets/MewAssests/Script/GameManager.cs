using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject player;
    public bool isGameStart;
    public bool canStart;
    private static GameManager Instance = null;
    public static GameManager GetInstace()
    {
        return Instance;
    }
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        if(canStart && !isGameStart)
        {
            isGameStart = true;
            player.SetActive(true);
            mainCam.SetActive(true);
        }
    }
    public void OnGameStart()
    {
        canStart = true;
    }
}
