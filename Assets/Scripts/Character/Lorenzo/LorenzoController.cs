using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorenzoController : MonoBehaviour
{
    protected CharacterController controller;
    Animator animator;
    public Vector3 velocity;
    public float speed = 3f, turnSmoothTime = 0.1f, turnSmoothVelocity;
    public Weapon currentWeapon;
    public GameObject rightHand,leftHand;
    public Transform cam;
    public bool isShootingMode;
    public GameObject exploreCam, shootingCamR, shootingCamL;

    // Start is called before the first frame update
    void Start()
    {
        isShootingMode = false;

        Lorenzo.GetInstance().primaryWeapon = new Weapon(GameObject.Find("Primary Weapon"), rightHand, new Vector3(0.0824f, 0.1932f, -0.0396f), new Vector3(-97.142f, 49.003f, 160.291f), 150, 15, 40, 10, 10);

        Lorenzo.GetInstance().secondaryWeapon = new Weapon(GameObject.Find("Secondary Weapon"), leftHand, new Vector3(0.0310f, 0.00602f, -0.0749f), new Vector3(-225.829f, -191.532f, -104.281f), 150, 10, 25, 5, 15);

        currentWeapon = Lorenzo.GetInstance().primaryWeapon;

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
        ChangeShootingCamera();

        if (isShootingMode)
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            var weaponRotation = Vector3.up * mouseY;

            transform.Rotate(new Vector3(0, mouseX, 0));

            currentWeapon.weaponObj.transform.Rotate(new Vector3(-mouseY, 0, 0));
            //rightHand.transform.Rotate(new Vector3(-mouseY, 0, 0));
            //leftHand.transform.Rotate(new Vector3(-mouseY, 0, 0));

            //if (playerRotation.magnitude > 0.1f)
            //{
            //var targetRotation = Quaternion.LookRotation(playerRotation, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);


            //}

            //if(weaponRotation.magnitude > 0.1f && animator.GetBool("isAiming"))
            //{
            //    var targetRotation = Quaternion.LookRotation(weaponRotation, Vector3.up);
            //    currentWeapon.weaponObj.transform.rotation = Quaternion.Slerp(currentWeapon.weaponObj.transform.rotation, targetRotation, speed * Time.deltaTime);
            //}

        }

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var direction = Vector3.forward * vertical + Vector3.right * horizontal;
        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if(!isShootingMode)
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

    private void ChangeShootingCamera()
    {
        if (isShootingMode)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (shootingCamR.activeInHierarchy)
                {
                    shootingCamL.SetActive(true);
                    shootingCamR.SetActive(false);
                }
                else
                {
                    shootingCamR.SetActive(true);
                    shootingCamL.SetActive(false);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                var currentCam = (shootingCamR.activeInHierarchy) ? shootingCamR : shootingCamL;
                currentCam.SetActive(false);
                currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius = 1.6f;
                currentCam.SetActive(true);

            }
            if (Input.GetMouseButtonUp(1))
            {
                var currentCam = (shootingCamR.activeInHierarchy) ? shootingCamR : shootingCamL;
                currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius = 3f;
            }
        }
        
    }

    

    private void CheckAiming()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!animator.GetBool("isAiming") && !isShootingMode)
            {
                shootingCamR.SetActive(true);
                exploreCam.SetActive(false);

                isShootingMode = true;
                speed = 2f;
                currentWeapon.PickWeapon();

                animator.SetBool("isAiming", true);
            }
            else
            {
                exploreCam.SetActive(true);
                shootingCamR.SetActive(false);
                shootingCamL.SetActive(false);


                if (isShootingMode)
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
        if (Input.GetKeyDown(KeyCode.Q) && isShootingMode)
        {
            if (animator.GetBool("isAiming") && currentWeapon != Lorenzo.GetInstance().primaryWeapon)
            {
                StartCoroutine(WaitAndPutWeapon());
                animator.SetBool("isAiming", false);
                yield return new WaitForSeconds(0.25f);
            }
            currentWeapon = Lorenzo.GetInstance().primaryWeapon;
            currentWeapon.PickWeapon();
            animator.SetBool("isAiming", true);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isShootingMode)
        {
            if (animator.GetBool("isAiming") && currentWeapon != Lorenzo.GetInstance().secondaryWeapon)
            {
                StartCoroutine(WaitAndPutWeapon());
                animator.SetBool("isAiming", false);
                yield return new WaitForSeconds(0.25f);
            }
            currentWeapon = Lorenzo.GetInstance().secondaryWeapon;
            currentWeapon.PickWeapon();
            animator.SetBool("isAiming", true);
        }
        else if (Input.GetKeyDown(KeyCode.X) && isShootingMode)
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
