﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClick : MonoBehaviour
{
    // Components
    Animator animator;
    Camera main_camera;
    Eat EatScriptReference;

    // raycasting
    private Vector3 mouse_point;
    RaycastHit ray_hit;
    Ray ray;

    //
    Transform active_object_transform;

    public Vector3 Mouse_point { get => mouse_point;}

    // Start is called before the first frame update
    void Start()
    {
        // get components
        main_camera = Camera.main;
        animator = GetComponent<Animator> ();
        EatScriptReference = GetComponent<Eat> ();

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

            EatScriptReference.eatLeaf (active_object_transform );

            
        }
    }

}
