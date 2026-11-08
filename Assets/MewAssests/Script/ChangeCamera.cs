using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public GameObject target;
    private CameraContoller cam;
    public CameraPosition position;
    void Awake()
    {
        cam = target.GetComponent<CameraContoller>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && cam.canChangePos && cam.position != position.Position)
        {
            cam.canChangePos = false;
            cam.Change(position);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cam.canChangePos = true;
        }
    }
}
