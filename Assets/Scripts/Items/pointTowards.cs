using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointTowards : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float offset;
   
    // Update is called once per frame
    void Update()
    {
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotation;
    }
}
