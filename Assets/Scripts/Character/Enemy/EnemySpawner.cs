using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> patrolPoints;
    public GameObject spawnPoint;

    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //var k = Instantiate(kyle, spawnPoint.transform.position, Quaternion.identity);

        foreach (GameObject p in patrolPoints)
        {
            Transform patrolStart = p.transform.GetChild(0);
            Transform patrolEnd = p.transform.GetChild(1);

            SpawnEnemy(patrolStart, patrolEnd);
            //k.GetComponent<NavMeshAgent>().SetDestination(p.transform.position);

        }

        //StartCoroutine(SpawnPlayer());

        //k.GetComponent<NavMeshAgent>().destination = patrolPoints[1].transform.position;
        //k.GetComponent<NavMeshAgent>().Move(transform.position + (transform.position - patrolPoints[1].transform.position));
        //Debug.Log(k.GetComponent<NavMeshAgent>().destination);
    }

    public void SpawnEnemy(Transform patrolStart, Transform patrolEnd)
    {
        GameObject k = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity);
        k.GetComponent<EnemyController>().patrolStart = patrolStart;
        k.GetComponent<EnemyController>().patrolEnd = patrolEnd;
        k.GetComponent<EnemyController>().enemySpawner = this;

        k.GetComponent<NavMeshAgent>().SetDestination(patrolStart.position);
        Debug.Log("kebuat");
    }

    //IEnumerator SpawnPlayer()
    //{
    //    foreach (GameObject p in patrolPoints)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        GameObject k = Instantiate(kyle, spawnPoint.transform.position, Quaternion.identity);
    //        k.GetComponent<NavMeshAgent>().SetDestination(p.transform.position);
    //    }

    //    yield return null;
    //}

    // Update is called once per frame
    void Update()
    {
        //k.GetComponent<NavMeshAgent>().updatePosition = true;
        //Vector3 p = patrolPoints[1].transform.position;
        //p.y = 0;

        //k.GetComponent<NavMeshAgent>().destination = patrolPoints[1].transform.position;

        //k.GetComponent<NavMeshAgent>().Move(transform.forward * Time.deltaTime);
        //k.GetComponent<CharacterController>().Move(-transform.forward * Time.deltaTime);
        //patrolPoints.
        //var p = patrolPoints[0];
        //Instantiate(kyle, spawnPoint.transform.position, Quaternion.identity);
        //patrolPoints.Remove(p);

        //float distance = Vector3.Distance(transform.position, patrolPoints[1].transform.position);

        //Vector3 pos = transform.position + (transform.position - patrolPoints[1].transform.position);
        //Debug.Log(pos);
        //k.GetComponent<NavMeshAgent>().nextPosition = pos;



        //Debug.Log("status:" + k.GetComponent<NavMeshAgent>().pathStatus);
    }
}
