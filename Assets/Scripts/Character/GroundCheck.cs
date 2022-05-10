using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] private float _raycastRadius = 0.25f;
    [SerializeField] private LayerMask _floorLayer;

    [Header("Feedback")]
    [SerializeField] private bool _isGrounded;

    private void Update()
    {
        _isGrounded = CheckIsGrounded();
    }

    public bool GetIsGrounded()
    {
        return _isGrounded;
    }

    private bool CheckIsGrounded()
    {
        if (Physics2D.CircleCast(transform.position, _raycastRadius, Vector2.down, 0f,  _floorLayer)) return true;
        return false;
    }
}
