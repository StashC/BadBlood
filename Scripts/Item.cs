using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    //Raycast Max distance
    public float raycastDis;
    public LayerMask layermask;
    public enum InteractionType {NULL, PickUp, Examine };
    public InteractionType type;
    public Color itemColor;
    private RaycastHit2D PointtingItem;
    //private SpriteRenderer spriteRenderer;
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D enter = Physics2D.Raycast(mousepos, Vector2.zero, raycastDis,layermask);
        if (enter)
        {
            enter.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = itemColor;   // changing all objects color not tracking the mosue
        }
    }
    public void Interact()
    {
        switch(type)
        {
            case InteractionType.PickUp:
                GameObject item = gameObject;
                FindObjectOfType<Interaction_System>().PickUp(gameObject);
                gameObject.SetActive(false);
                GetComponent<SpriteRenderer>().color = itemColor;
                Debug.Log("Good Pickup");
                break;
            case InteractionType.Examine:
                Debug.Log("Good Examine");
                break;
            default:
                Debug.Log("NULL");
                break;
        }
    }


}
