using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Components
    CharacterController controller;
    PointAndClick PointAndClickScriptReference;

    // Constants
    [SerializeField] float movement_speed = 80f;

    //
    Vector3 input;
    bool movement_disabled = false;

    public bool MovementDisabled { get => movement_disabled; set => movement_disabled = value; }

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
    }

    // Update is called once per frame
    void Update()
    {
        if ( MovementDisabled ) return;

        // Use input class please
        input.x = Input.GetAxis ("Horizontal");
        input.z = Input.GetAxis ("Vertical");

        // move according to input, but keep a height of 0
        controller.SimpleMove (input * Time.deltaTime * movement_speed );
        this.transform.position.Set (this.transform.position.x, 0f, this.transform.position.z);

        // look to the current mous position
        this.transform.LookAt (PointAndClickScriptReference.Mouse_point);
    }
}
