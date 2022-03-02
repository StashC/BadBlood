using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicState : MonoBehaviour
{ 
    public AK.Wwise.State OnTriggerEnterState;
    private void OnTriggerEnter2D(Collider2D collision)

    {
        if(collision.CompareTag("Player") )
        {
            OnTriggerEnterState.SetValue();

        }

    }
}
