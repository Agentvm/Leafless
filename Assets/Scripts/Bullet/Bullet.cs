using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool explode_time_started = false;
    float explode_time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter ( Collider collider )
    {
        if ( collider.transform.tag == "Enemy")
            collider.transform.GetComponent<Enemy> ().Die ();

        explode_on_contact ();
    }

    public void explode_on_contact ()
    {
        //if ( !explode_time_started )
        //{
        //    // hold the bullet in the air and play the explosion animation
        //    this.transform.parent.GetComponent<Translate> ().stopTranslating ();
        //    explode_time = Time.time;
        //    explode_time_started = true;
        //}

        //if ( Time.time > explode_time + 2f ) ;
    }
}
