using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputModule : MonoBehaviour
{
    // Static instance of InputModule which allows it to be accessed by any other script.
    public static InputModule Instance = null;

    // Debug
    [SerializeField]private TextMesh display_text;
    [SerializeField]private TextMesh display_text2;

    // References
    //PointAndClick PointAndClickScriptReference;
    Movement MovementScriptReference;

    // Input Variables
    private Vector3 user_input; // exposed to other scripts via Property
    private Vector2 touch_input; // exposed to other scripts via Property
    private bool touch_input_active = false;
   
    // Touch Variables
    //private Touch one_touch;
    private Vector2 touch_start_position;
    private string direction;
    private float time_touch_ended;

    // Touch Constants
    private float max_input_distance = 5f;
    private float display_time = 0.5f;

    // Mouse Point and Click
    private Vector3 mouse_point; // current mouse position
    //Transform clicked_interactable = null; // object clicked by mouse

    // Mouse Timing
    float last_click_time = 0f;
    float click_delay = 0.3f;

    // Raycasting
    Camera main_camera;
    RaycastHit ray_hit;
    Ray ray;

    // Properties
    public bool TouchInputActive { get => touch_input_active; }
    public Vector3 UserInput { get => user_input; }
    public Vector3 MousePoint { get => mouse_point; }
    //public Transform ClickedInteractable { get => clicked_interactable; }

    // Singleton
    void Awake ()
    {
        // check that there is only one instance of this and that it is not destroyed on load
        if ( Instance == null )
            Instance = this;
        else if ( Instance != this )
            Destroy (gameObject);
        DontDestroyOnLoad (gameObject);
    }

    // initiate
    private void Start ()
    {
        // Get References
        MovementScriptReference = GameObject.FindWithTag ("Player").GetComponent<Movement> ();
        //PointAndClickScriptReference = this.GetComponent<PointAndClick> ();
        main_camera = Camera.main;

        touch_input = new Vector2 (0f, 0f);
        if ( !display_text )
            display_text = this.transform.GetComponentInChildren<TextMesh> ();
    }

    // Update is called once per frame
    void Update()
    {
        // Touch input
        if ( Input.touchCount > 0 )
        {
            // Get touch information
            touch_input_active = true;
            touchAndTouch ();
        }
        // Mouse Input Active
        else if (touch_input.magnitude == 0)
        {
            // Disable Touch
            touch_input_active = false;
            if ( Time.time > time_touch_ended + display_time  && display_text && display_text2)
            {
                display_text.text = "No Touch Input"; // Debug
                display_text2.text = "No Touch Input"; // Debug
            }

            // Mouse Behaviour
            mousePointAndClick ();
        }

        // Set actual input values for external scripts
        if ( TouchInputActive )
            user_input.Set (touch_input.x, 0f, touch_input.y);
        else
            user_input.Set (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));
    }

    // Handle touch input
    void touchAndTouch ()
    {
        // Iterate through all touches
        for (int i = 0; i < Input.touchCount; i++ )
        {
            Touch touch = Input.GetTouch (i);

            // First Touch
            if ( i == 0 )
            {
                // Debug Text
                if (display_text)
                    display_text.text = touch.phase.ToString ();

                // On first sensing the touch
                if ( touch.phase == TouchPhase.Began )
                    touch_start_position = touch.position;

                // On swiping
                else if ( touch.phase == TouchPhase.Moved )
                    setTouchInput (touch);

                // When the touch contact is ending
                else if ( touch.phase == TouchPhase.Ended )
                {
                    // Check if tapped
                    if ( touch_start_position == touch.position )
                    {
                        Debug.Log ("Tapped");

                        // Raycast
                        Transform active_object_transform = Raycast (Input.mousePosition);

                        // Clicked on object, or in the air?
                        if ( active_object_transform && active_object_transform.tag == "Interactable" )
                        {
                            Debug.Log ("1st Touch Interactable Clicked");
                            MovementScriptReference.leafClicked (active_object_transform);
                        }

                        else // Shoot, if no Interactable object was clicked
                        {
                            Debug.Log (" 1st Touch Empty Clicked");
                            MovementScriptReference.tryStartShooting ();
                        }
                    }
                    else // if it was a swipe that just ended
                    {
                        touch_input.x = 0f;
                        touch_input.y = 0f;
                    }

                    time_touch_ended = Time.time;

                }
            }

            // Second Touch
            else if ( i == 1 )
            {
                // Debug Text
                if (display_text2)
                    display_text2.text = touch.phase.ToString ();

                // On first sensing the touch
                if ( touch.phase == TouchPhase.Began )
                    touch_start_position = touch.position;

                // On swiping
                else if ( touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary )
                {
                    mouse_point = touch.position;
                    mouse_point.y = 0.0f;
                }

                // When the touch contact is ending
                else if ( touch.phase == TouchPhase.Ended )
                {
                    // Check if tapped
                    if ( touch_start_position == touch.position )
                    {
                        Debug.Log ("2nd Tapped");

                        // Raycast
                        Transform active_object_transform = Raycast (Input.mousePosition);

                        // Clicked on object, or in the air?
                        if ( active_object_transform && active_object_transform.tag == "Interactable" )
                        {
                            Debug.Log ("2nd Touch Interactable Clicked");
                            MovementScriptReference.leafClicked (active_object_transform);
                        }

                        else // Shoot, if no Interactable object was clicked
                        {
                            Debug.Log (" 2nd Touch Empty Clicked");
                            MovementScriptReference.tryStartShooting ();
                        }
                    }

                }
            }
        }
        
    }

    void setTouchInput (Touch touch)
    {
        // get the magnitude of the swipe based on the distance to the start
        float touch_distance = (touch.position - touch_start_position).magnitude;
        float magnitude = Mathf.Min (max_input_distance, touch_distance) / max_input_distance;

        // get the direction of the swipe and combine it with the magnitude
        Vector2 movement_direction = (touch.position - touch_start_position).normalized;
        touch_input = movement_direction * magnitude;
    }

    void mousePointAndClick ()
    {
        // Shoot a ray from the camera through the mouse position into the scene and get the collision point
        // Also get the object that was hit
        Transform active_object_transform = Raycast (Input.mousePosition);

        // Mouse clicked?
        if ( Input.GetMouseButton (0) && Time.time > last_click_time + click_delay )
        {
            last_click_time = Time.time;

            // Clicked on object, or in the air?
            if ( active_object_transform && active_object_transform.tag == "Interactable" )
                MovementScriptReference.leafClicked (active_object_transform);
            else // Shoot, if no Interactable object was clicked
                MovementScriptReference.tryStartShooting ();
        }
    }

    // Do a raycast from the main camera to the specified position, set the mouse position and return the object hit
    Transform Raycast (Vector3 point)
    {
        ray = main_camera.ScreenPointToRay (point);
        if ( Physics.Raycast (ray, out ray_hit) )
        {
            mouse_point = ray_hit.point;
            mouse_point.y = 0.0f;
            return ray_hit.transform;
        }
        else return null;
    }
}
