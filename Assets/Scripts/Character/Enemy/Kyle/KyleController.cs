using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KyleController : MonoBehaviour
{
    public GameObject player;

    public KyleSpawner kyleSpawner;

    public Kyle kyle;
    private CharacterController controller;

    public SpriteRenderer coreItem;

    public GameObject patrolPoint;

    Animator animator;
    //private NavMeshAgent agent;

    //public GameObject healthBar;

    public Transform patrolStart;
    public Transform patrolEnd;

    public Slider healthSlider;

    public LayerMask playerLayer;

    Vector3 destination;
    NavMeshAgent agent;

    public GameObject weapon;

    RaycastWeapon rw;
    bool isAiming;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Lorenzo");

        isAiming = false;

        agent = GetComponent<NavMeshAgent>();
        kyle = new Kyle();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();

        StartCoroutine(MoveKyle());
        StartCoroutine(CheckPlayerInRange());

        rw = weapon.GetComponentInChildren<RaycastWeapon>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = patrolPoint.transform.position;

        //Debug.Log("-----" + this.gameObject);
        healthSlider.value = (float)kyle.healthPoints / (float)kyle.maxHealth;

        if (kyle.healthPoints <= 0)
        {
            StartCoroutine(DieAnimation());
            //Vector3 pos = this.transform.position;
            //pos.y = 1;
            //var c = Instantiate(coreItem, pos, Quaternion.identity);
            //c.a
        }


        //if(Physics.CheckSphere(transform.position, 10f, playerLayer))



    }

    IEnumerator DieAnimation()
    {
        rw.StopShooting();
        //isAiming = false;
        rw.raycastDest = rw.raycastSource;
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        Vector3 pos = this.transform.position;
        pos.y = 1;
        var c = Instantiate(coreItem, pos, Quaternion.identity);


        //yield return new WaitForSeconds(3f);
        kyleSpawner.SpawnKyle(this.patrolStart, this.patrolEnd);
        Debug.Log("sampe");
    }

    IEnumerator CheckPlayerInRange()
    {
        while (true)
        {
            yield return null;

            if (Vector3.Distance(player.transform.position, transform.position) <= 5f)
            {
                if (!isAiming)
                {
                    agent.SetDestination(transform.position);
                    transform.LookAt(player.transform.position);
                    animator.SetBool("isWalking", false);
                    isAiming = true;

                    rw.raycastDest = GameObject.Find("EnemyTarget").transform;

                    rw.StartShooting();
                }
                else
                {
                    transform.LookAt(player.transform.position);
                    yield return new WaitForSeconds(kyle.shootingInterval);
                    GameObject hitObject = rw.StartShooting();

                    if (hitObject != null)
                    {
                        if (hitObject.name == "Lorenzo")
                        {
                            Lorenzo.GetInstance().healthPoints -= kyle.bulletDamage;
                        }
                    }

                }
            }
            else
            {
                animator.SetBool("isWalking", true);
                isAiming = false;
                rw.StopShooting();
            }
        }
    }

    public float turnSmoothVelocity;
    IEnumerator MoveKyle()
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
