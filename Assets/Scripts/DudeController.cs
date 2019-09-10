using UnityEngine;

public class DudeController : MonoBehaviour
{

    [SerializeField]
    private float
          minSpeed = 3f
        , maxSpeed = 10f
        , currentSpeed = 3f
        , minMaxRotationSpeed = 5f
        , currentRotationSpeed = 1f
        , nextTimeEvent = 0f
        , minTimeEvent = 3f
        , maxTimeEvent = 10f;
    public int
          love = 50
        , health = 100
        , attraction = 100;

    private Rigidbody rb;



    private enum STATE { WANDERING, FOLLOWING, DANCING, FIGHTING, ESCAPING };
    [SerializeField] private STATE PlayerState = STATE.WANDERING;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        Debug.Log("I am a Dude");
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
        if (PlayerState == STATE.WANDERING)
        {
            Scan();
            Walk();
        } else if (PlayerState == STATE.FOLLOWING)
        {
            Follow();
        } else if (PlayerState == STATE.FIGHTING)
        {
            Fight();
        } else if (PlayerState == STATE.DANCING)
        {
            Dance();
        } else if (PlayerState == STATE.ESCAPING)
        {
            Escape();
        }

    }

    private void Walk()
    {

        rb.AddRelativeForce(Vector3.forward * currentSpeed);
        transform.Rotate(Vector3.up * currentRotationSpeed);

        if (Time.time > nextTimeEvent) ChangeWalkingMode();

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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 25f, LayerMask.GetMask("humans"));
        
        if (hitColliders.Length > 1)
        {
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].transform != transform)
                {
                    Debug.Log("Other Valio has " + hitColliders[i].gameObject.GetComponent<DudeController>().love + " LOVE for me");
                }
                i++;
            }
        }
    }

    private void Follow()
    {
        /*transform.LookAt()*/
    }

    private void Fight()
    {

    }

    private void Dance()
    {

    }

    private void Escape()
    {

    }

}
