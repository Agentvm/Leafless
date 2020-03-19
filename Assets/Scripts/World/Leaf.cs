using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    AudioSource audio_source;
    [SerializeField]AudioClip[] regrow_sounds;
    [SerializeField]AudioClip[] get_eaten_sounds;
    Growth GrowthScriptReference;

    //// Start is called before the first frame update
    void Start()
    {
        audio_source = this.GetComponent<AudioSource> ();
        GrowthScriptReference = this.transform.parent.parent.GetComponent<Growth> ();
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
        GrowthScriptReference.noticeEatenLeaf (this.transform);
        SceneLoader.Instance.Award = 1;
    }

    void play_audio (AudioClip clip )
    {
        audio_source.clip = clip;
        audio_source.Play ();
    }

    private void OnTriggerEnter ( Collider collision )
    {
        if (collision.transform.tag == "PlantBody" || collision.transform.tag == "Interactable")
        {
            GrowthScriptReference.destroyOneLeaf ();
            Destroy (this.transform.parent.gameObject);
        }
    }
}
