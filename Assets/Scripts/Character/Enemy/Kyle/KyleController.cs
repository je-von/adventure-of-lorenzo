using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KyleController : MonoBehaviour
{
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
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        Vector3 pos = this.transform.position;
        pos.y = 1;
        var c = Instantiate(coreItem, pos, Quaternion.identity);

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

                    rw.raycastDest = GameObject.Find("EnemyTarget").transform;
                    //Transform t = collider[0].gameObject.transform;
                    //t.position += new Vector3(0, 100f, 0);
                    //rw.raycastDest = t;



                    rw.StartShooting();
                }
                else
                {
                    //animator.SetBool("isWalking", true);
                    //agent.updatePosition = true;
                }
            }
            else
            {
                if(collider.Length <= 0)
                {
                    //agent.isStopped = false;
                    //agent.updatePosition = true;
                    animator.SetBool("isWalking", true);
                    isAiming = false;
                    rw.StopShooting();
                }
                else
                {
                    transform.LookAt(collider[0].gameObject.transform.position);
                    yield return new WaitForSeconds(kyle.shootingInterval);
                    GameObject hitObject = rw.StartShooting();

                    if(hitObject != null)
                    {
                        //Debug.Log(hitObject.name);
                        if(hitObject.name == "Lorenzo")
                        {
                            Lorenzo.GetInstance().healthPoints -= kyle.bulletDamage;
                        }
                    }
                }
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
