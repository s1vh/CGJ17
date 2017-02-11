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
    private SpriteRenderer render;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private bool canChange, changingToFriend;
    //Controlador de tiempo, el tiempo en el que te conviertes a to colega
    [SerializeField]
    private float timeToTurnInFriend, maxSpeed, followSpeed, initialRotation, currentSpeed;
    private float timeChangingToFriend, minEnmity, maxEnmity, actualEnemity;
    //La Enmity es con lo que huyen, y la affinity con la que se acercan a ti, ambas cambian, la enmity aleatoriamente entre el maximo y minimo y el otro
    [SerializeField]
    private AudioClip blobSound;
	
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
                break;
            case FSM.Group:
                //Busca al Leader más cercano en interpolaciones (en caso de perderlo en el proceso), mientras tanto, en esa interpolación se comporta como un Lonely, si lo tiene, va a buscarlo
                //Se comporta como un seguidor, pudiendo pasar a ser lonely, couple o friendly
                render.color = standardColor;
                minEnmity = 0.5f; maxEnmity = 0.8f;
                break;
            case FSM.Leader:
                //Puede pasar a ser Lonely si entra en contacto con el trigger del player, mientras tanto seguira un path seguido por sus colegas
                //No puede pasar ni a colega, ni a group, ni a friendly
                render.color = standardColor;
                minEnmity = 0.5f; maxEnmity = 1f;
                break;
            case FSM.Couple:
                //Busca al potencial Couple más cercano y se liga, a ese, en caso de estar emparejados y que uno de ellos pase a ser otro estado, rompen y si el otro se queda en este estado
                //Busca a otra pareja, mientras tanto sencillamente lo mantenemos como Lonely, se comporta igual hasta que encuentra a alguien nuevo
                render.color = standardColor;
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
                        if (newChangeStatus <= 0.25f)
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
                        }
                        else if (newChangeStatus <= 0.8f)
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
                        else if (newChangeStatus <= 0.75f)
                        {
                            actualStatus = FSM.Friendly;
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
                        else if (newChangeStatus <= 0.5f)
                        {
                            actualStatus = FSM.Group;
                        }
                        break;
                }
            }
        }
    }

    public void PulseToMove()
    {
        actualEnemity = Random.Range(minEnmity, maxEnmity);
        AudioSource.PlayClipAtPoint(blobSound, this.transform.position);
        if (actualStatus == FSM.Friend)
        {
            //MoveToThePlayer
            rigid.AddRelativeForce(new Vector2(followSpeed * smothFollow, 0), ForceMode2D.Impulse);

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
