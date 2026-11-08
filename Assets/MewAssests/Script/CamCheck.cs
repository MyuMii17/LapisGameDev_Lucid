using UnityEngine;

public class CamCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        CameraContoller cam = other.GetComponentInParent<CameraContoller>();
        cam.OnPosiotion = true;
    }
}
