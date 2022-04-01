using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadHandMovement : MonoBehaviour
{
    public GameObject XROriginGameObject;
    public GameObject HandLeftGameObject;
    public GameObject HandRightGameObject;

    private GameObject HeldBallGameObject;
    private bool isBallInLeftHand;
    private float lastBallReleaseTime;

    private const float MAX_HAND_DISTANCE = 0.5f;
    private readonly Vector3 INITIAL_HAND_POSITION_LEFT = new Vector3(
        -0.25f, 
        0.3f, 
        0.0f
    );
    private readonly Vector3 INITIAL_HAND_POSITION_RIGHT = new Vector3(
        0.25f, 
        0.3f, 
        0.0f
    );
    private const float MAX_HAND_DISTANCE_PER_SECOND = 0.5f;
    private const float MIN_THROW_ANGLE_DEGREES = 30.0f;
    private const float MAX_THROW_ANGLE_DEGREES = 60.0f;
    private const float BALL_RELEASE_TIMEOUT_SECONDS = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        HandLeftGameObject.transform.localPosition = INITIAL_HAND_POSITION_LEFT;
        HandRightGameObject.transform.localPosition = INITIAL_HAND_POSITION_RIGHT;
    }

    // Update is called once per frame
    void Update()
    {
        if (HeldBallGameObject != null) {
            if (HandsAreAtInitialPositions()) {
                TossBallToPlayer();
            } else {
                Debug.Log("moving hadns towards initial positions");
                MoveHandsTowardsInitialPositions();
                if (isBallInLeftHand) {
                    HeldBallGameObject.transform.position = HandLeftGameObject.transform.position;
                } else {
                    HeldBallGameObject.transform.position = HandRightGameObject.transform.position;
                }
            }
        }
    }

    public void OnHandCollision(GameObject ballObject, bool isLeftHand) {
        Debug.Log("Ball collided with dad hand");
        if (Time.time - lastBallReleaseTime > BALL_RELEASE_TIMEOUT_SECONDS) {
            HeldBallGameObject = ballObject;
            isBallInLeftHand = isLeftHand;
            ballObject.GetComponent<BallGrabHandler>().OnDadGrab();
        }
    }

    private void TossBallToPlayer() {
        Debug.Log("Tossing ball to player");
        HeldBallGameObject.GetComponent<BallGrabHandler>().OnDadGrabRelease();
        HeldBallGameObject.GetComponent<Rigidbody>().velocity = CalculateThrowVector();
        HeldBallGameObject = null;
        lastBallReleaseTime = Time.time;
    }

    private Vector3 CalculateThrowVector() {  
        Vector3 playerPosition = XROriginGameObject.transform.position;
        Vector3 ballPosition = HeldBallGameObject.transform.position;
        Vector3 horizontalPlayerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
        Vector3 horizontalBallPosition = new Vector3(ballPosition.x, 0, ballPosition.z);
 
        float horizontalDistance = Vector3.Distance(horizontalPlayerPosition, horizontalBallPosition);
        float heightDifference = ballPosition.y - playerPosition.y;
 
        float throwAngle = Random.Range(MIN_THROW_ANGLE_DEGREES, MAX_THROW_ANGLE_DEGREES) * Mathf.Deg2Rad;

        float velocityMagnitude = (
            (1 / Mathf.Cos(throwAngle))
            * Mathf.Sqrt(
                (0.5f *  Physics.gravity.magnitude * Mathf.Pow(horizontalDistance, 2))
                / (horizontalDistance * Mathf.Tan(throwAngle) + heightDifference)
            )
        );
        Vector3 velocityUnangled = new Vector3(
            0,
            velocityMagnitude * Mathf.Sin(throwAngle),
            velocityMagnitude * Mathf.Cos(throwAngle)
        );
 
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, horizontalPlayerPosition - horizontalBallPosition);
        return Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocityUnangled;
    }

    private bool HandsAreAtInitialPositions() {
        return (
            Mathf.Approximately(
                0.0f,
                Vector3.Distance(
                    HandLeftGameObject.transform.localPosition,
                    INITIAL_HAND_POSITION_LEFT
                )
            )
            && Mathf.Approximately(
                0.0f,
                Vector3.Distance(
                    HandRightGameObject.transform.localPosition,
                    INITIAL_HAND_POSITION_RIGHT
                )
            )
        );
    }

    private void MoveHandsTowardsInitialPositions() {
        float maxHandDistance = MAX_HAND_DISTANCE_PER_SECOND * Time.deltaTime;
        HandLeftGameObject.transform.localPosition = Vector3.MoveTowards(
            HandLeftGameObject.transform.localPosition,
            INITIAL_HAND_POSITION_LEFT,
            maxHandDistance
        );
        HandRightGameObject.transform.localPosition = Vector3.MoveTowards(
            HandRightGameObject.transform.localPosition,
            INITIAL_HAND_POSITION_RIGHT,
            maxHandDistance
        );
    }
}
