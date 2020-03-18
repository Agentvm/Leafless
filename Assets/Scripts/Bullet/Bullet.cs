using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool explosion_started = false;
    float start_time = 0f;
    float maximum_life_time = 10f;
    float explode_time = 0f;

    [SerializeField]Transform bullet_body;
    [SerializeField]ParticleSystem explode_particle;
    [SerializeField]ParticleSystem explode_flash;
    [SerializeField]TrailRenderer trail_renderer;

    [SerializeField]AudioClip[] shoot_sounds;

    private void Awake ()
    {
        this.GetComponent<AudioSource> ().clip = shoot_sounds[Random.Range (0, shoot_sounds.Length)];
    }

    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;
        this.GetComponent<AudioSource> ().Play ();
    }

    // Update is called once per frame
    void Update()
    {
        if ( explosion_started && !explode_particle.isPlaying || Time.time > start_time + maximum_life_time )
            Destroy (this.gameObject);
    }

    private void OnTriggerEnter ( Collider collider )
    {
        if ( collider.transform.tag == "Enemy")
            collider.transform.GetComponent<Enemy> ().Die ();

        if (collider.tag != "Player")
            explode_on_contact ();
    }

    public void explode_on_contact ()
    {
        // hold the bullet in the air and disable the bullet body as well as the collider
        this.GetComponent<Translate> ().stopTranslating ();
        bullet_body.gameObject.SetActive(false );
        this.GetComponent<Collider> ().enabled = false;

        // play the explosion animation, disable the trail
        explosion_started = true;
        explode_flash.Play ();
        explode_particle.Play ();
        //if ( !explode_time_started )
        //{
        //    
        //    explode_time = Time.time;
        //    explode_time_started = true;
        //}

        //if ( Time.time > explode_time + 2f ) ;
    }
}
