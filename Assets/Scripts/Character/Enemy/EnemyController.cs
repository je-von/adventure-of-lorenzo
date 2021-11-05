using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    public GameObject bombSprite;

    public SpriteRenderer[] inventoryItems;

    public GameObject player;

    public EnemySpawner enemySpawner;

    public Enemy enemy;
    private CharacterController controller;

    public SpriteRenderer coreItem;

    public GameObject patrolPoint;

    public Animator animator;
    //private NavMeshAgent agent;

    //public GameObject healthBar;

    public Transform patrolStart;
    public Transform patrolEnd;

    public Slider healthSlider;

    public LayerMask playerLayer;

    Vector3 destination;
    public NavMeshAgent agent;

    public GameObject weapon;

    public RaycastWeapon rw;
    public bool isAiming;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Lorenzo");

        isAiming = false;

        agent = GetComponent<NavMeshAgent>();
        enemy = new Kyle();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();

        StartCoroutine(MoveEnemy());
        StartCoroutine(CheckPlayerInRange());

        rw = weapon.GetComponentInChildren<RaycastWeapon>();

    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = patrolPoint.transform.position;

        //Debug.Log("-----" + this.gameObject);
        healthSlider.value = (float)enemy.healthPoints / (float)enemy.maxHealth;

        if (enemy.healthPoints <= 0)
        {
            StartCoroutine(DieAnimation());
            //Vector3 pos = this.transform.position;
            //pos.y = 1;
            //var c = Instantiate(coreItem, pos, Quaternion.identity);
            //c.a
        }


        //if(Physics.CheckSphere(transform.position, 10f, playerLayer))



    }

    public void SetEnemyInAttackRange(bool active)
    {
        bombSprite.SetActive(active);
    }
    public virtual IEnumerator DieAnimation()
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

        //item
        if (Random.Range(0, 1) < enemy.itemPossibilty)
        {
            int index = (int)Random.Range(0, 5);
            Instantiate(inventoryItems[index], pos + new Vector3(0, 0, 1.5f), Quaternion.identity);
        }

        //yield return new WaitForSeconds(3f);
        enemySpawner.SpawnEnemy(this.patrolStart, this.patrolEnd);
        Debug.Log("sampe");


    }

    public virtual IEnumerator CheckPlayerInRange()
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
                    yield return new WaitForSeconds(enemy.shootingInterval);
                    GameObject hitObject = rw.StartShooting();

                    if (hitObject != null)
                    {
                        if (hitObject.name == "Lorenzo")
                        {
                            //Lorenzo.GetInstance().healthPoints -= enemy.bulletDamage;
                            hitObject.GetComponent<LorenzoController>().DecreaseHealth(enemy.bulletDamage);
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
    public virtual IEnumerator MoveEnemy()
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
