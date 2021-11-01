using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{
    public GameObject player;

    public GameObject droneObject;

    public DroneSpawner droneSpawner;

    public Drone drone;
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

        agent = GetComponentInChildren<NavMeshAgent>();
        drone = new Drone();
        //controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();

        StartCoroutine(MoveDrone());
        StartCoroutine(CheckPlayerInRange());

        rw = weapon.GetComponentInChildren<RaycastWeapon>();

    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = patrolPoint.transform.position;

        //Debug.Log("-----" + this.gameObject);
        healthSlider.value = (float)drone.healthPoints / (float)drone.maxHealth;

        if (drone.healthPoints <= 0)
        {
            //StopAllCoroutines();
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
        //agent.baseOffset = 0;
        //agent.isStopped = true;


        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
        Vector3 pos = this.transform.position;
        pos.y = 1;
        var c = Instantiate(coreItem, pos, Quaternion.identity);


        //yield return new WaitForSeconds(3f);
        droneSpawner.SpawnDrone(this.patrolStart, this.patrolEnd);
        Debug.Log("sampe");
    }

    IEnumerator CheckPlayerInRange()
    {
        while (true)
        {
            yield return null;
            if (Vector3.Distance(player.transform.position, transform.position) <= 10f)
            {
                if (!isAiming)
                {
                    agent.SetDestination(transform.position);
                    droneObject.transform.LookAt(player.transform.position);
                    //animator.SetBool("isWalking", false);
                    isAiming = true;

                    rw.raycastDest = GameObject.Find("EnemyTarget").transform;

                    rw.StartShooting();
                }
                else
                {
                    droneObject.transform.LookAt(player.transform.position);
                    yield return new WaitForSeconds(drone.shootingInterval);
                    GameObject hitObject = rw.StartShooting();

                    if (hitObject != null)
                    {
                        if (hitObject.name == "Lorenzo")
                        {
                            Lorenzo.GetInstance().healthPoints -= drone.bulletDamage;
                        }
                    }

                }
            }
            else
            {
                //animator.SetBool("isWalking", true);
                isAiming = false;
                rw.StopShooting();
                droneObject.transform.rotation = transform.rotation;
            }

        }
    }

    public float turnSmoothVelocity;
    IEnumerator MoveDrone()
    {
        //animator.SetBool("isWalking", true);

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
