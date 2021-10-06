using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreItemTextController : MonoBehaviour
{
    // Start is called before the first frame update
    private TMPro.TextMeshProUGUI coreItemText;
    void Start()
    {
        coreItemText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        coreItemText.text = "CORE ITEMS " + Lorenzo.GetInstance().coreItemCount + "/9";
    }
}
