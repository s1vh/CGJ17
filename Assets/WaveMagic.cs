using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMagic : MonoBehaviour {

    [SerializeField]
    private float force,radius;
    // Use this for initialization
	void Start () {
        Destroy(gameObject, 2f);
        Invoke("DoOverlap", 0.3f);
    }
     void DoOverlap()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D colly in objectsInRange)
        {
            if (colly.gameObject.tag == "Nenúfar")
            {
                Debug.Log("Hitted" + colly.gameObject.name);
                colly.gameObject.GetComponent<Rigidbody2D>().AddForce((colly.gameObject.transform.position - transform.position).normalized * force);
            }
        }
    }
    [ExecuteInEditMode]
    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + radius, transform.position.y, transform.position.z));
    }
}
