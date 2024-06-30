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

    #endregion

    #region Setup

    void Start() {
        // GET CHARACTER CONTROLLER
        controller = gameObject.GetComponent<CharacterController>();
    }

    #endregion

    #region Movement

    void Update() {
        // MOVE EVERY FRAME
        Move();
    }

    private void Move() {
        // MOVE PLAYER BY CURRENT INPUT VECTOR
        controller.Move(currInputVector * movementSpeed * Time.deltaTime);
    }

    #endregion

    #region Input

    public void OnPlayerMovement(InputValue value) {
        // SET CURRENT INPUT VECTOR
        currInputVector = value.Get<Vector3>();
    }

    #endregion
}
