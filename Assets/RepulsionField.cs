using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsionField : MonoBehaviour {
    //[SerializeField]
    //private float repulsionforce;
    Persona persona;
    Rigidbody2D personaRigidbody;

    // Use this for initialization
    void Start ()
    {
        persona = GetComponentInParent<Persona>();
        personaRigidbody = GetComponentInParent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if(persona.leader == true)
            {
                persona.leader = false;
                Debug.Log("Leadership cancelled");
            }
            personaRigidbody.AddForce((transform.position-col.transform.position) * persona.enmity * 10, ForceMode2D.Impulse);
        }
    }
}
