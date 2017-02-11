using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerController : MonoBehaviour {

    [SerializeField]
    private OtherTargetsController daddy;
	
    void Awake()
    {
        daddy = daddy.GetComponent<OtherTargetsController>();
    }
    public void TimeToImpulseMeBaby()
    {
        daddy.PulseToMove();
    }
}
