using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArctoMouse : MonoBehaviour
{
    private LineRenderer lineRend;
    private Vector2 start;
    private Vector2 mousePos;
    private float offset;
    private float height;
    [SerializeField] private float points;
    [SerializeField] private bool medium;
    [SerializeField] private bool heavy;
    void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (medium)
        {
            height = 1;
            offset = 2;
        }
        else if (heavy)
        {
            height = 2;
            offset = 4;
        }
        else
        {
            height = 0;
            offset = 0;
        }
        start = transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRend.positionCount = (int) points;
        lineRend.enabled = true;

        for (float i = 0; i < points; i++)
        {
            lineRend.SetPosition((int) i,Parabola(start, mousePos, height, i / points));
        }
    }
    void OnDrawGizmos()
    {
        //Draw the parabola by sample a few times
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, mousePos);
        float count = 20;
        Vector2 lastP = start;
        for (float i = 0; i < count + 1; i++)
        {
            Vector2 p = Parabola(start, mousePos, height, i / count);
            Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
            Gizmos.DrawLine(lastP, p);
            lastP = p;
        }
    }

    Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        //end.y = end.y - offset;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector2 travelVector = end - start;
            Vector2 result = start + t * travelVector;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector2 travelVector = end - start;
            Vector2 levelDirection = end - new Vector2(start.x, end.y);
            Vector3 right = Vector3.Cross(travelVector, levelDirection);
            Vector3 up = Vector3.Cross(right, travelVector);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelVector;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }
}
