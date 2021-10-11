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
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        kyle = new Kyle();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();

        //StartCoroutine(MoveKyle());
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

        

        
    }

    public float turnSmoothVelocity;
    IEnumerator MoveKyle()
    {
        animator.SetBool("isWalking", true);

        int i = 0;
        Vector3 direction = Vector3.forward;
        
        while (true)
        {
            //Debug.Log(i);
            if (i > 200)
            {
                i = 0;
                direction = -direction;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 2f * Time.deltaTime);
            }
            float targetAngle = Mathf.Atan2(direction.x, direction.z);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * direction;
            controller.Move(moveDirection.normalized * 2f * Time.deltaTime);
            i++;

            yield return null;
        }

    }
}
