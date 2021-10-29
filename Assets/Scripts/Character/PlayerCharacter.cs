using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private CharacterMovement _characterMovement;


    //UNITY CALLS _____________________________________________________
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    //MOVEMENT __________________________________________________
    public void Move(Vector2 moveInput)
    {
        _characterMovement.UpdateMoveInput(moveInput);
    }
    public void Jump()
    {
        _characterMovement.Jump();
    }

    //PICKUP AND THROW __________________________________________

    //HEALING ____________________________________________________

}
