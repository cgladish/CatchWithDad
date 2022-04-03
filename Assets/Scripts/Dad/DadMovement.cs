using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadMovement : MonoBehaviour
{
    public GameObject XROriginGameObject;
    public GameObject HandLeftGameObject;
    public GameObject HandRightGameObject;
    public GameObject BallGameObject;

    private bool isHoldingBall;
    private bool isBallInLeftHand;
    private float lastBallReleaseTime;

    private const float MAX_HAND_DISTANCE = 0.5f;
    private readonly Vector3 INITIAL_HAND_POSITION_LEFT = new Vector3(
        -0.25f, 
        0.0f, 
        0.25f
    );
    private readonly Vector3 INITIAL_HAND_POSITION_RIGHT = new Vector3(
        0.25f, 
        0.0f, 
        0.25f
    );
    private const float MAX_HAND_DISTANCE_PER_SECOND = 0.5f;
    private const float MIN_THROW_ANGLE_DEGREES = 30.0f;
    private const float MAX_THROW_ANGLE_DEGREES = 60.0f;
    private const float BALL_RELEASE_TIMEOUT_SECONDS = 1.0f;
    private const float DAD_MOVE_SPEED = 2.0f;
    private const float DAD_ROTATE_SPEED = 30f;

    // Start is called before the first frame update
    void Start()
    {
        HandLeftGameObject.transform.localPosition = INITIAL_HAND_POSITION_LEFT;
        HandRightGameObject.transform.localPosition = INITIAL_HAND_POSITION_RIGHT;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionFromDadToPlayer = (XROriginGameObject.transform.position - gameObject.transform.position).normalized;
        gameObject.transform.rotation = Quaternion.RotateTowards(
            gameObject.transform.rotation,
            Quaternion.LookRotation(new Vector3(directionFromDadToPlayer.x, 0, directionFromDadToPlayer.z)),
            DAD_ROTATE_SPEED * Time.deltaTime
        );
        if (isHoldingBall) {
            if (HandsAreAtInitialPositions()) {
                TossBallToPlayer();
            } else {
                Debug.Log("moving hands towards initial positions");
                MoveHandsTowardsInitialPositions();
                if (isBallInLeftHand) {
                    BallGameObject.transform.position = HandLeftGameObject.transform.position;
                } else {
                    BallGameObject.transform.position = HandRightGameObject.transform.position;
                }
            }
        } else {
            if (BallGameObject.GetComponent<BallGrabHandler>().inFlightAfterPlayerThrow) {
                Vector3? ballDestination = CalculateBallDestination();
                if (ballDestination.HasValue) {
                    gameObject.transform.position = Vector3.MoveTowards(
                        gameObject.transform.position,
                        ballDestination.Value,
                        Time.deltaTime * DAD_MOVE_SPEED
                    );
                }
            }
        }
    }

    public void OnHandCollision(GameObject ballObject, bool isLeftHand) {
        Debug.Log("Ball collided with dad hand");
        if (Time.time - lastBallReleaseTime > BALL_RELEASE_TIMEOUT_SECONDS) {
            isHoldingBall = true;
            isBallInLeftHand = isLeftHand;
            ballObject.GetComponent<BallGrabHandler>().OnDadGrab();
        }
    }

    private void TossBallToPlayer() {
        Debug.Log("Tossing ball to player");
        BallGameObject.GetComponent<BallGrabHandler>().OnDadGrabRelease();
        BallGameObject.GetComponent<Rigidbody>().velocity = CalculateThrowVector();
        isHoldingBall = false;
        lastBallReleaseTime = Time.time;
    }

    private Vector3 CalculateThrowVector() {  
        Vector3 playerPosition = XROriginGameObject.transform.position;
        Vector3 ballPosition = BallGameObject.transform.position;
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

    private Vector3? CalculateBallDestination() {
        Vector3 ballPosition = BallGameObject.transform.position;
        Vector3 ballVelocity = BallGameObject.GetComponent<Rigidbody>().velocity;

        float endHeight = 0; // CHANGE THIS TO DAD HAND HEIGHT
        float heightDifference = ballPosition.y - endHeight;
        float timeToDestination = ( // Quadratic equation time
            (
                -ballVelocity.y
                - Mathf.Sqrt(Mathf.Pow(ballVelocity.y, 2) - 2 * Physics.gravity.y * heightDifference)
            )
            / Physics.gravity.y
        );
        if (!float.IsNaN(timeToDestination)) {
            return new Vector3(
                ballPosition.x + (ballVelocity.x * timeToDestination),
                endHeight,
                ballPosition.z + (ballVelocity.z * timeToDestination)
            );
        } else {
            return null;
        }
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
