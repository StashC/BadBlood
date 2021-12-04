using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour
{
    public LayerMask layermask;
    public float raycastDis;
    // Update is called once per frame
    private void Update()
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D enter = Physics2D.Raycast(mousepos, Vector2.zero, raycastDis, layermask);
        if (enter)
        {
            enter.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            //GetComponent<SpriteRenderer>().color = itemColor;   // changing all objects color not tracking the mosue
        }
    }
}
