// This script is to help with dev testing by letting you reset the ball
// with pressing the primary controller button

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ResetBall : MonoBehaviour
{
    public InputActionReference ResetBallLeftInputActionReference;
    public InputActionReference ResetBallRightInputActionReference;
    private Rigidbody m_Rigidbody;

    private void OnEnable()
    {
        ResetBallLeftInputActionReference.action.performed += OnResetBall;
        ResetBallRightInputActionReference.action.performed += OnResetBall;
    }

    private void OnDisable()
    {
        ResetBallLeftInputActionReference.action.performed -= OnResetBall;
        ResetBallRightInputActionReference.action.performed -= OnResetBall;
    }

    void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnResetBall(InputAction.CallbackContext callbackContext) {
        if (GlobalSettings.Instance.IsDevModeEnabled) {
            gameObject.transform.position = Vector3.zero;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
