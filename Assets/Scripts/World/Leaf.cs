using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    AudioSource audio_source;
    [SerializeField]AudioClip[] regrow_sounds;
    [SerializeField]AudioClip[] get_eaten_sounds;

    //// Start is called before the first frame update
    void Start()
    {
        audio_source = this.GetComponent<AudioSource> ();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Grow ()
    {
        this.gameObject.SetActive (true);
        play_audio (regrow_sounds[Random.Range (0, regrow_sounds.Length)] );
        // play grow animation
    }

    public void getEaten ()
    {
        // play get eaten animation
        play_audio (get_eaten_sounds[Random.Range (0, get_eaten_sounds.Length)]);
        if ( this.transform.parent && this.transform.parent.parent )
            this.transform.parent.parent.GetComponent<Growth> ().noticeEatenLeaf (this.transform);
        SceneLoader.Instance.Award = 1;
    }

    void play_audio (AudioClip clip )
    {
        audio_source.clip = clip;
        audio_source.Play ();
    }
}
