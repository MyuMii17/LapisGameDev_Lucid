using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public GameObject mainCam;
    public GameObject player;
    public bool isGameStart;
    public bool canStart;
    public GameObject menuUI;
    public bool isOn;
    [Header("Checkpoint Setting")]
    public bool isOnCheckpoint;
    private Coroutine checkpointCoroutine;
    public GameObject checkpointButton;
    public bool isShowButton;
    public GameObject fog;
    private ParticleSystem fogParticle;
    public CameraCheckpoint cameraCheckpoint;
    public PlayableDirector fade;
    public PlayableDirector fadeIn;
    public List<Animator> animators = new List<Animator>();
    public string animationTriggerName;
    private static GameManager Instance = null;
    public static GameManager GetInstace()
    {
        return Instance;
    }
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        if(Instance != null) DontDestroyOnLoad(gameObject);

        fogParticle = fog.GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if(canStart && !isGameStart)
        {
            isGameStart = true;
            player.SetActive(true);
            mainCam.SetActive(true);
        }

        if(cameraCheckpoint != null && !isShowButton)
        {
            isShowButton = true;
            checkpointButton.SetActive(true);
        }
    }
    public void OnGameStart()
    {
        canStart = true;
    }
    public void MenuUiSet()
    {
        if(isOnCheckpoint || isGameOver) return;
        isOn = !isOn;
        menuUI.SetActive(isOn);
    }
    public void OnRestart()
    {
        if(isOnCheckpoint) return;
        SceneManager.LoadScene(0);
    }

    public void OnCheckpoint()
    {
        isOnCheckpoint = true;
        if(checkpointCoroutine == null)
        {
            checkpointCoroutine = StartCoroutine(Checkpoint());
        }
    }
    IEnumerator Checkpoint()
    {
        OnClose();
        fade.Play();

        yield return new WaitForSeconds(1.5f);

        fog.SetActive(false);
        fogParticle.Pause();
        fogParticle.Clear();

        foreach(Animator animator in animators)
        {
            animator.SetBool(animationTriggerName, false);
        }
        
        cameraCheckpoint.player.transform.position = cameraCheckpoint.playerPosition;
        isGameOver = false;
        checkpointCoroutine = null;
        isOnCheckpoint = false;
    }
    public void OnExit()
    {
#if UNITY_STANDALONE_WIN
        Application.Quit();
#endif
    }
    public void OnClose()
    {
        isOn = false;
        menuUI.SetActive(false);
    }
}
