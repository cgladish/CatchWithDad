using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSync : MonoBehaviour
{
    public Transform AvatarHandLeft;
    public Transform AvatarHandRight;
    public Transform XRControllerLeft;
    public Transform XRControllerRight;  

    // Update is called once per frame
    void Update()
    {
        SyncPositionAndRotation(AvatarHandLeft, XRControllerLeft);
        SyncPositionAndRotation(AvatarHandRight, XRControllerRight);
    }

    private void SyncPositionAndRotation(Transform AvatarTransform, Transform XRTransform)
    {
        AvatarTransform.position = XRTransform.position;
        AvatarTransform.rotation = XRTransform.rotation;
    }
}
