using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    public Transform player;
    public Transform position;
    public Transform nextPosition;
    public float speed;
    public float smoothSpeed;
    public bool canChangePos;
    public bool OnPosiotion;
    public bool canMove;
    private float progress;
    private Vector3 pathProgression;
    private Vector3 direction;
    private Vector3 path;
    private int CameraLayer;
    public Collider hit;
    private Collider[] hits = null;
    void Start()
    {
        CameraLayer = LayerMask.GetMask("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

        if(position == null) return;
        if (!canMove && !OnPosiotion)
        {
            direction = (position.position - transform.position).normalized;
            transform.position += direction * speed * smoothSpeed * Time.deltaTime;
        }

        if (canMove)
        {
            float progress = Vector3.Dot(player.position - position.position, path)/ path.sqrMagnitude;
            progress = Mathf.Clamp01(progress);
            pathProgression = Vector3.Lerp(position.position, nextPosition.position, progress);

            if (!OnPosiotion)
            {
                hits = Physics.OverlapSphere(pathProgression, 0.25f, CameraLayer);
                if(hits.Length > 0)
                {
                    hit = hits[0];
                    OnPosiotion = true;
                }
            }

            if(hit != null)
            {
                transform.position = pathProgression;
                return;
            }
            else
            {
                direction = (pathProgression - transform.position).normalized;
                transform.position += direction * speed * smoothSpeed * Time.deltaTime;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CamPosition") && !canMove)
        {
            OnPosiotion = true;
        }
    }
    public void Change(CameraPosition change)
    {
        OnPosiotion = false;

        canMove = change.CanMove;
        position = change.Position;
        speed = change.Speed;
        smoothSpeed = change.Smooth;
        
        if (canMove)
        {
            nextPosition = change.NextPosition;

            path = nextPosition.position - position.position;

            progress = Vector3.Dot(player.position - position.position, path)/ path.sqrMagnitude;
            progress = Mathf.Clamp01(progress);

            pathProgression = Vector3.Lerp(position.position, nextPosition.position, progress);

            hits = Physics.OverlapSphere(pathProgression, 0.25f, CameraLayer);
            hit = null;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = OnPosiotion ? Color.red : Color.green;

        Gizmos.DrawSphere(pathProgression, 0.25f);
    }
}
