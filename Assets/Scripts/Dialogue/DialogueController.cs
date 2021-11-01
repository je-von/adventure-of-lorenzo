using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public GameObject HUD;

    public Animator animator;

    public TMPro.TextMeshProUGUI nameTxt, contentTxt;

    private Queue<string> sentences = new Queue<string>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        HUD.SetActive(false);
        animator.SetBool("isOpen", true);
        //Debug.Log(dialogue.name + " lagi ngmg");
        nameTxt.text = dialogue.name;
        sentences.Clear();

        foreach(string s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //Debug.Log(sentence);
        //contentTxt.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypingEffect(sentence));
    }

    IEnumerator TypingEffect(string sentence)
    {
        contentTxt.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            contentTxt.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        
        animator.SetBool("isOpen", false);
        HUD.SetActive(true);
        Debug.Log("ngomong selesai");
    }
}
