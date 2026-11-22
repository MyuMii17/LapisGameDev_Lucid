using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzle : InteractionObject
{
    public List<int> correctPassword = new List<int>();
    public List<int> inputPassword = new List<int>();
    public Canvas passwordUi;
    public override void GetInteract(GameObject interactor)
    {
        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();

        getInteract = playerController.interact.IsHold;
        
        passwordUi.gameObject.SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        passwordUi.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
