using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Interaction_System : MonoBehaviour
{

    //Detection position
    public Transform DetectPosition;
    //Raycast Max distance
    public float raycastDis;
    //Detection Radius
    private const float DetectRadius = 0.25f;
    //Detection Layer
    public LayerMask DetectionLayer;
    //Cashed Trigger 
    public GameObject DectecedObject;
    //List of a item which picked up
    public List<GameObject> pickedObjects = new List<GameObject>();
    //List of items nearby
    public List<GameObject> nearbyObjects = new List<GameObject>();
    //Mouse raycast
    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    // Update is called once per frame
    void Update()
    {
        
        if (DetectObject())
        {
            if (InteractInput())
            {
                DectecedObject.GetComponent<Item>().Interact();
            }
        }
        
    }

    bool InteractInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
            return true;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {

            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitted = Physics2D.Raycast(mousepos, Vector2.zero, raycastDis, DetectionLayer);
            if (hitted)
                return true;
            else
                return false;
        }
        return false;
    }

    bool DetectObject()
    {
        
        Collider2D obj = Physics2D.OverlapCircle(DetectPosition.position, DetectRadius,DetectionLayer);
        if (obj == null)
        {
            DectecedObject = null;
            return false;
        }

        else
        {
            DectecedObject = obj.gameObject;
            return true;
        }

    }
     public void PickUp(GameObject obj)
    {
        if (pickedObjects.Count != 0)
        {
            DropOff();
        }
        pickedObjects.Add(obj);
    }
    // Check if pickedObject > 1
    //change the position of the item and set active
    public void DropOff()
    {
        for (int i = 0; i < pickedObjects.Count; i++)
        {
            pickedObjects[i].transform.position = new Vector2(DetectPosition.position.x, DetectPosition.position.y);
            pickedObjects[i].SetActive(true);
        }
        pickedObjects.Clear();
    }


}
