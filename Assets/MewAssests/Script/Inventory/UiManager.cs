using UnityEngine;

public class UiManager : MonoBehaviour
{
    private static UiManager Instance = null;
    public static UiManager GetInstance()
    {
        return Instance;
    }
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        // DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
