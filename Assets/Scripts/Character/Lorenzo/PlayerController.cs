using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Camera cam;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", horizontal);
        animator.SetFloat("InputY", vertical);


    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, 
                                              Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0), 
                                              speed * Time.fixedDeltaTime);
    }
}
