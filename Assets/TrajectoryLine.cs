using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrajectoryLine : MonoBehaviour
{
    private LineRenderer lineRend;
    private Vector2 mousePos;
    [SerializeField] private bool isOn;
    void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetMousePosition(Vector2 position)
    {
        mousePos = position;
    }
    // Update is called once per frame
    void Update()
    {
        lineRend.SetPosition(0, transform.position);
        Vector2 worldmousePos = Camera.main.ScreenToWorldPoint(mousePos);
        lineRend.SetPosition(1, new Vector3(worldmousePos.x, worldmousePos.y, 0f));

        lineRend.enabled = isOn;
    }

    public void setIsOn(bool check)
    {
        isOn = check;
    }
}
