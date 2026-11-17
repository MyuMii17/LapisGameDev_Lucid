using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavTest : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 startDestPos;
    private Vector3 currentDestPos;
    [SerializeField] private Transform destination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        startDestPos = destination.position;
    }
    private void Start()
    {
        StartCoroutine(Patrol());
    }
    // Update is called once per frame
    void Update()
    {
        if(destination == null) return;
        agent.SetDestination(destination.position);
        currentDestPos = destination.position;
    }

    IEnumerator Patrol()
    {
        destination.transform.Translate(Vector3.forward);
        yield return new WaitForSeconds(3f);
        destination.transform.Translate(Vector3.right);
        yield return new WaitForSeconds(3f);
        destination.transform.Translate(Vector3.back);
        yield return new WaitForSeconds(3f);
        destination.position = startDestPos;

    }


}

