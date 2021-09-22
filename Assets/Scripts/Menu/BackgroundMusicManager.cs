using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource src;
    public AudioClip[] clips;
    void Start()
    {
        StartCoroutine(playMusic());
    }
    IEnumerator playMusic()
    {
        yield return null;
        int i = 0;
        while(true)
        {
            if (i >= clips.Length) i = 0;
            src.clip = clips[i];
            src.Play();
            while (src.isPlaying)
            {
                yield return null;
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
