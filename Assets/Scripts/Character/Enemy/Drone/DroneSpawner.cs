using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneSpawner : MonoBehaviour
{
    public List<GameObject> patrolPoints;
    public GameObject spawnPoint;

    public GameObject drone;

    void Start()
    {
        foreach (GameObject p in patrolPoints)
        {
            Transform patrolStart = p.transform.GetChild(0);
            Transform patrolEnd = p.transform.GetChild(1);

            SpawnDrone(patrolStart, patrolEnd);

        }

        //StartCoroutine(SpawnPlayer());

        //k.GetComponent<NavMeshAgent>().destination = patrolPoints[1].transform.position;
        //k.GetComponent<NavMeshAgent>().Move(transform.position + (transform.position - patrolPoints[1].transform.position));
        //Debug.Log(k.GetComponent<NavMeshAgent>().destination);
    }

    public void SpawnDrone(Transform patrolStart, Transform patrolEnd)
    {
        GameObject k = Instantiate(drone, spawnPoint.transform.position, Quaternion.identity);
        k.GetComponent<DroneController>().patrolStart = patrolStart;
        k.GetComponent<DroneController>().patrolEnd = patrolEnd;
        k.GetComponent<DroneController>().droneSpawner = this;

        k.GetComponent<NavMeshAgent>().SetDestination(patrolStart.position);
        Debug.Log("kebuat");
    }

    IEnumerator SpawnPlayer()
    {
        foreach (GameObject p in patrolPoints)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject k = Instantiate(drone, spawnPoint.transform.position, Quaternion.identity);
            k.GetComponent<NavMeshAgent>().SetDestination(p.transform.position);
        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
