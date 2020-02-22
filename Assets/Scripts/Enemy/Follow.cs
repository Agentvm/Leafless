using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]Transform followed_object;
    [SerializeField]float movement_speed = 600f;

    // Components
    CharacterController controller;

    // variables
    [SerializeField]bool currently_following = true;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController> ();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !followed_object || !currently_following) return;
        
        // figure out move vector towards leaf
        Vector3 move_vector = followed_object.position - this.transform.position;
        move_vector.Normalize ();

        // move towards leaf
        controller.SimpleMove (move_vector * Time.deltaTime * movement_speed);
        this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z );

        // look to the leaf
        this.transform.LookAt (followed_object.position );
    }

    public void stopFollowing ()
    {
        currently_following = false;
    }

    public void startFollowing ()
    {
        currently_following = true;
    }
}
