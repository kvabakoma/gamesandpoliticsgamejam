using UnityEngine;

public class DudeControllerV1 : MonoBehaviour
{

    [SerializeField]
    private float
          minSpeed = 3f
        , maxSpeed = 10f
        , loveSpeed = 5f
        , currentSpeed = 3f
        , minMaxRotationSpeed = 5f
        , currentRotationSpeed = 1f
        , nextTimeEvent = 0f
        , minTimeEvent = 3f
        , maxTimeEvent = 10f
        , attractionDistance = 15f
        , danceDistance = 5f
        , fightDistance = 1f;
    public int
          love = 50
        , minLove = 0
        , maxLove = 20;

    private Rigidbody rb;
    private GameObject targetDude;
    private DudeControllerV1 targetDudeController;
    private Animator animator;



    private enum STATE { WANDERING, FOLLOWING, DANCING, FIGHTING, ATTACKING, ESCAPING };
    [SerializeField] private STATE PlayerState = STATE.WANDERING;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        Debug.Log("I am a Dude");
        this.love = Random.Range(40, 60);
        ReturnToWandering();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        // DIFFERENT BEHAVIOUR BASED ON STATE
        // if WANDERING - SCAN
        // IF RELAXING - NOTHING (DELAY WILL EXIT IT)
        // IF FOLLOWING - NOTHING ( EVENT WILL EXIT IT)
        // IF DANCING - UPDATE STATS
        // IF FIGHTING - UPDATE STATS

        Walk();
        Scan();

        if (PlayerState == STATE.WANDERING)
        {            
            Rotate();
            if (Time.time > nextTimeEvent) ChangeWalkingMode();
        }
        else if (PlayerState == STATE.FOLLOWING)
        {
            Follow();
        }
        else if (PlayerState == STATE.ATTACKING)
        {
            Attack();
        }
        else if (PlayerState == STATE.FIGHTING)
        {
            Fight();
        }
        else if (PlayerState == STATE.DANCING)
        {
            Dance();
        }
        else if (PlayerState == STATE.ESCAPING)
        {
            Escape();
        }

    }

    private void Walk()
    {
        rb.AddRelativeForce(Vector3.forward * currentSpeed);
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * currentRotationSpeed);
    }

    private void ChangeWalkingMode()
    {
        nextTimeEvent = Time.time + Random.Range(minTimeEvent, maxTimeEvent);
        int draw = Random.Range(1, 8);
        Debug.Log("DRAW:" + draw);
        if (draw < 5) // walk straight
        {
            currentRotationSpeed = 0;
            currentSpeed = Random.Range(minSpeed, maxSpeed);
        }
        else if (draw < 8) // walk with turn
        {
            currentRotationSpeed = Random.Range(-minMaxRotationSpeed, minMaxRotationSpeed);
            currentSpeed = Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            currentRotationSpeed = 0;
            currentSpeed = 0;
        }
    }

    private void Scan()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attractionDistance, LayerMask.GetMask("humans"));
        
        if (hitColliders.Length > 1)
        {
            int i = 0;
            int totalLove = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].transform != transform)
                {
                    targetDude = hitColliders[i].gameObject;
                    targetDudeController = targetDude.GetComponent<DudeControllerV1>();
                    //Debug.Log(gameObject.name + " HAS DETECTED " + hitColliders[i].gameObject.GetComponent<DudeControllerV1>().love + " LOVE for me");

                    if (this.love >= 50 && targetDudeController.love >= 50 && this.PlayerState == STATE.WANDERING)
                    {
                        this.PlayerState = STATE.FOLLOWING;
                    }
                    else if (this.love >= 50 && targetDudeController.love < 50)
                    {
                        this.PlayerState = STATE.ESCAPING;
                    }
                    else if (this.love < 50 && targetDudeController.love >= 50 && this.PlayerState == STATE.WANDERING)
                    {
                        this.PlayerState = STATE.ATTACKING;
                    }
                    else if (this.love < 50 && targetDudeController.love < 50)
                    {
                        //
                    }

                    totalLove += hitColliders[i].gameObject.GetComponent<DudeControllerV1>().love;
                }
                i++;
                UpdateLoveAttribute(i, totalLove);
            } 
        }
        else // RETURN TO WANDERING
        {
            if (this.PlayerState != STATE.WANDERING) ReturnToWandering();
        }
    }


    /*transform.LookAt(targetDude.transform);*/
    // if I am good and you are good - follow
    // if I am good and you are bad - run
    // if I am bad nad you are good - attack
    // if I am bad and you are bad - avoid

    private void Follow()
    {
        Debug.Log(this.name + " IN FOLLOW STATE " + targetDudeController.name);
        transform.LookAt(targetDude.transform);
        rb.angularVelocity = Vector3.zero;
        currentSpeed = loveSpeed;
        currentRotationSpeed = 0;

        if (Vector3.Distance(transform.position,targetDude.transform.position) < danceDistance && this.love >=50 && targetDudeController.love >=50)
        {
            this.PlayerState = STATE.DANCING;
            animator.SetTrigger("hiphop");
        }
    }

    private void Attack()
    {
        Debug.Log(this.name + " IN FIGHT STATE " + targetDudeController.name);
        transform.LookAt(targetDude.transform);
        rb.angularVelocity = Vector3.zero;
        rb.AddRelativeForce(Vector3.forward * .2f, ForceMode.Impulse);
        currentRotationSpeed = 0;

        if (Vector3.Distance(transform.position, targetDude.transform.position) < fightDistance)
        {
            animator.SetTrigger("boxing");
            this.PlayerState = STATE.FIGHTING;
        }
    }

    private void Fight()
    {
        currentSpeed = minSpeed;
        // fight until something happens
    }

    private void Dance()
    {
        // IF YOU LOSE YOUR DANCING PARTNER - EXIT
        // IF AN ENEMY ENTERS - RUN AWAY
        if (Vector3.Distance(transform.position, targetDude.transform.position) > danceDistance)
        {
            ReturnToWandering();
        }
    }

    private void Escape()
    {
        transform.LookAt(transform.position - targetDude.transform.position);
        rb.angularVelocity = Vector3.zero;
        currentRotationSpeed = 0;
        currentSpeed = minSpeed;
    }

    private void UpdateLoveAttribute(int n, int totalLove)
    {
        if (totalLove / n < 50) this.love--;
        else this.love++;
        Mathf.Clamp(this.love, 0, 100);
    }

    private void ReturnToWandering()
    {
        this.PlayerState = STATE.WANDERING;
        if (this.love < 50) animator.SetTrigger("walking");
        if (this.love >= 50) animator.SetTrigger("happywalk");
    }

}
