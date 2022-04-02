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
    public Dropdown DropdownThrowSmoothingCurve;
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

        switch (DropdownThrowSmoothingCurve.value) {
            case 0: // Constant
                BallXRGrabInteractable.throwSmoothingCurve = AnimationCurve.Constant(0, 1, 1);
                break;
            case 1: // Linear (Increasing)
                BallXRGrabInteractable.throwSmoothingCurve = AnimationCurve.Linear(0, 0, 1, 1);
                break;
            case 2: // Linear (Decreasing)
                BallXRGrabInteractable.throwSmoothingCurve = AnimationCurve.Linear(0, 1, 1, 0);
                break;
            case 3: // Exponential
                BallXRGrabInteractable.throwSmoothingCurve = new AnimationCurve();
                BallXRGrabInteractable.throwSmoothingCurve.AddKey(new Keyframe(0, 0, 0, 0));
                BallXRGrabInteractable.throwSmoothingCurve.AddKey(new Keyframe(1, 1, 1, 1));
                break;
            case 4: // Logarithmic
                BallXRGrabInteractable.throwSmoothingCurve = new AnimationCurve();
                BallXRGrabInteractable.throwSmoothingCurve.AddKey(new Keyframe(0, 0, 1, 1));
                BallXRGrabInteractable.throwSmoothingCurve.AddKey(new Keyframe(1, 1, 0, 0));
                break;
            default:
                Debug.LogError("Fell through to default for DropdownThrowSmoothingCurve.value");
                break;
        }
    }
}
