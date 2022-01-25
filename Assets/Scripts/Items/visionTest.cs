using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visionTest : MonoBehaviour
{
    public Transform _player;
    [SerializeField] private float _sightRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool CanSeePlayer()
    {
        bool returnValue = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.position);
        
        //if we hit something detectable
        //Debug.Log(hit.distance);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player") && hit.distance <= _sightRange)
            {
                Debug.DrawLine(transform.position, _player.position, Color.green);
                returnValue = true;
            }  else if(!hit.collider.gameObject.CompareTag("Player") | hit.distance > _sightRange)
            {
                Debug.DrawLine(transform.position, _player.position, Color.red);
            }
        }
        return returnValue;
    }   


    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
    }
}
