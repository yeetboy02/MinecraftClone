using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour {

    #region Parameters

    private Vector2 rotationSpeed = new Vector2(2.0f, 2.0f);

    #endregion

    #region Variables

    private Vector2 currInputVector;

    private Quaternion currRotation;

    #endregion

    #region Setup

    void Start() {
        // GET CURRENT ROTATION
        currRotation = transform.rotation;
    }

    #endregion

    #region Rotation

    void Update() {
        // ROTATE ON EVERY FRAME
        Rotate();
    }

    private void Rotate() {
        // ROTATE CAMERA
        currRotation.x -= currInputVector.y * rotationSpeed.x;
        currRotation.y += currInputVector.x * rotationSpeed.y;

        // APPLY ROTATION
        transform.rotation = Quaternion.Euler(currRotation.x, currRotation.y, currRotation.z);
    }

    private void OnCameraMovement(InputValue value) {
        // SET CURRENT INPUT VECTOR
        currInputVector = value.Get<Vector2>();
    }

    #endregion
}