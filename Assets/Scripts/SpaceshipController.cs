using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public GameObject player;
    public TMPro.TextMeshProUGUI overrideText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        if (Vector3.Distance(player.transform.position, transform.position) <= 7f)
        {
            //Debug.Log("masuk");
            overrideText.text = "Press F";
            overrideText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                player.GetComponent<LorenzoController>().ShowVictoryMenu();
            }
        }
        else
        {
            overrideText.gameObject.SetActive(false);
        }
    }
}
