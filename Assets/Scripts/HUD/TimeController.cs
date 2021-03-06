using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // Start is called before the first frame update
    public static float currentTime;
    public TMPro.TextMeshProUGUI timeText;

    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timeText.text = time.ToString("mm\\:ss");
    }
}
