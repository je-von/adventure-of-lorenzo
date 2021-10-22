using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MechController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI overrideText;

    public GameObject player;

    private bool isDead;

    public Mech mech;
    private CharacterController controller;

    //public SpriteRenderer coreItem;

    //public GameObject patrolPoint;

    Animator animator;
    //private NavMeshAgent agent;

    //public GameObject healthBar;

    public Transform patrolStart;
    public Transform patrolEnd;

    public Slider healthSlider;

    public LayerMask playerLayer;

    Vector3 destination;
    NavMeshAgent agent;

    //public GameObject weapon;

    //RaycastWeapon rw;
    bool isAiming;
    private List<Coroutine> coroutines;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        coroutines = new List<Coroutine>();
        isDead = false;
        isAiming = false;

        agent = GetComponent<NavMeshAgent>();
        mech = new Mech();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();

        coroutines.Add(StartCoroutine(MoveMech()));
        coroutine = StartCoroutine(CheckPlayerInRange());

        //rw = weapon.GetComponentInChildren<RaycastWeapon>();

    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = patrolPoint.transform.position;

        //Debug.Log("-----" + this.gameObject);
        healthSlider.value = (float)mech.healthPoints / (float)mech.maxHealth;

        if (mech.healthPoints <= 0 && !isDead)
        {
            transform.Find("Canvas").gameObject.SetActive(false);
            StopCoroutine(coroutine);
            agent.isStopped = true;
            isDead = true;
            //ShowOverride();

            //StartCoroutine(DieAnimation());
            //Vector3 pos = this.transform.position;
            //pos.y = 1;
            //var c = Instantiate(coreItem, pos, Quaternion.identity);
            //c.a
        }


        //if(Physics.CheckSphere(transform.position, 10f, playerLayer))

        if (Vector3.Distance(player.transform.position, transform.position) <= 3f && isDead)
        {
            ShowOverride();
        }
        else
        {
            overrideText.gameObject.SetActive(false);
        }

    }

    private void ShowOverride()
    {
        Debug.Log("OVERRIDE");
        overrideText.gameObject.SetActive(true);
    }

    IEnumerator DieAnimation()
    {
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);

    }

    IEnumerator CheckPlayerInRange()
    {
        while (true)
        {
            yield return null;

            Collider[] collider = Physics.OverlapSphere(transform.position, 5f, playerLayer);
            //Debug.Log(collider.Length);

            if (!isAiming)
            {
                if (collider.Length > 0 && collider[0].gameObject.name == "Lorenzo")
                {
                    //destination = collider[0].gameObject.transform.position;
                    agent.SetDestination(transform.position);
                    transform.LookAt(collider[0].gameObject.transform.position);

                    //yield return new WaitForSeconds(1f);

                    //agent.updatePosition = false;
                    //agent.isStopped = true;
                    animator.SetBool("isWalking", false);
                    //Debug.Log("player masuk enemy range");
                    isAiming = true;

                    //rw.raycastDest = GameObject.Find("EnemyTarget").transform;
                    //Transform t = collider[0].gameObject.transform;
                    //t.position += new Vector3(0, 100f, 0);
                    //rw.raycastDest = t;



                    //rw.StartShooting();
                }
                else
                {
                    //animator.SetBool("isWalking", true);
                    //agent.updatePosition = true;
                }
            }
            else
            {
                if (collider.Length <= 0)
                {
                    //agent.isStopped = false;
                    //agent.updatePosition = true;
                    animator.SetBool("isWalking", true);
                    isAiming = false;
                    //rw.StopShooting();
                }
                else
                {
                    transform.LookAt(collider[0].gameObject.transform.position);
                    yield return new WaitForSeconds(mech.shootingInterval);
                    //GameObject hitObject = rw.StartShooting();

                    //if (hitObject != null)
                    //{
                    //    //Debug.Log(hitObject.name);
                    //    if (hitObject.name == "Lorenzo")
                    //    {
                    //        Lorenzo.GetInstance().healthPoints -= mech.bulletDamage;
                    //    }
                    //}
                }
            }



        }
    }

    public float turnSmoothVelocity;
    IEnumerator MoveMech()
    {
        animator.SetBool("isWalking", true);

        destination = patrolStart.position;


        while (true)
        {
            yield return null;
            if (!isAiming)
            {
                if (agent.remainingDistance <= 0)
                    destination = (destination == patrolStart.position) ? patrolEnd.position : patrolStart.position;
                agent.SetDestination(destination);
            }
        }

    }
}
