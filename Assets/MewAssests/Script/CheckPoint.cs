using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject encounter;
    public string questHeader;
    public string questContent;
    private CheckPointManager checkPointManager;
    public bool isSave;
    public bool isUpdateQuest;

    void Start()
    {
        checkPointManager = CheckPointManager.GetInstance();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isSave)
        {
            isSave = true;

            if (isUpdateQuest)
            {
                checkPointManager.OnSetQuest(questHeader, questContent);
            }

            checkPointManager.checkpoint = gameObject.transform;
            
            checkPointManager.player = collision.gameObject;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isSave)
        {
            isSave = true;

            if (isUpdateQuest)
            {
                checkPointManager.OnSetQuest(questHeader, questContent);
            }

            checkPointManager.currentEncounter = encounter;

            checkPointManager.checkpoint = gameObject.transform;

            checkPointManager.player = collision.gameObject;
        }
    }
}
