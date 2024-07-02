using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlockInteraction : MonoBehaviour {

    #region Parameters

    [SerializeField] private GameObject blockPrefab;

    [SerializeField] private LayerMask blockLayerMask;

    [SerializeField] private InventoryHandler inventoryHandler;

    private float blockWidth = 1.0f;

    private float maxDistance = 10f;

    private float minPlaceDistance = 1.5f;

    #endregion

    #region Variables

    private GameObject currTargetBlock = null;

    private Vector3 currTargetFaceNormal = Vector3.zero;

    private bool currBlockPlaceable = false;

    private WorldHandler.BlockType currBlockType = WorldHandler.BlockType.Dirt;

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
        // CHECK IF TARGET BLOCK EXISTS AND IS WITHIN WORLD BOUNDS
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

        WorldHandler.instance.PlaceBlock(currTargetBlock.transform.position + currTargetFaceNormal * blockWidth, currBlockType);
    }

    #endregion

    #region ChangeBlockType

    public void OnSelectBlockType(InputValue value) {
        
        // RETRIEVE NEW BLOCK TYPE VALUE
        int blockType = (int)value.Get<float>() - 1;

        // CHECK IF BLOCK TYPE IS VALID
        if (blockType < 0 || blockType >= 9) return;

        // SET CURRENT BLOCK TYPE
        currBlockType = (WorldHandler.BlockType)(blockType);

        // DISPLAY SELECTED BLOCK
        inventoryHandler.SelectBlock(blockType);
    }

    #endregion

}
