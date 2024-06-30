using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockInteraction : MonoBehaviour {

    #region Parameters

    [SerializeField] private GameObject blockPrefab;

    [SerializeField] private LayerMask blockLayerMask;

    private float blockWidth = 1.0f;

    private float maxDistance = 10f;

    private float minPlaceDistance = 1.5f;

    #endregion

    #region Variables

    private GameObject currTargetBlock = null;

    private Vector3 currTargetFaceNormal = Vector3.zero;

    private bool currBlockPlaceable = false;

    #endregion

    void Update() {
        // GET CURRENT TARGET BLOCK
        UpdateCurrTarget();
    }

    #region TargetBlock

    private void UpdateCurrTarget() {
        RaycastHit hit;

        // GET PLAYER CAMERA
        Camera playerCamera = Camera.main;

        // CHECK RAYCAST
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance, blockLayerMask)) {
            // SET IF BLOCK CURRENTLY PLACABLE
            currBlockPlaceable = hit.distance < minPlaceDistance ? false : true;

            // SET CURRENT TARGET BLOCK
            currTargetBlock = hit.collider.gameObject;

            // SET CURRENT TARGET POSITION
            currTargetFaceNormal = hit.normal;
        }
        else {
            // RESET CURRENT TARGET BLOCK
            currTargetBlock = null;

            // RESET CURRENT TARGET POSITION
            currTargetFaceNormal = Vector3.zero;

            // RESET CURRENT BLOCK PLACABLE
            currBlockPlaceable = false;
        }
    }

    #endregion

    #region BreakBlock

    public void OnBreakBlock() {
        BreakBlock();
    }

    private void BreakBlock() {
        // CHECK IF TARGET BLOCK EXISTS
        if (currTargetBlock == null) return;

        WorldHandler.instance.BreakBlock(currTargetBlock);
    }

    #endregion


    #region PlaceBlock

    public void OnPlaceBlock() {
        PlaceBlock();
    }

    private void PlaceBlock() {
        // CHECK IF CURRENTLY A BLOCK PLACABLE
        if (!currBlockPlaceable) return;

        WorldHandler.instance.PlaceBlock(currTargetBlock.transform.position + currTargetFaceNormal * blockWidth);
    }

    #endregion

}
