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

    private const float MAX_HAND_DISTANCE = 1.5f;
    private readonly Vector3 INITIAL_HAND_POSITION_LEFT = new Vector3(
        -0.5f, 
        0.0f, 
        0.5f
    );
    private readonly Vector3 INITIAL_HAND_POSITION_RIGHT = new Vector3(
        0.5f, 
        0.0f, 
        0.5f
    );
    private const float MAX_HAND_DISTANCE_PER_SECOND = 2.0f;
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
        if (BallGameObject.GetComponent<BallGrabHandler>().inFlightAfterPlayerThrow) {
            Vector3? ballDestination = CalculateBallDestination();
            if (ballDestination.HasValue) {
                float distanceFromLeftHandToDestination = Vector3.Distance(
                    ballDestination.Value,
                    HandLeftGameObject.transform.position
                );
                float distanceFromRightHandToDestination = Vector3.Distance(
                    ballDestination.Value,
                    HandRightGameObject.transform.position
                );
                if (distanceFromLeftHandToDestination < distanceFromRightHandToDestination) {
                    if (distanceFromLeftHandToDestination <= MAX_HAND_DISTANCE) {
                        MoveLeftHandTowardsPosition(ballDestination.Value);
                    }
                } else {
                    if (distanceFromRightHandToDestination <= MAX_HAND_DISTANCE) {
                        MoveRightHandTowardsPosition(ballDestination.Value);
                    }
                }
            }
        } else {
            MoveHandsTowardsInitialPositions();
            if (isHoldingBall) {
                if (HandsAreAtInitialPositions()) {
                    TossBallToPlayer();
                } else {
                    if (isBallInLeftHand) {
                        BallGameObject.transform.position = HandLeftGameObject.transform.position;
                    } else {
                        BallGameObject.transform.position = HandRightGameObject.transform.position;
                    }
                }
            }
        }
    }

    public void OnHandCollision(GameObject ballObject, bool isLeftHand) {
        if (Time.time - lastBallReleaseTime > BALL_RELEASE_TIMEOUT_SECONDS) {
            isHoldingBall = true;
            isBallInLeftHand = isLeftHand;
            ballObject.GetComponent<BallGrabHandler>().OnDadGrab();
        }
    }

    private void TossBallToPlayer() {
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
        float verticalToHorizontalVelocityRatio = Random.Range(0.7f, 1.3f);

        float horizontalVelocityMagnitude = horizontalDistance * Mathf.Sqrt(
            Physics.gravity.y
            / (2 * (heightDifference - verticalToHorizontalVelocityRatio * horizontalDistance))
        );
        Vector3 horizontalThrowDirection = (horizontalPlayerPosition - horizontalBallPosition).normalized;

        return new Vector3(
            horizontalThrowDirection.x,
            verticalToHorizontalVelocityRatio,
            horizontalThrowDirection.z
        ) * horizontalVelocityMagnitude;
    }

    private Vector3? CalculateBallDestination() {
        Vector3 ballPosition = BallGameObject.transform.position;
        Vector3 ballVelocity = BallGameObject.GetComponent<Rigidbody>().velocity;

        // Calculate intersection point of horizontal direction and catch plane
        Vector3 ballVelocityHorizontal = new Vector3(ballVelocity.x, 0, ballVelocity.z);
        Plane catchPlane = new Plane(
            gameObject.transform.forward,
            gameObject.transform.TransformPoint(INITIAL_HAND_POSITION_LEFT)
        );
        Ray ballRay = new Ray(ballPosition, ballVelocityHorizontal);
        float intersectionDistance;
        Vector3 ballDestinationHorizontal;
        if (catchPlane.Raycast(ballRay, out intersectionDistance)) {
            ballDestinationHorizontal = ballRay.GetPoint(intersectionDistance);
        } else {
            return null;
        }

        // Parabola equation
        float timeToDestination = intersectionDistance / ballVelocityHorizontal.magnitude;
        float endHeight = (
            ballPosition.y
            + (ballVelocity.y * timeToDestination)
            + (Physics.gravity.y * Mathf.Pow(timeToDestination, 2) / 2)
        );

        return new Vector3(
            ballDestinationHorizontal.x,
            endHeight,
            ballDestinationHorizontal.z
        );
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

    private void MoveLeftHandTowardsPosition(Vector3 position) {
        float maxHandDistance = MAX_HAND_DISTANCE_PER_SECOND * Time.deltaTime;
        HandLeftGameObject.transform.position = Vector3.MoveTowards(
            HandLeftGameObject.transform.position,
            position,
            maxHandDistance
        );
        HandRightGameObject.transform.localPosition = Vector3.MoveTowards(
            HandRightGameObject.transform.localPosition,
            INITIAL_HAND_POSITION_RIGHT,
            maxHandDistance
        );
    }

    private void MoveRightHandTowardsPosition(Vector3 position) {
        float maxHandDistance = MAX_HAND_DISTANCE_PER_SECOND * Time.deltaTime;
        HandLeftGameObject.transform.localPosition = Vector3.MoveTowards(
            HandLeftGameObject.transform.localPosition,
            INITIAL_HAND_POSITION_LEFT,
            maxHandDistance
        );
        HandRightGameObject.transform.position = Vector3.MoveTowards(
            HandRightGameObject.transform.position,
            position,
            maxHandDistance
        );
    }
}
