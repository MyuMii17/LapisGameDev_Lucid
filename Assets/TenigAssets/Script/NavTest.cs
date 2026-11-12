using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavTest : MonoBehaviour
{
    public NavMeshAgent agent;
    public InputAction action;
    [SerializeField] private Transform destination;
    private bool isGridGenerated;

    private void Awake()
    {
        action = InputSystem.actions.FindAction("Attack");
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destination.position);
    }


}

