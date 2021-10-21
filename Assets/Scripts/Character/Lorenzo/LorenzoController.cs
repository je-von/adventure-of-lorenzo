using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LorenzoController : MonoBehaviour
{
    protected CharacterController controller;
    Animator animator;
    public Vector3 velocity;
    public float speed = 3f, turnSmoothTime = 0.1f, turnSmoothVelocity;
    public Weapon currentWeapon;
    public GameObject rightHand,leftHand;
    public Transform cam;
    public bool isShootingMode, isPaused;
    public GameObject exploreCam, shootingCamR, shootingCamL, pausePanel, deathPanel;
    public Slider healthSlider, skillSlider;
    RaycastWeapon rw;

    public List<GameObject> inventories;
    // Start is called before the first frame update
    void Start()
    {
        //rw = GetComponentInChildren<RaycastWeapon>();

        isPaused = false;

        isShootingMode = false;

        Lorenzo.GetInstance().lorenzoObject = this.gameObject;

        Lorenzo.GetInstance().primaryWeapon = new Weapon(GameObject.Find("Primary Weapon"), rightHand, new Vector3(0.0824f, 0.1932f, -0.0396f), new Vector3(-97.142f, 49.003f, 160.291f), 150, 15, 40, 10, 10, true);

        Lorenzo.GetInstance().secondaryWeapon = new Weapon(GameObject.Find("Secondary Weapon"), leftHand, new Vector3(0.0310f, 0.00602f, -0.0749f), new Vector3(-225.829f, -191.532f, -104.281f), 120, 10, 25, 5, 15, false);

        currentWeapon = Lorenzo.GetInstance().primaryWeapon;

        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        velocity.y = 0;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);

        healthSlider.value = (float)Lorenzo.GetInstance().healthPoints / (float)Lorenzo.GetInstance().maxHealth;
        skillSlider.value = (float)Lorenzo.GetInstance().skillPoints / (float)Lorenzo.GetInstance().maxSkill;
        CheckDeath();
        CheckAiming();
        StartCoroutine(ChangeWeapon());
        ChangeShootingCamera();
        RefreshInventory();
        GetInventoryInput();

        var esc = Input.GetKeyDown(KeyCode.Escape);

        if (esc)
        {
            if (!isPaused)
            {
                PauseMenu();
            }
            else
            {
                ResumeMenu();
            }

        }


        if (isShootingMode)
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            var weaponRotation = Vector3.up * mouseY;

            transform.Rotate(new Vector3(0, mouseX, 0));

            //currentWeapon.weaponObj.transform.Rotate(new Vector3(-mouseY, 0, 0));
            currentWeapon.RotateWeapon(mouseY);

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

    private void GetInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Lorenzo.GetInstance().UseInventoryItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Lorenzo.GetInstance().UseInventoryItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Lorenzo.GetInstance().UseInventoryItem(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Lorenzo.GetInstance().UseInventoryItem(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Lorenzo.GetInstance().UseInventoryItem(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Lorenzo.GetInstance().UseInventoryItem(6);
        }
    }

    

    private void RefreshInventory()
    {
        //Debug.Log(Lorenzo.GetInstance().items.Count);
        for (int i = 0; i < 6; i++)
        {
            var inventory = inventories[i].transform.Find("ITEM_IMAGE").GetComponent<Image>();
            if (Lorenzo.GetInstance().items.Count > i)
            {
                inventory.sprite = Lorenzo.GetInstance().items[i].sprite;
                inventory.gameObject.SetActive(true);
            }
            else
            {
                inventory.gameObject.SetActive(false);
                //break;
            }
        }
    }

    private void CheckDeath()
    {
        if(Lorenzo.GetInstance().healthPoints <= 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            deathPanel.SetActive(true);
        }
    }

    public void RestartMenu()
    {
        SceneManager.LoadScene(sceneName: "GameScene", LoadSceneMode.Single);
    }

    public void PauseMenu()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        pausePanel.SetActive(true);
    }

    public void ResumeMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene(sceneName: "MainScene", LoadSceneMode.Single);
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
            //zoom kamera
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(ZoomInCamera());

            }
            if (Input.GetMouseButtonUp(1))
            {
                //var currentCam = (shootingCamR.activeInHierarchy) ? shootingCamR : shootingCamL;
                //currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius = 3f;
                StartCoroutine(ZoomOutCamera());
            }

            if (isShootingMode && animator.GetBool("isAiming"))
            {
                rw = currentWeapon.weaponObj.GetComponentInChildren<RaycastWeapon>();
                if (Input.GetMouseButtonDown(0))
                {
                    if (currentWeapon.DecreaseAmmo(1))
                    {
                        GameObject hitObject = rw.StartShooting();
                        //currentWeapon.currentAmmo--;
                        if (hitObject != null)
                        {
                            //Debug.Log(hitObject.name + " | " + hitObject.tag);
                            //Debug.Log(hitObject.name);


                            if (hitObject.tag == "KYLE")
                            {
                                KyleController kc = hitObject.GetComponentInChildren<KyleController>();
                                //Debug.Log(kc.kyle.healthPoints + " - " + currentWeapon.damage + " = " + (kc.kyle.healthPoints - currentWeapon.damage));
                                kc.kyle.healthPoints -= currentWeapon.damage;
                            
                                //Debug.Log(kc.kyle.healthPoints);

                            }
                        }

                    }
                    else
                    {
                        StartCoroutine(ReloadWeapon());
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    rw.StopShooting();
                }
            }
        }
        
    }

    IEnumerator ZoomInCamera()
    {
        var currentCam = (shootingCamR.activeInHierarchy) ? shootingCamR : shootingCamL;
        while(currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius > 1.6f)
        {
            yield return null;
            currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius -= Time.deltaTime * 2f;

        }
    }

    IEnumerator ZoomOutCamera()
    {
        var currentCam = (shootingCamR.activeInHierarchy) ? shootingCamR : shootingCamL;
        while (currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius < 3f)
        {
            yield return null;
            currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius += Time.deltaTime * 2f;

        }
    }

    IEnumerator ReloadWeapon()
    {
        Debug.Log("reloading");
        yield return new WaitForSeconds(5f);

        currentWeapon.currentAmmo = currentWeapon.maxAmmo;
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
                currentWeapon.isUsed = false;
                StartCoroutine(WaitAndPutWeapon());
                animator.SetBool("isAiming", false);
                yield return new WaitForSeconds(0.25f);
            }
            currentWeapon = Lorenzo.GetInstance().primaryWeapon;
            currentWeapon.PickWeapon();
            currentWeapon.isUsed = true;
            animator.SetBool("isAiming", true);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isShootingMode)
        {
            if (animator.GetBool("isAiming") && currentWeapon != Lorenzo.GetInstance().secondaryWeapon)
            {
                currentWeapon.isUsed = false;
                StartCoroutine(WaitAndPutWeapon());
                animator.SetBool("isAiming", false);
                yield return new WaitForSeconds(0.25f);
            }
            currentWeapon = Lorenzo.GetInstance().secondaryWeapon;
            currentWeapon.PickWeapon();
            currentWeapon.isUsed = true;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //controller.co
        if (hit.gameObject.tag == "CORE ITEM")
        {
        //Debug.Log(hit.gameObject.name);
            Destroy(hit.gameObject);
            Lorenzo.GetInstance().coreItemCount++;
        }
        
        if(Lorenzo.GetInstance().items.Count < 6)
        {
            if(hit.gameObject.tag == "SKILL ITEM")
            {
                Lorenzo.GetInstance().items.Add(new SkillPotion(this));
                //Debug.Log(Lorenzo.GetInstance().items.Count);
                Destroy(hit.gameObject);
            }
            else if (hit.gameObject.tag == "HEALTH ITEM")
            {
                Lorenzo.GetInstance().items.Add(new HealthPotion(this));
                Destroy(hit.gameObject);
            }
            else if (hit.gameObject.tag == "SHIELD ITEM")
            {
                Lorenzo.GetInstance().items.Add(new Shield(this));
                Destroy(hit.gameObject);
            }
            else if (hit.gameObject.tag == "AMMO ITEM")
            {
                Lorenzo.GetInstance().items.Add(new Ammo(this));
                Destroy(hit.gameObject);
            }
            else if (hit.gameObject.tag == "PAINKILLER ITEM")
            {
                Lorenzo.GetInstance().items.Add(new PainKiller(this));
                Destroy(hit.gameObject);
            }
            else if (hit.gameObject.tag == "DOUBLEDAMAGE ITEM")
            {
                Lorenzo.GetInstance().items.Add(new DamageMultiplier(this));
                Destroy(hit.gameObject);
            }
        }
        
    }
}
