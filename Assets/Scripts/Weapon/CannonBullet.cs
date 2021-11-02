using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Lorenzo.GetInstance().healthPoints -= 250;
            Debug.Log("kena cannon");
        }
        else
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 6f);
        }
    }
}
