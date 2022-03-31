using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLeftCollisionHandler : MonoBehaviour
{
    public GameObject DadBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == Constants.TAG_BALL) {
            DadBase.GetComponent<DadHandMovement>().OnHandCollision(collider.gameObject, true);
        }
    }
}
