using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public GameObject player;
    public Transform checkpoint;
    public GameObject currentEncounter;

    public float playerHp;
    public float playerFuel;

    private UiManager uIManager;

    private static CheckPointManager Instance = null;
    public static CheckPointManager GetInstance()
    {
        return Instance;
    }
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        uIManager = UiManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnReset()
    {
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        // uIManager.OnLoadCheckpoint();
        // StartCoroutine(uIManager.OnShowInv(false));
        // StartCoroutine(uIManager.OnShowButton(false));
        // uIManager.isInventoryOpen = false;
        // yield return new WaitForSeconds(3f);

        PlayerController playerControllers = player.GetComponent<PlayerController>();
        player.transform.position = checkpoint.position;

        // playerControllers.rb.linearVelocity = Vector2.zero;
        // playerControllers.currentHp = playerControllers.maxHp;
        // playerControllers.currentFuel = playerFuel;
        // playerControllers.webModifier = 1;
        // playerControllers.slowModifier = 1;

        yield return new WaitForSeconds(2f);

        Collider2D collider2D = player.GetComponent<Collider2D>();
        collider2D.enabled = true;
    }

    public void OnSetQuest(string header, string content)
    {
        // uIManager.SetQuest(header, content);
    }
}
