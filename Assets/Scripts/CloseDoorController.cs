using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorController : MonoBehaviour
{
    public GameObject character;
    public Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(character.transform.position, transform.position) <= 1f)
        {
            Debug.Log("lewat");
            animator.SetBool("isOpen", false);
        }
    }
}
