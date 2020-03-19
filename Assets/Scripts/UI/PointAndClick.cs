using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Actually, this should be global
public class PointAndClick : MonoBehaviour
{
    // Components
    Animator animator;
    Camera main_camera;
    Movement MovementScriptReference;
    Shoot ShootScriptReference;

    // raycasting
    private Vector3 mouse_point;
    RaycastHit ray_hit;
    Ray ray;

    // object identification
    Transform active_object_transform;
    Transform clicked_interactable = null; // object clicked by player

    // timing
    float last_click_time = 0f;
    float click_delay = 0.3f;

    public Vector3 Mouse_point { get => mouse_point; }
    public Transform ClickedInteractable { get => clicked_interactable; set => clicked_interactable = value; }

    // Start is called before the first frame update
    void Start()
    {
        // get components
        main_camera = Camera.main;
        animator = GetComponent<Animator> ();
        MovementScriptReference = GetComponent<Movement> ();
        ShootScriptReference = GetComponent<Shoot> ();

        // zero
        mouse_point.x = 0;
        mouse_point.y = 0;
        mouse_point.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Shoot a ray from the camera through the mouse position into the scene and get the collision point
        // Also get the object that was hit
        ray = main_camera.ScreenPointToRay (Input.mousePosition);
        if ( Physics.Raycast (ray, out ray_hit) )
        {
            mouse_point = ray_hit.point;
            mouse_point.y = 0.0f;
            active_object_transform = ray_hit.transform;
        }
        else active_object_transform = null;

        // Mouse clicked?
        if ( Input.GetMouseButton (0) && Time.time > last_click_time + click_delay)
        {
            last_click_time = Time.time;

            // Clicked on object, or in the air?
            if ( active_object_transform && active_object_transform.tag == "Interactable" )
            {
                ShootScriptReference.disableShooting ();

                ClickedInteractable = active_object_transform;
                MovementScriptReference.leafClicked (active_object_transform);
            }
            else // Shoot, if no Interactable object was clicked
            {
                ClickedInteractable = null;
                ShootScriptReference.startShooting (true); // force shooting
            }   
        }

    }

}
