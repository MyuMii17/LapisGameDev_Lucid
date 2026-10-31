using UnityEngine;

public class CamareContoller : MonoBehaviour
{
    public Transform player;
    public float posY;
    public float posZ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + posY, player.position.z + posZ);
    }
}
