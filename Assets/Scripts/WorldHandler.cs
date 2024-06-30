using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHandler : MonoBehaviour {

    #region Singleton

    public static WorldHandler instance;

    void Start() {
            if (instance != null)
                    return;
            instance = this;
            
            DontDestroyOnLoad(gameObject);

            // INITIALIZE WORLD ARRAY
            currWorld = new GameObject[(int)worldSize.x, (int)worldSize.y, (int)worldSize.z];
    }
    
    #endregion

    #region Parameters

    [SerializeField] private GameObject blockPrefab;

    private Vector3 worldSize = new Vector3(100, 50, 100);

    #endregion

    #region Variables

    GameObject[ , , ] currWorld;

    #endregion

    #region Methods

    public void PlaceBlock(Vector3 position) {
        // CHECK IF POSITION IS WITHIN WORLD BOUNDS
        if (position.x < 0 || position.x >= worldSize.x || position.y < 0 || position.y >= worldSize.y || position.z < 0 || position.z >= worldSize.z)
            return;

        // INSTANTIATE BLOCK
        GameObject currBlock = Instantiate(blockPrefab, position, Quaternion.identity);

        // ADD BLOCK TO WORLD BLOCK LIST
        currWorld[(int)position.x, (int)position.y, (int)position.z] = currBlock;
    }

    public void BreakBlock(GameObject block) {
        // GET BLOCK POSITION
        Vector3 blockPosition = block.transform.position;

        // REMOVE BLOCK FROM WORLD BLOCK LIST
        currWorld[(int)blockPosition.x, (int)blockPosition.y, (int)blockPosition.z] = null;

        // DESTROY BLOCK
        Destroy(block);
    }


    #endregion

}
