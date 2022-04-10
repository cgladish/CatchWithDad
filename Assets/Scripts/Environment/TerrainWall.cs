using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainWall : MonoBehaviour
{
    public GameObject XROriginGameObject;
    public GameObject BallGameObject;

    public const float WALL_RADIUS = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameObjectOutsideBoundaries(XROriginGameObject);
        HandleGameObjectOutsideBoundaries(BallGameObject);
    }

    private void HandleGameObjectOutsideBoundaries(GameObject gameObject) {
        Vector3 horizontalPosition = new Vector3(
            gameObject.transform.position.x,
            0,
            gameObject.transform.position.z
        );
        if (Vector3.Distance(horizontalPosition, Vector3.zero) > WALL_RADIUS) {
            Vector3 newHorizontalPosition = horizontalPosition.normalized * WALL_RADIUS;
            gameObject.transform.position = new Vector3(
                newHorizontalPosition.x,
                gameObject.transform.position.y,
                newHorizontalPosition.z
            );
        }
    }
}
