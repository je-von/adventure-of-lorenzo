using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementDialogueTrigger : MonoBehaviour
{
    public GameObject character;
    public DialogueController dc;
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Lorenzo.GetInstance().isInBasement && Vector3.Distance(character.transform.position, transform.position) <= 1f)
        {
            Lorenzo.GetInstance().isInBasement = true;
            dc.StartDialogue(dialogue);
            character.GetComponent<Animator>().SetBool("isRunning", false);
        }
    }
}
