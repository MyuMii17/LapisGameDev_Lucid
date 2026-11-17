using UnityEngine;

public class NoiseDectection : MonoBehaviour
{
    public NavTest enemyNav;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyNav = other.GetComponent<NavTest>();
        }
        if (other.gameObject.CompareTag("Player"))
        {
           
            var player = other.GetComponent<PlayerController>();
            if(player != null)
            {
               if(player.move.IsJumping && enemyNav != null)
                {
                    enemyNav.destination.transform.position = player.gameObject.transform.position;
                }
            }
        }


    }

}
