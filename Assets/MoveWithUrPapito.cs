using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithUrPapito : MonoBehaviour
{

    [SerializeField]
    private Transform WhoIsMyPapito;
    private Animator anim;
    [SerializeField]
    private float increasehighttoyourpapito;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    public void HazmeTuPapito(Transform newPapito)
    {
        WhoIsMyPapito = newPapito;
    }
    void Update()
    {
        transform.position = new Vector3(WhoIsMyPapito.position.x, WhoIsMyPapito.position.y + increasehighttoyourpapito);
    }
    public void RomperConmigoPorQUENOMEQUIERES()
    {
        anim.SetTrigger("SplitUp");
        Destroy(gameObject, 0.35f);
    }
}
