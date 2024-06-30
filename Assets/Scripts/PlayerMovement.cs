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

    #endregion

    #region Rotation

    private void UpdateMovementDirection() {
        // GET CAMERA ROTATION
        Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        // ROTATE PLAYER MOVEMENT DIRECTION
        currDirectionalMovementVector = cameraRotation * currInputVector;
    }

    #endregion

    #region Input

    public void OnPlayerMovement(InputValue value) {
        // SET CURRENT INPUT VECTOR
        currInputVector = value.Get<Vector3>();
    }

    #endregion
}
