using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointTowards : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector2 _cursorPosition;
    [SerializeField] private float offset = 0;
    // Update is called once per frame
    void Update()
    {
        Vector2 targetPosition = target == null ? Camera.main.ScreenToWorldPoint(_cursorPosition) : target.transform.position;
        Vector2 direction = targetPosition - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotation;
    }

    public void SetCursorPosition(Vector2 cursorPosition)
    {
        _cursorPosition = cursorPosition;
    }
}
