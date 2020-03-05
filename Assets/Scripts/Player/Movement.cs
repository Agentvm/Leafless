using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Statemachine
    enum State {WasdMovement, ApproachLeaf, EatLeaf}
    State state;

    // References
    PointAndClick PointAndClickScriptReference;
    Shoot ShootScriptReference;

    // Components
    CharacterController controller;
    Animator animator;

    // Movement
    Vector3 input;
    bool movement_disabled = false;

        // Constants
        [Range(0f, 1600f)][SerializeField] float max_movement_speed = 800f;
        [Range(0f, 6f)][SerializeField] float leaf_distance = 4f;
        [Range(1f, 10f)][SerializeField] float acceleration = 4f;
        [Range(1f, 10f)][SerializeField] float rotation_acceleration = 6f;

    float current_movement_speed = 0f;

    // Damping variables
    float damp_movement = 0f;

    // Interaction with objects
    Transform leaf_to_approach = null;
    float time_start_of_leaf_eating = 0f;
    float leaf_eat_delay = 1.1f;
    
    // Properties
    public bool MovementDisabled { get => movement_disabled; set => movement_disabled = value; }


    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        controller = GetComponent<CharacterController> ();
        animator = GetComponent<Animator> ();

        PointAndClickScriptReference = GetComponent<PointAndClick> ();
        ShootScriptReference = GetComponent<Shoot> ();

        // no input
        input.x = 0f;
        input.y = 0f;
        input.z = 0f;

        // state
        state = State.WasdMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if ( MovementDisabled ) return;

        // Use input class please
        input.x = Input.GetAxis ("Horizontal");
        input.z = Input.GetAxis ("Vertical");
        //input.Normalize ();

        if (state == State.WasdMovement)
        {
            if ( leaf_to_approach != null )
                state = State.ApproachLeaf;

            // gather speed // a max speed of 1f ensures that character does not move faster in diagon alley
            dampedAccelerate (Mathf.Min (input.magnitude, 1f) * max_movement_speed); 

            // move according to input, but keep a height of 0
            controller.SimpleMove (input.normalized * Time.deltaTime * current_movement_speed);
            this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z);

            // turn towards current mouse position
            Quaternion look_rotation = Quaternion.LookRotation (PointAndClickScriptReference.Mouse_point - this.transform.position);
            transform.rotation = Quaternion.Slerp (transform.rotation, look_rotation, Time.deltaTime * rotation_acceleration);

            // look to the current mouse position
            //this.transform.LookAt (PointAndClickScriptReference.Mouse_point);
        }
        else if ( state == State.ApproachLeaf )
        {
            if ( Mathf.Abs (Input.GetAxis ("Horizontal" )) > .5f || Mathf.Abs (Input.GetAxis ("Vertical" )) > .5f )
            {
                state = State.WasdMovement;
                leaf_to_approach = null;
                return;
            }

            // figure out leaf position
            Vector3 modified_leaf_position = leaf_to_approach.position;

            // fix this by changing the model origin // we only need this once
            if ( leaf_to_approach.parent )
            {
                modified_leaf_position = leaf_to_approach.parent.position;
                modified_leaf_position += leaf_distance * leaf_to_approach.parent.forward;
                modified_leaf_position.y = this.transform.position.y; // set on same height, so goal can always be reached by walking the ground
            }

            if (Vector3.Distance (this.transform.position, modified_leaf_position) < .5f )
            {
                animator.SetBool ("Eating", true);
                state = State.EatLeaf;
                time_start_of_leaf_eating = Time.time;
                return;
            }
            else
            {
                // figure out move vector towards leaf
                Vector3 move_vector = modified_leaf_position - this.transform.position;
                move_vector.Normalize ();

                // move towards leaf
                dampedAccelerate (max_movement_speed);
                controller.SimpleMove (move_vector * Time.deltaTime * current_movement_speed);
                this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z);
            }

            // look to the leaf
            if ( leaf_to_approach ) this.transform.LookAt (leaf_to_approach.position );
        }
        else if (state == State.EatLeaf)
        {
            // play eat animation, then change state
            if ( Time.time > time_start_of_leaf_eating + leaf_eat_delay )
            {
                leaf_to_approach.transform.GetComponent<Leaf> ().getEaten ();
                leaf_to_approach = null;
                ShootScriptReference.Ammunition += 1;
                animator.SetBool ("Eating", false);
                ShootScriptReference.enableShooting ();
                state = State.WasdMovement;
            }
        }

        
    }

    void dampedAccelerate (float speed_to_be)
    {
        current_movement_speed = Mathf.SmoothDamp (current_movement_speed, speed_to_be, ref damp_movement, acceleration * Time.deltaTime);
    }
    
    void changeState ()
    {
    
    }

    public void leafClicked (Transform clicked_leaf)
    {
        if ( state != State.ApproachLeaf && state != State.EatLeaf )
            leaf_to_approach = clicked_leaf;
    }

}
