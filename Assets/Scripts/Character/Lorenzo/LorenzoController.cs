using Cinemachine;
using System;
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
    public GameObject rightHand, leftHand;
    public Transform cam;
    public bool isShootingMode, isPaused;
    public GameObject exploreCam, shootingCamR, shootingCamL, pausePanel, deathPanel, victoryPanel;
    public Slider healthSlider, skillSlider;
    RaycastWeapon rw;

    public List<GameObject> inventories;



    // Start is called before the first frame update
    void Start()
    {
        Lorenzo.GetInstance().restart();
        //rw = GetComponentInChildren<RaycastWeapon>();

        isPaused = false;

        isShootingMode = false;

        Lorenzo.GetInstance().lorenzoObject = this.gameObject;

        Lorenzo.GetInstance().primaryWeapon = new Weapon(GameObject.Find("Primary Weapon"), rightHand, new Vector3(0.0824f, 0.1932f, -0.0396f), new Vector3(-97.142f, 49.003f, 160.291f), 30, 150, 15, 40, 10, 10, true);

        Lorenzo.GetInstance().secondaryWeapon = new Weapon(GameObject.Find("Secondary Weapon"), leftHand, new Vector3(0.0310f, 0.00602f, -0.0749f), new Vector3(-225.829f, -191.532f, -104.281f), 30, 120, 10, 25, 5, 15, false);

        currentWeapon = Lorenzo.GetInstance().primaryWeapon;

        //Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        velocity.y = 0;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueController.isShowing)
            return;

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


        if (isShootingMode && !isPaused)
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
            if (!isShootingMode)
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
            var quantity = inventories[i].transform.Find("QUANTITY").GetComponent<TMPro.TextMeshProUGUI>();
            if (Lorenzo.GetInstance().items.Count > i)
            {
                inventory.sprite = Lorenzo.GetInstance().items[i].sprite;
                quantity.text = Lorenzo.GetInstance().items[i].quantity + "";
                inventory.gameObject.SetActive(true);
                quantity.gameObject.SetActive(true);

            }
            else
            {
                inventory.gameObject.SetActive(false);
                quantity.gameObject.SetActive(false);
                //break;
            }
        }
    }

    private void CheckDeath()
    {
        if (Lorenzo.GetInstance().healthPoints <= 0)
        {
            StartCoroutine(DieAnimation());

            
        }
    }
    IEnumerator DieAnimation()
    {
        rw.StopShooting();
        //isAiming = false;
        rw.raycastDest = rw.raycastSource;
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(3f);

        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        deathPanel.SetActive(true);
    }
    public void RestartMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        //Lorenzo.GetInstance().restart();
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

    public void ShowVictoryMenu()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;

        var text = victoryPanel.transform.Find("finish_time").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Finished in " +  TimeSpan.FromSeconds(TimeController.currentTime).ToString("mm\\:ss");
        victoryPanel.SetActive(true);

        Debug.Log("masuk");
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        //Lorenzo.GetInstance().restart();
        SceneManager.LoadScene(sceneName: "MainScene", LoadSceneMode.Single);
    }
    //private Coroutine current;

    private float nextFire = 0.5f, currTime = 0f;

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
                //if(current != null)
                //    StopCoroutine(current);
                StartCoroutine(ZoomInCamera());

            }
            if (Input.GetMouseButtonUp(1))
            {
                //var currentCam = (shootingCamR.activeInHierarchy) ? shootingCamR : shootingCamL;
                //currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius = 3f;

                //if (current != null)
                //    StopCoroutine(current);
                StartCoroutine(ZoomOutCamera());
            }

            if (isShootingMode && animator.GetBool("isAiming"))
            {
                if (Input.GetKeyDown(KeyCode.R) && !animator.GetBool("IsReloading"))
                {
                    StartCoroutine(ReloadWeapon());
                }

                rw = currentWeapon.weaponObj.GetComponentInChildren<RaycastWeapon>();

                currTime += Time.deltaTime;
                if (Input.GetMouseButton(0) && currTime > nextFire)
                {
                    if (!animator.GetBool("IsReloading"))
                    {
                        if (currentWeapon.DecreaseAmmo(1))
                        {
                            nextFire = currTime + 1f / (float)currentWeapon.fireRate;
                            GameObject hitObject = rw.StartShooting();
                            //currentWeapon.currentAmmo--;
                            if (hitObject != null)
                            {
                                //Debug.Log(hitObject.name + " | " + hitObject.tag);
                                Debug.Log(hitObject.name);


                                if (hitObject.tag == "KYLE")
                                {
                                    KyleController kc = hitObject.GetComponentInChildren<KyleController>();
                                    //Debug.Log(kc.kyle.healthPoints + " - " + currentWeapon.damage + " = " + (kc.kyle.healthPoints - currentWeapon.damage));
                                    kc.kyle.healthPoints -= currentWeapon.damage;

                                    //Debug.Log(kc.kyle.healthPoints);

                                } 
                                else if (hitObject.tag == "MECH")
                                {
                                    MechController mc = hitObject.GetComponentInChildren<MechController>();
                                    Debug.Log(mc.mech.healthPoints + " - " + currentWeapon.damage + " = " + (mc.mech.healthPoints - currentWeapon.damage));
                                    mc.mech.healthPoints -= currentWeapon.damage;

                                    //Debug.Log(kc.kyle.healthPoints);

                                }
                                else if (hitObject.tag == "WARRIOR")
                                {
                                    WarriorController wc = hitObject.GetComponentInChildren<WarriorController>();
                                    wc.warrior.healthPoints -= currentWeapon.damage;

                                    //Debug.Log(kc.kyle.healthPoints);

                                }
                                else if (hitObject.tag == "BOSS")
                                {
                                    BossController bc = hitObject.GetComponentInChildren<BossController>();
                                    bc.boss.healthPoints -= currentWeapon.damage;

                                }
                            }
                            nextFire -= currTime;
                            currTime = 0f;
                        }
                        else
                        {
                            StartCoroutine(ReloadWeapon());
                        }

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
        while (currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius > 1.6f && Input.GetMouseButton(1))
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
            currentCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Radius += Time.deltaTime * 3f;

        }
    }

    IEnumerator ReloadWeapon()
    {
        Debug.Log("reloading");
        int ammoUsed = currentWeapon.clipSize - currentWeapon.currentAmmo;
        if (currentWeapon.spareAmmo >= ammoUsed)
        {
            animator.SetBool("IsReloading", true);
            yield return new WaitForSeconds(5f);
            animator.SetBool("IsReloading", false);
            currentWeapon.currentAmmo = currentWeapon.clipSize;
            currentWeapon.spareAmmo -= ammoUsed;

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

        //if(Lorenzo.GetInstance().items.Count < 6)
        //{
        if (hit.gameObject.tag == "SKILL ITEM")
        {

            var item = Lorenzo.GetInstance().items.Find(i => i is SkillPotion);
            if (item == null && Lorenzo.GetInstance().items.Count < 6)
            {
                Lorenzo.GetInstance().items.Add(new SkillPotion(this));
                //Debug.Log(Lorenzo.GetInstance().items.Count);

            }
            else
            {
                item.quantity++;
                Debug.Log(item.quantity);
            }

            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "HEALTH ITEM")
        {
            
            var item = Lorenzo.GetInstance().items.Find(i => i is HealthPotion);
            if (item == null && Lorenzo.GetInstance().items.Count < 6)
            {
                Lorenzo.GetInstance().items.Add(new HealthPotion(this));
            }
            else
            {
                item.quantity++;
            }


            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "SHIELD ITEM")
        {
            
            var item = Lorenzo.GetInstance().items.Find(i => i is Shield);
            if (item == null && Lorenzo.GetInstance().items.Count < 6)
            {
                Lorenzo.GetInstance().items.Add(new Shield(this));
            }
            else
            {
                item.quantity++;
            }

            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "AMMO ITEM")
        {
            //Lorenzo.GetInstance().items.Add(new Ammo(this));
            var item = Lorenzo.GetInstance().items.Find(i => i is Ammo);
            if (item == null && Lorenzo.GetInstance().items.Count < 6)
            {
                Lorenzo.GetInstance().items.Add(new Ammo(this));
            }
            else
            {
                item.quantity++;
            }
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "PAINKILLER ITEM")
        {
            //Lorenzo.GetInstance().items.Add(new PainKiller(this));
            var item = Lorenzo.GetInstance().items.Find(i => i is PainKiller);
            if (item == null && Lorenzo.GetInstance().items.Count < 6)
            {
                Lorenzo.GetInstance().items.Add(new PainKiller(this));
            }
            else
            {
                item.quantity++;
            }
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "DOUBLEDAMAGE ITEM")
        {
            //Lorenzo.GetInstance().items.Add(new DamageMultiplier(this));
            var item = Lorenzo.GetInstance().items.Find(i => i is DamageMultiplier);
            if (item == null && Lorenzo.GetInstance().items.Count < 6)
            {
                Lorenzo.GetInstance().items.Add(new DamageMultiplier(this));
            }
            else
            {
                item.quantity++;
            }

            Destroy(hit.gameObject);
        }
        //}

    }
}
