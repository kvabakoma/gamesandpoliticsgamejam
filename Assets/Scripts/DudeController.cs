using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int 
          love = 50
        , health = 100
        , attraction = 100;

    private Rigidbody rb;



    private enum STATE { WANDERING, FOLLOWING, DANCING, FIGHTING };
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
        }
        Walk();
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("humans"));
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].SendMessage("AddDamage");
            i++;
        }
    }
}
