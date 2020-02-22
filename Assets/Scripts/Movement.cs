using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    enum State {WasdMovement, EatLeaf}
    State state;

    // Components
    CharacterController controller;
    PointAndClick PointAndClickScriptReference;

    // Constants
    [SerializeField] float movement_speed = 80f;

    //
    Vector3 input;
    Transform clicked_interactable = null; // object clicked by player
    //bool movement_disabled = false;

    public Transform ClickedInteractable { get => clicked_interactable; set => clicked_interactable = value; }
    //public bool MovementDisabled { get => movement_disabled; set => movement_disabled = value; }

    
    // Damping variables


    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        controller = GetComponent<CharacterController> ();
        PointAndClickScriptReference = GetComponent<PointAndClick> ();

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

        if (state == State.WasdMovement)
        {
            if ( ClickedInteractable != null ) state = State.EatLeaf;

            // move according to input, but keep a height of 0
            controller.SimpleMove (input * Time.deltaTime * movement_speed);
            this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z);

            // look to the current mous position
            this.transform.LookAt (PointAndClickScriptReference.Mouse_point);
        }
        else if ( state == State.EatLeaf )
        {
            if (false ) // if done with state
            {
                ClickedInteractable = null;
            }

            // move towards leaf
            Vector3 move_vector = ClickedInteractable.position - this.transform.position;
            move_vector.Normalize ();

            controller.SimpleMove (move_vector * Time.deltaTime * movement_speed);
            this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z);

            // look to the current mous position
            this.transform.LookAt (ClickedInteractable.position );
        }

        
    }

    
    void changeState ()
    {
    
    }

}
