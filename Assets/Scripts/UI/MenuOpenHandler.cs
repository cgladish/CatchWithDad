using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class MenuOpenHandler : MonoBehaviour
{
    public GameObject LeftHandControllerGameObject;
    public GameObject LeftHandUIControllerGameObject;
    public GameObject RightHandControllerGameObject;
    public GameObject RightHandUIControllerGameObject;
    public GameObject MenuCanvasGameObject;
    public GameObject XROriginGameObject;
    public GameObject MainCameraGameObject;
    public InputActionReference LeftHandToggleMenuInputActionReference;
    public InputActionReference RightHandToggleMenuInputActionReference;
    
    private Vector3 m_PreviousXROriginPosition;

    private const float MENU_DISTANCE_FROM_PLAYER = 2.0f;
    private const float MENU_HEIGHT_OFFSET = 1.2f;

    Boolean isMenuOpen = false;

    void OnEnable() {
        LeftHandToggleMenuInputActionReference.action.performed += onToggleMenu;
        RightHandToggleMenuInputActionReference.action.performed += onToggleMenu;
    }

    void OnDisable() {
        LeftHandToggleMenuInputActionReference.action.performed -= onToggleMenu;
        RightHandToggleMenuInputActionReference.action.performed -= onToggleMenu;
    }

    void Start() {
        setMenuState(false);
    }

    void Update() {
        MenuCanvasGameObject.transform.position += (XROriginGameObject.transform.position - m_PreviousXROriginPosition);
        m_PreviousXROriginPosition = XROriginGameObject.transform.position;
    }

    void onToggleMenu(InputAction.CallbackContext callbackContext) {
        setMenuState(!isMenuOpen);
    }

    private void setMenuState(Boolean newIsMenuOpen) {
        isMenuOpen = newIsMenuOpen;

        LeftHandControllerGameObject.GetComponent<XRDirectInteractor>().enabled = !newIsMenuOpen;
        RightHandControllerGameObject.GetComponent<XRDirectInteractor>().enabled = !newIsMenuOpen;
        LeftHandUIControllerGameObject.GetComponent<XRInteractorLineVisual>().enabled = newIsMenuOpen;
        LeftHandUIControllerGameObject.GetComponent<XRRayInteractor>().enabled = newIsMenuOpen;
        RightHandUIControllerGameObject.GetComponent<XRInteractorLineVisual>().enabled = newIsMenuOpen;
        RightHandUIControllerGameObject.GetComponent<XRRayInteractor>().enabled = newIsMenuOpen;

        MenuCanvasGameObject.SetActive(newIsMenuOpen);
        MenuCanvasGameObject.transform.position = GetMenuPosition();
        MenuCanvasGameObject.transform.rotation = GetMenuRotation();
    }

    private Vector3 GetMenuPosition() {
        Vector3 mainCameraHorizontalDirection = Vector3.Normalize(new Vector3(
            MainCameraGameObject.transform.forward.x,
            0,
            MainCameraGameObject.transform.forward.z
        ));
        Vector3 menuHorizontalPosition = XROriginGameObject.transform.position + mainCameraHorizontalDirection * MENU_DISTANCE_FROM_PLAYER;
        return new Vector3(menuHorizontalPosition.x, menuHorizontalPosition.y + MENU_HEIGHT_OFFSET, menuHorizontalPosition.z);
    }

    private Quaternion GetMenuRotation() {
        return Quaternion.Euler(
            0,
            MainCameraGameObject.transform.rotation.eulerAngles.y,
            0
        );
    }
}
