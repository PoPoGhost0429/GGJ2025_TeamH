using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    private static AudioController instance;

    public static AudioController Instance{
        get{
            if (instance == null){
                instance = FindObjectOfType<AudioController>();
                if (instance == null){
                    GameObject go = new GameObject("AudioPlayer");
                    instance = go.AddComponent<AudioController>();
                }
            }
            return instance;
        }
    }

    // Update is called once per frame
    public void PlaySound(int soundindex){
        AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClips[soundindex];
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
    }
}
