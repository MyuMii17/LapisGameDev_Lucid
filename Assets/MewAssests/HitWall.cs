using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HitWall : MonoBehaviour
{
    public TwoBoneIKConstraint constraint;
    public Transform target;
    public Transform shoulderPosition;
    private PlayerController playerController;
    public float radius;
    public float maxDistance;
    public int boxLayer;
    public float wallOffset;
    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        boxLayer = LayerMask.GetMask("Box");
    }
    private RaycastHit hit;
    public bool isHit;
    void FixedUpdate()
    {
        if(Physics.SphereCast(shoulderPosition.position, radius, playerController.move.MoveDirection, out hit, maxDistance, boxLayer))
        {
            Vector3 handPosition = hit.point + hit.normal * wallOffset;
            target.position = handPosition;
            // target.rotation = Quaternion.LookRotation(playerController.climb.WallDirection);
            isHit = true;
            // constraint.weight = Mathf.Clamp01(blend);
        }
        else
        {
            isHit = false;
        }
    }

    private void OnDrawGizmos()
    {
        if(playerController == null) return;
        Gizmos.color = isHit ? Color.red : Color.green; // ถ้าชนจะเป็นสีแดง ถ้าไม่ชนจะเป็นสีเขียว

        Vector3 startCenter = shoulderPosition.position;
        Vector3 direction = playerController.move.MoveDirection;

        if (isHit)
        {
            // วาดเส้นจากจุดเริ่มต้นไปยังจุดที่ชน (ดึงตำแหน่ง Center ของทรงกลม ณ จุดที่ชนออกมา)
            Vector3 hitCenter = startCenter + direction * hit.distance;
            
            // วาดรูปทรงกลมจุดเริ่มต้น และ จุดที่เกิดการชน
            Gizmos.DrawWireSphere(startCenter, radius);
            Gizmos.DrawWireSphere(hitCenter, radius);
            
            // วาดเส้นเชื่อมต่อแสดงแกนการเคลื่อนที่
            Gizmos.DrawLine(startCenter, hitCenter);
        }
        else
        {
            // หากไม่ชนอะไรเลย ให้วาดระยะทางสูงสุดที่กำหนดไว้
            Vector3 endCenter = startCenter + direction * maxDistance;

            Gizmos.DrawWireSphere(startCenter, radius);
            Gizmos.DrawWireSphere(endCenter, radius);
            Gizmos.DrawLine(startCenter, endCenter);
        }
    }
}
