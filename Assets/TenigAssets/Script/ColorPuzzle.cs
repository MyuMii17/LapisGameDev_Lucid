using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum CodeColor { Blue, Green, Yellow, Red }
public class ColorPuzzle : InteractObject
{
    [Header("Password")]
    [SerializeField] private List<CodeColor> correctColors = new List<CodeColor> {CodeColor.Blue, CodeColor.Green, CodeColor.Yellow, CodeColor.Red };
    private readonly List<CodeColor> inputColor = new List<CodeColor>();

    [Header("GameObjects")]
    public Canvas passwordUi;
    public GameObject Door;
    public TextMeshProUGUI textMesh;

    [Header("Booleans")]
    private bool unlocked;

    [Header("Events")]
    public UnityEvent onUnlocked;
    public UnityEvent onWrongCode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        passwordUi.gameObject.SetActive(false);
        textMesh.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override IEnumerator OnInteract()
    {
        passwordUi.gameObject.SetActive(true);
        playerController.ResetVelocity();
        playerController.StopSound();
        playerController.enabled = false;
        
        yield return null;
        interactCorutine = null;
    }

    public void PressColor(CodeColor color)
    {
        if(unlocked) return;
        inputColor.Add(color);

        int index = inputColor.Count - 1;
        if (inputColor.Count < correctColors.Count) return;

        if(inputColor.SequenceEqual(correctColors))
        {
            animator.SetTrigger("Start");
            unlocked = true;
            playerController.enabled = true;
            passwordUi.gameObject.SetActive(false);
            gameObject.SetActive(false);
            textMesh.gameObject.SetActive(true);

            textMesh.text = "Correct";
            onUnlocked?.Invoke();
        }
        else
        {
            textMesh.gameObject.SetActive(true);
            Debug.Log("Input: " + string.Join(", ", inputColor));
            Debug.Log("Expected: " + string.Join(", ", correctColors));
            textMesh.text = "Wrong";
            onWrongCode?.Invoke();
        }
        inputColor.Clear();
    }

    //void WrongCode()
    //{
    //    Debug.Log("Wrong");
    //    inputColor.Clear();
    //    onWrongCode?.Invoke();
    //}

    public void ResetInput() => inputColor.Clear();

    public void ClosePasswordUI()
    {
        playerController.enabled = true;
        playerInteract.OnInteract();
        passwordUi.gameObject.SetActive(false);
    }
}
