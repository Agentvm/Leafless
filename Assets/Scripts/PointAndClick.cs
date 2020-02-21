using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClick : MonoBehaviour
{
    // Components
    Animator animator;
    Camera main_camera;

    // raycasting
    private Vector3 mouse_point;
    RaycastHit ray_hit;
    Ray ray;

    public Vector3 Mouse_point { get => mouse_point;}

    // Start is called before the first frame update
    void Start()
    {
        // get components
        main_camera = Camera.main;
        animator = GetComponent<Animator> ();

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
        }

        // On Click
        if ( Input.GetMouseButton (0) )
        {
            animator.SetBool ("LeftMouse", true);
        }
        else animator.SetBool ("LeftMouse", false);
    }

}
