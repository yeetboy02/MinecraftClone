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
        // INSTANTIATE BLOCK
        GameObject currBlock = Instantiate(blockPrefab, position, Quaternion.identity);

        // ADD BLOCK TO WORLD BLOCK LIST
        currWorld[(int)position.x, (int)position.y, (int)position.z] = currBlock;
    }

    public void BreakBlock(GameObject block) {
        
        // GET BLOCK POSITION
        Vector3 blockPosition = block.transform.position;

        Debug.Log(blockPosition);

        // REMOVE BLOCK FROM WORLD BLOCK LIST
        currWorld[(int)blockPosition.x, (int)blockPosition.y, (int)blockPosition.z] = null;

        // DESTROY BLOCK
        Destroy(block);
    }


    #endregion

}
