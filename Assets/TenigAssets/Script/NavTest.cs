using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavTest : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform destination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destination.position);
    }


}

