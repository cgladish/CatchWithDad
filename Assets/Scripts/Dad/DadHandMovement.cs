using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadHandMovement : MonoBehaviour
{
    public GameObject HandLeftGameObject;
    public GameObject HandRightGameObject;

    private GameObject HeldBallGameObject;
    private bool isBallInLeftHand;

    private const float MAX_HAND_DISTANCE = 0.5f;
    private const float INITIAL_HAND_POSITION_X = 0.25f;
    private const float INITIAL_HAND_POSITION_Y = 0.3f;
    private const float INITIAL_HAND_POSITION_Z = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        ResetHandPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (HeldBallGameObject != null) {
            if (isBallInLeftHand) {
                HeldBallGameObject.transform.position = HandLeftGameObject.transform.position;
            } else {
                HeldBallGameObject.transform.position = HandRightGameObject.transform.position;
            }
        }
    }

    private void ResetHandPositions() {
        HandLeftGameObject.transform.localPosition = new Vector3(
            -INITIAL_HAND_POSITION_X,
            INITIAL_HAND_POSITION_Y,
            INITIAL_HAND_POSITION_Z
        );
        HandRightGameObject.transform.localPosition = new Vector3(
            INITIAL_HAND_POSITION_X,
            INITIAL_HAND_POSITION_Y,
            INITIAL_HAND_POSITION_Z
        );

    }

    public void OnHandCollision(GameObject ballObject, bool isLeftHand) {
        HeldBallGameObject = ballObject;
        isBallInLeftHand = isLeftHand;
        ballObject.GetComponent<BallGrabHandler>().OnDadGrab();
    }
}
