using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    #region Parameters

    private WorldHandler.BlockType blockType;

    #endregion

    #region Getters/Setters

    public WorldHandler.BlockType GetBlockType() {
        // GET BLOCK TYPE
        return blockType;
    }

    public void SetBlockType(WorldHandler.BlockType type) {
        // SET BLOCK TYPE
        blockType = type;

        // SET BLOCK COLOR
        switch (blockType) {
            case WorldHandler.BlockType.Stone:
                GetComponent<Renderer>().material.color = Color.gray;
                break;
            case WorldHandler.BlockType.Dirt:
                GetComponent<Renderer>().material.color = Color.green;
                break;
        }
    }

    #endregion
}