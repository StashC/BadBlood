using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


//Marco Cornejo, October 22nd 2021
public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerCharacter _playerCharacter;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent<Vector2> _cursorPositionEvent;
    [SerializeField] private UnityEvent _leftClickEvent;

    // PLAYER INPUT CALLS ___________________________________
    private void OnMove(InputValue actionInput)
    {
        Vector2 input = actionInput.Get<Vector2>();

        _playerCharacter.Move(input);
    }

    private void OnJump()
    {
        _playerCharacter.Jump();

    }

    private void OnLeftClick()
    {
        _leftClickEvent.Invoke();
    }
    private void OnCursor(InputValue actionInput)
    {
        Vector2 cursorPosition = actionInput.Get<Vector2>();
        _cursorPositionEvent.Invoke(cursorPosition);
    }
}
