using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyBagScript : MonoBehaviour
{
    //[Header("Componenet References")]
    [SerializeField] private GameObject impactParticle;
    [SerializeField] private GameObject impactDebris;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //What should be counted as a collision is handled with layers.  This should be on enemy projectiles layer.  Enemy projectile layer will ignore NPC's and go through them.

        //create particle effect
        Instantiate(impactParticle, transform.position, transform.rotation);
        //spawn 1 - 2 cement debris around impact
        for (int i = 1; i <= (int) Random.Range(1, 3); i++) {            
            Instantiate(impactDebris, new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y, transform.position.z), transform.rotation);
                }
        Destroy(gameObject);

        
        
    }
}
