using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Music_Play", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
