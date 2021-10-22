using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public Boss boss;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        boss = new Boss();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = (float)boss.healthPoints / (float)boss.maxHealth;

        if (boss.healthPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
