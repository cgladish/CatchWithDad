using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadHandMovement : MonoBehaviour
{
    public GameObject HandLeft;
    public GameObject HandRight;
    public GameObject Baseball;
    private const float MAX_HAND_DISTANCE = 0.5f;
    private const float INITIAL_HAND_POSITION_X = 0.25f;
    private const float INITIAL_HAND_POSITION_Y = 0.5f;
    private const float INITIAL_HAND_POSITION_Z = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        ResetHandPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetHandPositions() {
        HandLeft.transform.localPosition = new Vector3(
            -INITIAL_HAND_POSITION_X,
            INITIAL_HAND_POSITION_Y,
            INITIAL_HAND_POSITION_Z
        );
        HandRight.transform.localPosition = new Vector3(
            INITIAL_HAND_POSITION_X,
            INITIAL_HAND_POSITION_Y,
            INITIAL_HAND_POSITION_Z
        );

    }
}
