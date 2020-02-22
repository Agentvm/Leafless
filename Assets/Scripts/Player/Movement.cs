using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    enum State {WasdMovement, ApproachLeaf, EatLeaf}
    State state;

    // References
    PointAndClick PointAndClickScriptReference;
    Shoot ShootScriptReference;

    // Components
    CharacterController controller;
    Animator animator;

    // Constants
    [Range(0f, 1600f)][SerializeField] float movement_speed = 800f;
    [Range(0f, 6f)][SerializeField] float leaf_distance = 4f;

    //
    Vector3 input;
    Transform leaf_to_approach = null;
    
    //bool movement_disabled = false;
    float time_start_of_leaf_eating = 0f;
    float leaf_eat_delay = 1.1f;
    
    //public bool MovementDisabled { get => movement_disabled; set => movement_disabled = value; }

    
    // Damping variables


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
        //if ( MovementDisabled ) return;

        // Use input class please
        input.x = Input.GetAxis ("Horizontal");
        input.z = Input.GetAxis ("Vertical");
        input.Normalize ();

        if (state == State.WasdMovement)
        {
            if ( PointAndClickScriptReference.ClickedInteractable != null )
            {
                leaf_to_approach = PointAndClickScriptReference.ClickedInteractable;
                state = State.ApproachLeaf;
            }

            // move according to input, but keep a height of 0
            controller.SimpleMove (input * Time.deltaTime * movement_speed);
            this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z);

            // look to the current mouse position
            this.transform.LookAt (PointAndClickScriptReference.Mouse_point);
        }
        else if ( state == State.ApproachLeaf )
        {
            // figure out leaf position
            Vector3 modified_leaf_position = leaf_to_approach.position;

            // fix this by changing the model origin
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
                controller.SimpleMove (move_vector * Time.deltaTime * movement_speed);
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
                leaf_to_approach.gameObject.SetActive (false); // grow script should link in here
                leaf_to_approach = null;
                ShootScriptReference.Ammunition += 1;
                animator.SetBool ("Eating", false);
                state = State.WasdMovement;
            }
        }

        
    }

    
    void changeState ()
    {
    
    }

}
