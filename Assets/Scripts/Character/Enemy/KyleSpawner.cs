using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KyleSpawner : MonoBehaviour
{
    public List<GameObject> patrolPoints;
    public GameObject spawnPoint;

    public GameObject kyle, k;

    // Start is called before the first frame update
    void Start()
    {
        //var k = Instantiate(kyle, spawnPoint.transform.position, Quaternion.identity);
        k = Instantiate(kyle, spawnPoint.transform.position, Quaternion.identity);
        //k.GetComponent<NavMeshAgent>().destination = patrolPoints[1].transform.position;
        //k.GetComponent<NavMeshAgent>().Move(transform.position + (transform.position - patrolPoints[1].transform.position));
        //Debug.Log(k.GetComponent<NavMeshAgent>().destination);
    }

    // Update is called once per frame
    void Update()
    {
        //k.GetComponent<NavMeshAgent>().updatePosition = true;
        k.GetComponent<NavMeshAgent>().destination = patrolPoints[1].transform.position;

        //Debug.Log(k.GetComponent<NavMeshAgent>().destination);
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

        //k.GetComponent<NavMeshAgent>().SetDestination(pos);
    }


}
