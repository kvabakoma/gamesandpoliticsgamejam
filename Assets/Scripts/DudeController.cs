﻿using UnityEngine;
using System.Collections.Generic;

public class DudeController : MonoBehaviour
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
    private GameObject targetDude; // DELETE
    [SerializeField] private GameObject angryIcon;
    private Animator animator;
    private Vector3 friendAreaCenter, dangerPosition;

    private enum STATE { WANDERING, FOLLOWING, DANCING, FIGHTING, ATTACKING, ESCAPING, DYING };
    [SerializeField] private STATE PlayerState = STATE.WANDERING;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        this.love = 12;
        if (this.name == "Valio") this.love = minLove;
        if (this.love < maxLove *.5f) angryIcon.SetActive(true);
        ReturnToWandering();
    }

    private void FixedUpdate()
    {

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
        /*else if (PlayerState == STATE.DYING)
        {
            Die();
        }*/

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
            // YOU ARE BAD
            if (this.love < maxLove * .5f)
            {
                if (this.PlayerState != STATE.ATTACKING && this.PlayerState != STATE.FIGHTING)
                {
                    int v = 0;
                    while (v < hitColliders.Length)
                    {
                        if (hitColliders[v].transform != transform)
                        {
                            targetDude = hitColliders[v].gameObject;
                            this.PlayerState = STATE.ATTACKING;
                            return;
                        }
                        v++;
                    }                    
                }
            }
            // YOU ARE GOOD
            else 
            {
                if (this.PlayerState == STATE.ESCAPING || this.PlayerState == STATE.DYING) return;
                int i = 0;
                friendAreaCenter = Vector3.zero;

                while (i < hitColliders.Length)
                {
                    if (hitColliders[i].transform != transform)
                    {
                        DudeController thisDudeController = hitColliders[i].GetComponent<DudeController>();

                        if (thisDudeController.love < maxLove *.5 && this.PlayerState != STATE.ESCAPING)
                        {
                            dangerPosition = hitColliders[i].gameObject.transform.position;
                            animator.SetTrigger("walking");
                            //transform.LookAt(transform.position - dangerPosition);
                            transform.LookAt(transform.position - friendAreaCenter);
                            rb.AddRelativeForce(Vector3.forward * minSpeed, ForceMode.Impulse);
                            this.PlayerState = STATE.ESCAPING;
                            Invoke("ReturnToWandering", 3);
                            return;
                        }

                        friendAreaCenter += hitColliders[i].transform.position;
                    }
                    i++;
                    friendAreaCenter = friendAreaCenter / i; // calculate the middle point
                }

                // ako ne si v pravilen state - otivai
                if (this.PlayerState != STATE.FOLLOWING && this.PlayerState != STATE.DANCING)
                {
                    this.PlayerState = STATE.FOLLOWING;
                }
                UpdateLoveAttribute(1);
            }
            
        }
        else if (this.PlayerState != STATE.ESCAPING) // RETURN TO WANDERING
        {
            if (this.PlayerState != STATE.WANDERING) ReturnToWandering();
        }
    }

    private void Follow()
    {
        transform.LookAt(friendAreaCenter);
        rb.angularVelocity = Vector3.zero;
        currentSpeed = loveSpeed;
        currentRotationSpeed = 0;

        if (Vector3.Distance(transform.position, friendAreaCenter) < danceDistance)
        {
            this.PlayerState = STATE.DANCING;
            animator.SetTrigger("hiphop");
        }
    }

    private void Dance()
    {
        // IF YOU LOSE YOUR DANCING PARTNER - EXIT
        // IF AN ENEMY ENTERS - RUN AWAY
        if (Vector3.Distance(transform.position, friendAreaCenter) > danceDistance)
        {
            ReturnToWandering();
        }
    }

    private void Attack()
    {
        transform.LookAt(targetDude.transform);
        rb.angularVelocity = Vector3.zero;
        rb.AddRelativeForce(Vector3.forward * .2f, ForceMode.Impulse);
        currentSpeed = maxSpeed;
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
        targetDude.GetComponent<DudeController>().Die();
        this.PlayerState = STATE.WANDERING;
        // fight until something happens
    }

    private void Escape()
    {
        rb.angularVelocity = Vector3.zero;
        currentSpeed = 0;
        currentRotationSpeed = 0;
        currentSpeed = maxSpeed;
    }

    public void Die()
    {
        this.PlayerState = STATE.DYING;
        Debug.Log("I died" + name);
        animator.SetTrigger("knockedout");
        Invoke("ReturnToWandering", 5f);
        /*transform.LookAt(transform.position - targetDude.transform.position);
        rb.angularVelocity = Vector3.zero;
        currentRotationSpeed = 0;
        currentSpeed = minSpeed;*/
    }

    private void UpdateLoveAttribute(int n)
    {
        this.love += n;
        this.love = Mathf.Clamp(this.love, 0, 100);
    }

    private void ReturnToWandering()
    {
        this.PlayerState = STATE.WANDERING;
        if (this.love < maxLove * .5f) animator.SetTrigger("walking");
        if (this.love >= maxLove * .5f) animator.SetTrigger("happywalk");
    }

}
