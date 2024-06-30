using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    
    #region Parameters
    private float movementSpeed = 5.0f;

    #endregion

    #region Variables

    private CharacterController controller;

    private Vector3 currInputVector;

    private Vector3 currDirectionalMovementVector;

    #endregion

    #region Setup

    void Start() {
        // GET CHARACTER CONTROLLER
        controller = gameObject.GetComponent<CharacterController>();
    }

    #endregion

    #region Movement

    void Update() {
        // UPDATE CURRENT MOVEMENT VECTOR DIRECTION
        UpdateMovementDirection();

        // MOVE EVERY FRAME
        Move();
    }

    private void Move() {
        // MOVE PLAYER BY CURRENT INPUT VECTOR
        controller.Move(currDirectionalMovementVector * movementSpeed * Time.deltaTime);
    }

    private void UpdateMovementDirection() {
        // SET CURRENT DIRECTIONAL MOVEMENT VECTOR TO FACE IN CAMERA DIRECTION
        currDirectionalMovementVector = Camera.main.transform.forward * currInputVector.z + Camera.main.transform.right * currInputVector.x;
    }

    #endregion

    #region Input

    public void OnPlayerMovement(InputValue value) {
        // SET CURRENT INPUT VECTOR
        currInputVector = value.Get<Vector3>();
    }

    #endregion
}
