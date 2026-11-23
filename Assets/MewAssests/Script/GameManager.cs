using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
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
            Player.SetActive(true);
        }
    }
    public void OnGameStart()
    {
        canStart = true;
    }
}
