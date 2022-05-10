using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyBagScript : MonoBehaviour
{
    //[Header("Componenet References")]
    [SerializeField] private GameObject impactParticle;
    //[SerializeField] private GameObject impactDebris;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _damage;
    [SerializeField] private float _maxDamageMultiplier;


    private GameObject _player;
    // Start is called before the first frame update

   void Update()
    {
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //What should be counted as a collision is handled with layers.  This should be on enemy projectiles layer.  Enemy projectile layer will ignore NPC's and go through them.

        //create particle effect
        Instantiate(impactParticle, transform.position, transform.rotation);


        //spawn 1 - 2 cement debris around impact
        //for (int i = 1; i <= (int) Random.Range(1, 3); i++) {            
        //    Instantiate(impactDebris, new Vector3(transform.position.x + Random.RandomRange(-1, 1), transform.position.y, transform.position.z), transform.rotation);
        //   }


        Explode();
        Destroy(gameObject);        
    }
    void Explode()
    {

        //creates a layermask which makes the raycast only hit the floor, obstacles or the player.
        int plyr = 1 << LayerMask.NameToLayer("Player");
        int floor = 1 << LayerMask.NameToLayer("Floor");
        int cover = 1 << LayerMask.NameToLayer("Cover");
        int mask = plyr | floor | cover;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.transform.position, mask); //creates layermask which ignores NPCs layer, enemies can see through enemies.
        Debug.Log(hit.transform.tag);
        if(hit.transform.tag == "Player" && hit.distance <= _explosionRadius)
        {
            //player isnt behind cover
            //apply damage to player based on distance from explosion.
            //bounds for dmg multiplier, [0.5*, _maxDamageMultiplier]
            //no need to calculate lower bound, as it occurs when hit.distance == _explosionRadius, this is only called if within radius. 

            float dmgMultiplier = _explosionRadius /(2*hit.distance);
            if (dmgMultiplier > _maxDamageMultiplier)
            {
                dmgMultiplier = _maxDamageMultiplier;
            }
            float damage = _damage * dmgMultiplier;
                
            Debug.Log("Player was hit, apply damge: " + damage);
        }
    }
}
