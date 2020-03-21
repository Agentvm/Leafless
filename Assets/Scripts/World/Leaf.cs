using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    // Components
    AudioSource audio_source;
    [SerializeField]AudioClip[] regrow_sounds;
    [SerializeField]AudioClip[] get_eaten_sounds;
    Renderer renderer;
    Rigidbody rigid_body;

    // References
    Growth GrowthScriptReference;

    // Transform
    Vector3 original_position;
    Quaternion original_rotation;
    Vector3 original_scale;

    //// Start is called before the first frame update
    void Start()
    {
        // Get Components
        audio_source = this.GetComponent<AudioSource> ();
        renderer = this.GetComponent<Renderer> ();
        rigid_body = this.GetComponent<Rigidbody> ();

        // Get References
        GrowthScriptReference = this.transform.parent.parent.GetComponent<Growth> ();

        // save original transform values
        original_position = this.transform.position;
        original_rotation = this.transform.rotation;
        original_scale = this.transform.localScale;
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void reGrow ()
    {
        // reset transform
        this.transform.position = original_position;
        this.transform.rotation = original_rotation;
        rigid_body.isKinematic = false;

        // activate object and play sound
        this.gameObject.SetActive (true);
        play_audio (regrow_sounds[Random.Range (0, regrow_sounds.Length)] );

        // play grow animation
        StopAllCoroutines ();
        StartCoroutine (growCoroutine());
    }

    IEnumerator growCoroutine ()
    {
        for (float strength = 0; strength < 1; strength += 0.01f )
        {
            // change alpha
            //Color color = renderer.material.color;
            //color.a = strength;

            // change size
            this.transform.localScale = original_scale * strength;

            yield return new WaitForSeconds (.1f);
        }
    }

    public void getEaten ()
    {
        // play get eaten animation
        rigid_body.isKinematic = true;
        StopAllCoroutines ();
        StartCoroutine (getEatenCoroutine ());

        // audio
        play_audio (get_eaten_sounds[Random.Range (0, get_eaten_sounds.Length)]);

        // logic
        GrowthScriptReference.noticeEatenLeaf (this.transform);
        SceneLoader.Instance.Award = 1;
    }

    IEnumerator getEatenCoroutine ()
    {
        for ( float strength = 1; strength > 0; strength -= 0.01f )
        {
            // change alpha
            //renderer.material.color *= strength;

            // change size
            this.transform.localScale *= strength;
            this.transform.Translate (this.transform.forward * 15f * Time.deltaTime );

            yield return new WaitForSeconds (.02f);
        }
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
