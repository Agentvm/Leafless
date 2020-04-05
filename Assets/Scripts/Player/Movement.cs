using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Statemachine
    enum State {FreeMovement, ApproachLeaf, EatLeaf, Shoot}
    State state;

    // References
    //PointAndClick PointAndClickScriptReference;

    // Components
    CharacterController controller;
    Animator animator;

    // Movement
    bool movement_disabled = false;

        // Constants
        [Range(0f, 1600f)][SerializeField] float max_movement_speed = 800f;
        [Range(0f, 6f)][SerializeField] float leaf_distance = 4f;
        [Range(1f, 10f)][SerializeField] float acceleration = 4f;
        [Range(1f, 10f)][SerializeField] float rotation_acceleration = 6f;

    float current_movement_speed = 0f;

    // Damping variables
    float damp_movement = 0f;

    // Shoot
    Object bullet_object;
    [SerializeField]int ammunition_capacity = 3;
    [SerializeField]int ammunition = 0;
    float shoot_time = 0f;
    float shoot_delay = .5f * (1f/1.5f);

    // Interaction with objects
    Transform leaf_to_approach = null;
    float time_start_of_leaf_eating = 0f;
    float leaf_eat_delay = 1.1f;
    
    // Properties
    public bool MovementDisabled { get => movement_disabled; set => movement_disabled = value; }
    public int Ammunition { get => ammunition; set => ammunition = Mathf.Min (value, ammunition_capacity); }


    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        controller = GetComponent<CharacterController> ();
        animator = GetComponent<Animator> ();

        // Get References
        //PointAndClickScriptReference = GetComponent<PointAndClick> ();

        // Preload bullet object
        bullet_object = Resources.Load ("Bullet");

        // set initial state
        state = State.FreeMovement;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log ("Intensity: " + GameState.Instance.GameIntensity);

        if ( MovementDisabled ) return;

        if (state == State.FreeMovement)
        {
            if ( leaf_to_approach != null )
                changeState (State.ApproachLeaf );

            Move ();
        }
        else if ( state == State.ApproachLeaf )
        {
            if ( Mathf.Abs (Input.GetAxis ("Horizontal" )) > .5f || Mathf.Abs (Input.GetAxis ("Vertical" )) > .5f )
            {
                changeState (State.FreeMovement);
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
                changeState (State.EatLeaf );
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
            // Stop the leaf eating "animation"
            if ( Time.time > time_start_of_leaf_eating + leaf_eat_delay / 2 )
                leaf_to_approach.transform.GetComponent<Leaf> ().gameObject.SetActive (false);

            // play eat animation, then change state
            if ( Time.time > time_start_of_leaf_eating + leaf_eat_delay )
            {
                //leaf_to_approach.transform.GetComponent<Leaf> ().gameObject.SetActive (false);
                leaf_to_approach = null;
                Ammunition += 1;
                animator.SetBool ("Eating", false);
                changeState (State.FreeMovement);
            }
        }
        else if (state == State.Shoot)
        {
            // Make speed changes less fast
            Move (0.5f, 0.4f );
            
            // at the right time in the animation, instantiate the bullet
            if ( Time.time > shoot_time + shoot_delay )
            {
                Ammunition -= 1;
                Quaternion shoot_rotation = this.transform.rotation;
                shoot_rotation.eulerAngles = new Vector3 (0f, shoot_rotation.eulerAngles.y, 0f);
                Instantiate (bullet_object, this.transform.position + this.transform.forward * .5f, shoot_rotation);
                animator.SetBool ("Shooting", false);
                changeState (State.FreeMovement);
            }
        }

    }

    void Move ( float acceleration_modifier = 1f, float rotation_acceleration_modifier = 1f )
    {
        // gather speed // a max speed of 1f ensures that character does not move faster in diagon alley
        dampedAccelerate (Mathf.Min (InputModule.Instance.UserInput.magnitude, 1f) * max_movement_speed, acceleration_modifier );

        // move according to input, but keep a height of 0
        controller.SimpleMove (InputModule.Instance.UserInput.normalized * Time.deltaTime * current_movement_speed);
        this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z);

        // turn towards current mouse position
        Quaternion look_rotation;
        //if ( InputModule.Instance.TouchInputActive )
        //    look_rotation = Quaternion.LookRotation (InputModule.Instance.UserInput.normalized);
        //else
        look_rotation = Quaternion.LookRotation (InputModule.Instance.MousePoint - this.transform.position);

        transform.rotation = Quaternion.Slerp (transform.rotation, look_rotation, Time.deltaTime * rotation_acceleration * rotation_acceleration_modifier );
    }

    void dampedAccelerate (float speed_to_be, float acceleration_modifier = 1f)
    {
        current_movement_speed = Mathf.SmoothDamp (current_movement_speed, speed_to_be, ref damp_movement, acceleration * acceleration_modifier * Time.deltaTime);
    }
    
    // this is the place for state entry instructions
    void changeState ( State new_state)
    {
        if (new_state == State.EatLeaf)
            leaf_to_approach.transform.GetComponent<Leaf> ().getEaten ();
        else if (new_state == State.Shoot)
        {
            animator.SetBool ("Shooting", true);
            shoot_time = Time.time;
        }
        //Debug.Log ("Changing State to: " + new_state);
        state = new_state;
    }

    public void leafClicked (Transform clicked_leaf)
    {
        if ( InputModule.Instance.TouchInputActive )
        {
            Debug.Log ("Eating Triggered through Touch, state: " + state);
        }

        if ( state != State.ApproachLeaf && state != State.EatLeaf )
            leaf_to_approach = clicked_leaf;
    }

    public void tryStartShooting ( bool forced = false )
    {
        if ( InputModule.Instance.TouchInputActive )
        {
            Debug.Log ("Shooting Triggered through Touch, state: " + state);
        }

        if ( state == State.FreeMovement &&
             Ammunition > 0 &&
             Time.time > shoot_time + shoot_delay * 2 )
        {
            changeState (State.Shoot);
        }
    }

}
