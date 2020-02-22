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

    // raycasting
    private Vector3 mouse_point;
    RaycastHit ray_hit;
    Ray ray;

    //
    Transform active_object_transform;
    Transform clicked_interactable = null; // object clicked by player

    public Vector3 Mouse_point { get => mouse_point; }
    public Transform ClickedInteractable { get => clicked_interactable; set => clicked_interactable = value; }

    // Start is called before the first frame update
    void Start()
    {
        // get components
        main_camera = Camera.main;
        animator = GetComponent<Animator> ();
        MovementScriptReference = GetComponent<Movement> ();

        // zero
        mouse_point.x = 0;
        mouse_point.y = 0;
        mouse_point.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Shoot a ray from the camera through the mouse position into the scene and get the collision point
        ray = main_camera.ScreenPointToRay (Input.mousePosition);
        if ( Physics.Raycast (ray, out ray_hit) )
        {
            mouse_point = ray_hit.point;
            mouse_point.y = 0.5f;
            active_object_transform = ray_hit.transform;
        }
        else active_object_transform = null;

        // If leaf is clicked
        if ( Input.GetMouseButton (0) && active_object_transform.tag == "Interactable" )
        {
            ClickedInteractable = active_object_transform;
        }
        else ClickedInteractable = null;
    }

}
