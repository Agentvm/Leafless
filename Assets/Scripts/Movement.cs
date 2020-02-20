using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Components
    CharacterController controller;

    // Constants
    [SerializeField] float movement_speed = 8f;

    //
    Vector3 input;

    // Damping variables

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        controller = GetComponent<CharacterController> ();

        // no input
        input.x = 0f;
        input.y = 0f;
        input.z = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Use input class please
        input.x = Input.GetAxis ("Horizontal");
        input.z = Input.GetAxis ("Vertical");

        controller.Move (input * Time.deltaTime * movement_speed );
    }
}
