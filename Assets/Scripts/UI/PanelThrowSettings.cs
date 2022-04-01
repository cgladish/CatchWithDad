using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PanelThrowSettings : MonoBehaviour
{
    public Slider SliderThrowSmoothingDuration;
    public Text SliderThrowSmoothingDurationText;
    public Slider SliderThrowVelocityScale;
    public Text SliderThrowVelocityScaleText;
    public Slider SliderThrowAngularVelocityScale;
    public Text SliderThrowAngularVelocityScaleText;
    public XRGrabInteractable BallXRGrabInteractable;

    void Start()
    {
        SliderThrowSmoothingDuration.value = BallXRGrabInteractable.throwSmoothingDuration;    
        SliderThrowVelocityScale.value = BallXRGrabInteractable.throwVelocityScale;    
        SliderThrowAngularVelocityScale.value = BallXRGrabInteractable.throwAngularVelocityScale;        
    }

    // Update is called once per frame
    void Update()
    {
        BallXRGrabInteractable.throwSmoothingDuration = SliderThrowSmoothingDuration.value;        
        SliderThrowSmoothingDurationText.text = "Throw Smoothing Duration:" + SliderThrowSmoothingDuration.value.ToString();

        BallXRGrabInteractable.throwVelocityScale = SliderThrowVelocityScale.value;
        SliderThrowVelocityScaleText.text = "Throw Velocity Scale:" + SliderThrowVelocityScale.value.ToString();

        BallXRGrabInteractable.throwVelocityScale = SliderThrowAngularVelocityScale.value;
        SliderThrowAngularVelocityScaleText.text = "Throw Angular Velocity Scale:" + SliderThrowAngularVelocityScale.value.ToString();
    }
}
