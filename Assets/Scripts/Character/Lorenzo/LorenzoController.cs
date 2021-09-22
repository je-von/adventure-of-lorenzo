using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorenzoController : MonoBehaviour
{
    protected CharacterController controller;
    Animator animator;
    public Vector3 velocity;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        velocity.y = 0;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            animator.SetBool("isAiming", !animator.GetBool("isAiming"));
        }
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var direction = Vector3.forward * vertical + Vector3.right * horizontal;
        if (direction.magnitude > 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

            animator.SetBool("isRunning", true);

        }
        else
        {
            animator.SetBool("isRunning", false);

        }
        if (velocity.y < 0 && controller.isGrounded)
        {
            velocity.y = 0f;
        }
        velocity.y -= 9.81f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
