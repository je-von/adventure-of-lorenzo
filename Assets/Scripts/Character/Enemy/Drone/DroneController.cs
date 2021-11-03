using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DroneController : EnemyController
{
    public GameObject droneObject;
    public override IEnumerator CheckPlayerInRange()
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
                    yield return new WaitForSeconds(enemy.shootingInterval);
                    GameObject hitObject = rw.StartShooting();

                    if (hitObject != null)
                    {
                        if (hitObject.name == "Lorenzo")
                        {
                            Lorenzo.GetInstance().healthPoints -= enemy.bulletDamage;
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

}
