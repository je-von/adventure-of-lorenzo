using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorenzoController : MonoBehaviour
{
    protected CharacterController controller;
    Animator animator;
    public Vector3 velocity;
    public float speed = 3f, turnSmoothTime = 0.1f, turnSmoothVelocity;
    public Weapon primaryWeapon, secondaryWeapon, currentWeapon;
    public GameObject rightHand,leftHand;
    public Transform cam;
    public bool isShootingMode;
    // Start is called before the first frame update
    void Start()
    {
        isShootingMode = false;

        primaryWeapon = new Weapon(GameObject.Find("Primary Weapon"), rightHand, new Vector3(0.064f, 0.158f, -0.025f), new Vector3(-38.653f, 97.681f, 311.494f));

        secondaryWeapon = new Weapon(GameObject.Find("Secondary Weapon"), leftHand, new Vector3(0.0111f, -0.068f, 0.087f), new Vector3(-204.06f, 0f, -129.7f));

        currentWeapon = primaryWeapon;

        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        velocity.y = 0;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAiming();
        StartCoroutine(ChangeWeapon());

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var direction = Vector3.forward * vertical + Vector3.right * horizontal;
        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            //var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * speed * Time.deltaTime);

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

    //private void ChangeWeapon()
    //{
        
    //}

    private void CheckAiming()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (!animator.GetBool("isAiming") && !isShootingMode)
            {
                isShootingMode = true;
                speed = 2f;
                currentWeapon.PickWeapon();

                animator.SetBool("isAiming", true);
            }
            else
            {
                if(isShootingMode)
                    StartCoroutine(WaitAndPutWeapon());
                isShootingMode = false;
                speed = 3f;
                animator.SetBool("isAiming", false);
            }


            //animator.SetBool("isAiming", !animator.GetBool("isAiming"));
        }
    }

    IEnumerator WaitAndPutWeapon()
    {
        yield return new WaitForSeconds(0.25f);

        currentWeapon.PutWeapon();
    }

    IEnumerator ChangeWeapon()
    {
        if (Input.GetKeyUp(KeyCode.Q) && isShootingMode)
        {
            if (animator.GetBool("isAiming") && currentWeapon != primaryWeapon)
            {
                StartCoroutine(WaitAndPutWeapon());
                animator.SetBool("isAiming", false);
                yield return new WaitForSeconds(0.25f);
            }
            currentWeapon = primaryWeapon;
            currentWeapon.PickWeapon();
            animator.SetBool("isAiming", true);
        }
        else if (Input.GetKeyUp(KeyCode.E) && isShootingMode)
        {
            if (animator.GetBool("isAiming") && currentWeapon != secondaryWeapon)
            {
                StartCoroutine(WaitAndPutWeapon());
                animator.SetBool("isAiming", false);
                yield return new WaitForSeconds(0.25f);
            }
            currentWeapon = secondaryWeapon;
            currentWeapon.PickWeapon();
            animator.SetBool("isAiming", true);
        }
        else if (Input.GetKeyUp(KeyCode.X) && isShootingMode)
        {
            if (animator.GetBool("isAiming"))
            {
                StartCoroutine(WaitAndPutWeapon());
            }
            else
            {
                currentWeapon.PickWeapon();
            }
            animator.SetBool("isAiming", !animator.GetBool("isAiming"));
        }

        //animator.SetBool("isAiming", !animator.GetBool("isAiming"));
        //yield return new WaitForSeconds(0.25f);

        //animator.SetBool("isAiming", !animator.GetBool("isAiming"));
    }
}
