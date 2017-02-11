using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherTargetsController : MonoBehaviour {
    [SerializeField]
    private Color standardColor, friendlyColor, friendColor;

    //Esto para la transformación progresiva de los ángulos y el color
    [SerializeField]
    private float angle, minToFollow, smothFollow = 1f;

    private Transform Player;
    private Rigidbody2D rigid;
    
    [SerializeField]
    private Transform nearestLeader;
    [SerializeField]
    private bool lookForALeader = true, followLeader, isOnRange;

    //For the couple
    private bool tieneChorva;

    public bool TieneChorva
    {
        get
        {
            return tieneChorva;
        }
    }

    [SerializeField]
    private float researchForChrova = 1.5f, goToYourChorva;
    //For the leader
    [SerializeField]
    private float anglePosibleToMove, leaderMove;
    private bool canMoveLeader = true, impulseTheFUCKINGLeader = true;

    [SerializeField]
    private SpriteRenderer render;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private bool canChange, changingToFriend, isFollowing, runOut;
    //Controlador de tiempo, el tiempo en el que te conviertes a to colega
    [SerializeField]
    private float timeToTurnInFriend, maxSpeed, followSpeed, initialRotation, currentSpeed, distanceToStopFollow, distanceToFollowLeader, distanceToMoveLeader;
    private float timeChangingToFriend, minEnmity, maxEnmity, actualEnemity, timeToStopFollow, timeFollowing;
    //La Enmity es con lo que huyen, y la affinity con la que se acercan a ti, ambas cambian, la enmity aleatoriamente entre el maximo y minimo y el otro
	
    public enum FSM
    {
        Friendly, Lonely, Group, Leader, Couple, ChangingToFriend, Friend
    }
    [SerializeField]
    private FSM actualStatus;

    void Awake()
    {
        anim = anim.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        render = render.GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Start()
    {
        render.color = standardColor;
        InvokeRepeating("ChangeStatus", 0f, Random.Range(2.5f, 3.5f));
    }
    void Update()
    {
        if (changingToFriend)
        {
            timeChangingToFriend -= Time.deltaTime;
            if (timeChangingToFriend <= 0)
            {
                TurnIntoFriend();
            }
        }

        switch (actualStatus)
        {
            case FSM.Friendly:
                //Aquí es to colega, puedes acercarte a él aunque se aleje a una velocidad menor
                render.color = friendlyColor;
                minEnmity = 0.1f; maxEnmity = 0.3f;
                break;
            case FSM.Lonely:
                //Aquí se comporta como un ser individual e independiente
                //Puede pasar a cualquier estado
                render.color = standardColor;
                minEnmity = 0.5f; maxEnmity = 0.9f;
                if (!isOnRange)
                {
                    anim.SetBool("RUNOUT", false);
                }
                break;
            case FSM.Group:
                //Busca al Leader más cercano en interpolaciones (en caso de perderlo en el proceso), mientras tanto, en esa interpolación se comporta como un Lonely, si lo tiene, va a buscarlo
                //Se comporta como un seguidor, pudiendo pasar a ser lonely, couple o friendly
                render.color = standardColor;
                if (lookForALeader)
                {
                    WhoIsTheNearestLeader();
                    lookForALeader = false;
                    Invoke("ResetLookForAPlayer", 1.5f);
                }
                if (nearestLeader == null)
                {
                    //Se comporta como un Lonely
                    //break;
                }
                else if (distanceToFollowLeader <= Vector2.Distance(transform.position, nearestLeader.position))
                {
                    followLeader = true;
                    anim.SetBool("RUNOUT", true);
                }
                else if (!runOut)
                {
                    followLeader = false;
                    anim.SetBool("RUNOUT", false);
                }
                minEnmity = 0.5f; maxEnmity = 0.8f;
                break;
            case FSM.Leader:
                //Puede pasar a ser Lonely si entra en contacto con el trigger del player, mientras tanto seguira un path seguido por sus colegas
                //No puede pasar ni a colega, ni a group, ni a friendly
                gameObject.tag = "Leader";
                if (canMoveLeader)
                {
                    canMoveLeader = false;
                    //Random move
                    float randomAngleIncrease = Random.Range(-anglePosibleToMove, anglePosibleToMove);
                    transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + randomAngleIncrease, Vector3.forward);
                    impulseTheFUCKINGLeader = true;
                    anim.SetTrigger("Impulse");

                    Invoke("ResetIfCanMoveTheLeader", Random.Range(1.3f, 3f));
                }
                render.color = standardColor;
                minEnmity = 0.5f; maxEnmity = 1f;
                break;
            case FSM.Couple:
                //Busca al potencial Couple más cercano y se liga, a ese, en caso de estar emparejados y que uno de ellos pase a ser otro estado, rompen y si el otro se queda en este estado
                //Busca a otra pareja, mientras tanto sencillamente lo mantenemos como Lonely, se comporta igual hasta que encuentra a alguien nuevo
                render.color = standardColor;
                
                //Buscar por la pareja soltera más cercana

                //Ir a por ella, como buen macho pecho peludo de gran nabo

                minEnmity = 0.8f; maxEnmity = 1f;
                break;
            case FSM.ChangingToFriend:
                //Estado en el que se queda quieto esperando a que se quede lejos, como solamente puedes pillar a los friendly, mientras estés en ese estado no podemos cambiarlo
                canChange = false;
                break;
            case FSM.Friend:
                //Aquí sigue al player, y eso, cada vez con una distancia mayor y tienes que volver a por él, es cuestión de interpolar colores
                render.color = friendColor;
                angle = Mathf.Atan2(Player.position.y - transform.position.y, Player.position.x - transform.position.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                //Controla la distancia máxima, si la supera sube a Lone
                if (Vector2.Distance(transform.position, Player.position) >= distanceToStopFollow)
                {
                    canChange = true;
                    changingToFriend = false;
                    actualStatus = FSM.Lonely;
                    anim.SetBool("RUNOUT", false);
                    break;
                }

                if (Vector2.Distance(transform.position, Player.position) >= minToFollow)
                {
                    anim.SetBool("RUNOUT", true);
                }
                else
                {
                    anim.SetBool("RUNOUT", false);
                }

                break;
        }
    }
    void SearchForChorva()
    {

    }
    void ResetIfCanMoveTheLeader()
    {
        canMoveLeader = true;
    }
    void ResetLookForAPlayer()
    {
        lookForALeader = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //Primero huye, a menos que le hayas logrado pillar por los huevers
        if (col.gameObject.tag == "PlayerSexAppeal" && actualStatus == FSM.Friendly)
        {
            actualStatus = FSM.ChangingToFriend;
            changingToFriend = true;
            timeChangingToFriend = timeToTurnInFriend;


            anim.SetBool("RUNOUT", false);
        }
        if (col.gameObject.tag == "Reek" && actualStatus != FSM.Friend && actualStatus != FSM.ChangingToFriend)
        {
            isOnRange = true;

            runOut = true;
            anim.SetBool("RUNOUT", true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        //Deja de huir si lo has conseguido
        if (col.gameObject.tag == "PlayerSexAppeal" && actualStatus == FSM.ChangingToFriend)
        {
            actualStatus = FSM.Friendly;
            changingToFriend = false;
        }
        if (col.gameObject.tag == "Reek" && actualStatus != FSM.Friend && actualStatus != FSM.ChangingToFriend)
        {
            isOnRange = false;

            runOut = false;
            anim.SetBool("RUNOUT", false);
        }

    }
    //Cambia el estado con su correspondiente factor de aleatoriedad, tiene los valores implementados así que para equilibrar tocarlos como os salga de la polla pero sin manchar, por favor
    //Que el teclado es nuevo
    void ChangeStatus()
    {
        if (canChange)
        {
            //La posibilidad de cambiar de estado la limitamos a un 50%, en caso de no cambiar de estado se mantiene, si no se lanza un nuevo random con las normas ya establecidas
            float ChangeStatusPosibility = Random.Range(0, 1f);
            Debug.Log("Changing" + ChangeStatusPosibility);
            if (ChangeStatusPosibility <= 0.7f)
            {
                Debug.Log("Will change");
                float newChangeStatus = Random.Range(0, 1f);
                switch (actualStatus)
                {
                    case FSM.Friendly:
                        if (newChangeStatus <= 0.15f)
                        {
                            actualStatus = FSM.Lonely;
                        }
                        break;
                    case FSM.Lonely:
                        if (newChangeStatus <= 0.3f)
                        {
                            actualStatus = FSM.Group;
                        }
                        else if (newChangeStatus <= 0.55f)
                        {
                            actualStatus = FSM.Couple;
                        }
                        else if (newChangeStatus <= 0.75f)
                        {
                            actualStatus = FSM.Friendly;
                            anim.SetBool("RUNOUT", false);
                        }
                        else if (newChangeStatus <= 0.95f)
                        {
                            actualStatus = FSM.Leader;
                        }
                        break;
                    case FSM.Group:
                        if (newChangeStatus <= 0.40f)
                        {
                            actualStatus = FSM.Lonely;
                        }
                        else if (newChangeStatus <= 0.65f)
                        {
                            actualStatus = FSM.Couple;
                        }
                        else if (newChangeStatus >= 0.90f)
                        {
                            actualStatus = FSM.Friendly;
                            anim.SetBool("RUNOUT", false);
                        }
                        break;
                    case FSM.Leader:
                        if (newChangeStatus <= 0.1f)
                        {
                            actualStatus = FSM.Lonely;
                        }
                        break;
                    case FSM.Couple:
                        if (newChangeStatus <= 0.25f)
                        {
                            actualStatus = FSM.Lonely;
                        }
                        else if (newChangeStatus <= 0.7f)
                        {
                            actualStatus = FSM.Group;
                        }
                        break;
                }
            }
        }
    }

    void WhoIsTheNearestLeader()
    {
        GameObject[] allTheLeaders = GameObject.FindGameObjectsWithTag("Leader");
        if (allTheLeaders.Length == 0)
        {
            nearestLeader = null;
            return;
        }
        else if (allTheLeaders.Length == 1)
        {
            Debug.Log(allTheLeaders.Length);
            nearestLeader = allTheLeaders[0].transform;
            return;
        }
        nearestLeader = allTheLeaders[0].transform;
        for (int i = 1; i < allTheLeaders.Length; i++)
        {
            if (Vector2.Distance(transform.position, allTheLeaders[i].transform.position) < Vector2.Distance(transform.position, nearestLeader.position))
            {
                nearestLeader = allTheLeaders[i].transform;
            }
        }
    }

    public void PulseToMove()
    {
        actualEnemity = Random.Range(minEnmity, maxEnmity);
        if (actualStatus == FSM.Friend)
        {
            //MoveToThePlayer
            rigid.AddRelativeForce(new Vector2(followSpeed * smothFollow, 0), ForceMode2D.Impulse);

        }
        else if (impulseTheFUCKINGLeader)
        {
            impulseTheFUCKINGLeader = false;
            rigid.AddRelativeForce(new Vector2(leaderMove * Random.Range(0.8f, 1.6f), 0), ForceMode2D.Impulse);
        }
        else if (followLeader && !runOut)
        {
            followLeader = false;
            angle = Mathf.Atan2(nearestLeader.position.y - transform.position.y, nearestLeader.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Debug.Log("Jumpy");
            rigid.AddRelativeForce(new Vector2(followSpeed * Random.Range(1.2f, 2f), 0), ForceMode2D.Impulse);
        }
        else
        {
            //Huir del player

            angle = Mathf.Atan2(Player.position.y - transform.position.y, Player.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle -180f, Vector3.forward);
            Debug.Log("Jumpy");
            rigid.AddRelativeForce(new Vector2(maxSpeed * actualEnemity, 0), ForceMode2D.Impulse);
        }
    }
    public void TurnIntoFriend()
    {
        //Te conviertes en amigo, en este caso te comportas como te corresponde
        actualStatus = FSM.Friend;
    }
    public void MakeMeAFriend()
    {
        //Ahora somos amigos, 4EverAndever, #OkNo #Bae
        actualStatus = FSM.Friend;
    }
}
