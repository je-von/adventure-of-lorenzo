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

    public Slider slider;

    public LayerMask playerLayer;

    Vector3 destination;
    NavMeshAgent agent;

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
    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = patrolPoint.transform.position;

        //Debug.Log("-----" + this.gameObject);
        slider.value = (float)kyle.healthPoints / (float)kyle.maxHealth;

        if (kyle.healthPoints <= 0)
        {
            Destroy(this.gameObject);
            Vector3 pos = this.transform.position;
            pos.y = 1;
            var c = Instantiate(coreItem, pos, Quaternion.identity);
            //c.a
        }
        

        //if(Physics.CheckSphere(transform.position, 10f, playerLayer))



    }

    IEnumerator CheckPlayerInRange()
    {
        while (true)
        {
            yield return null;

            Collider[] collider = Physics.OverlapSphere(transform.position, 5f, playerLayer);
            Debug.Log(collider.Length);

            if (!isAiming)
            {
                if (collider.Length > 0 && collider[0].gameObject.name == "Lorenzo")
                {
                    destination = collider[0].gameObject.transform.position;

                    //yield return new WaitForSeconds(1f);

                    agent.updatePosition = false;
                    //agent.isStopped = true;
                    animator.SetBool("isWalking", false);
                    //Debug.Log("player masuk enemy range");
                    isAiming = true;
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
                    agent.updatePosition = true;
                    animator.SetBool("isWalking", true);
                    isAiming = false;
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
            if (agent.remainingDistance <= 0)
                destination = (destination == patrolStart.position) ? patrolEnd.position : patrolStart.position;
            agent.SetDestination(destination);
        }

    }
}
