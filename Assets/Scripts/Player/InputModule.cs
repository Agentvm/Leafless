using System.Collections.Generic;
using UnityEngine;
using Leafless.UI;


public class InputModule : MonoBehaviour
{
    // Static instance of InputModule which allows it to be accessed by any other script.
    public static InputModule Instance = null;

    // Debug
    [SerializeField] private TextMesh display_text;
    [SerializeField] private TextMesh display_text2;

    // Input Variables
    private Vector3 user_input; // exposed to other scripts via Property
    private Vector2 touch_input; // exposed to other scripts via Property
    private bool touch_input_active = false;

    // Configuration
    [Range(0f, 30f)][SerializeField] private float tap_threshold = 15f;

    // Touch Variables
    private Vector2 touch_start_position, touch2_start_position;
    private string direction;
    private float time_touch_ended;
    private List<int> known_touch_ids = new List<int>();
    float time_no_touch_since = 0f;
    float touch_time_delay = 1.2f;

    // Touch Constants
    private float max_input_distance = 5f;
    private float display_time = 0.5f;

    // Mouse Point and Click
    private Vector3 mouse_point = new Vector3(10f, 0f, 0f); // current mouse position

    // Mouse Timing
    float last_click_time = 0f;
    float click_delay = 0.3f;

    // Raycasting

    RaycastHit ray_hit;
    Ray ray;

    // Properties
    public bool TouchInputActive { get => touch_input_active; }
    public Vector3 UserInput { get => user_input; }
    public Vector3 MousePoint { get => mouse_point; }


    // Chached Properties
    #region CachedProperties
    private Camera _mainCamera;
    public Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            return _mainCamera;
        }
    }

    private Movement _movementScriptReference;
    public Movement MovementScriptReference
    {
        get
        {
            if (_movementScriptReference == null)
                _movementScriptReference = GameObject.FindObjectOfType<Movement>(); // Player Movement

            return _movementScriptReference;
        }
    }
    #endregion

    // Singleton
    void Awake()
    {
        // check that there is only one instance of this and that it is not destroyed on load
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // initiate
    private void OnEnable()
    {
        touch_input = new Vector2(0f, 0f);
        if (!display_text)
            display_text = this.transform.GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneLoader.Instance.CurrentScene == SceneIdentifiers.Menu) return;

        // Touch input
        if (Input.touchCount > 0/* || InputHelper.GetTouches ().Count > 0*/)
        {
            // Get touch information
            touch_input_active = true;
            time_no_touch_since = Time.time;
            touchAndTouch();
        }
        // Mouse Input Active
        else if (Time.time > time_no_touch_since + touch_time_delay)
        {
            if (Input.touchCount == 0/*  || InputHelper.GetTouches ().Count == 0*/ )
            {
                touch_input.x = 0f;
                touch_input.y = 0f;
            }

            // Disable Touch
            touch_input_active = false;
            if (Time.time > time_touch_ended + display_time && display_text && display_text2)
            {
                display_text.text = "No Touch Input"; // Debug
                display_text2.text = "No Touch Input"; // Debug
            }

            // Mouse Behaviour
            mousePointAndClick();
        }

        // Set actual input values for external scripts
        if (TouchInputActive)
            user_input.Set(touch_input.x, 0f, touch_input.y);
        else
            user_input.Set(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    // Handle touch input
    void touchAndTouch()
    {
        List<int> current_touch_ids = new List<int>();

        // Iterate through all touches
        foreach (Touch touch in Input.touches/*InputHelper.GetTouches ()*/ )
        {
            current_touch_ids.Add(touch.fingerId);

            // First Touch
            if (known_touch_ids.Count == 0 || (known_touch_ids.Count > 0 && touch.fingerId == known_touch_ids[0]))
            {
                // Debug Text
                if (display_text)
                    display_text.text = touch.phase.ToString();

                // On first sensing the touch
                if (touch.phase == TouchPhase.Began)
                    touch_start_position = touch.position;

                // On swiping
                else if (touch.phase == TouchPhase.Moved && (touch_start_position - touch.position).magnitude >= tap_threshold)
                    setTouchInput(touch);

                // When the touch contact is ending
                else if (touch.phase == TouchPhase.Ended)
                {
                    // Check if tapped
                    if ((touch_start_position - touch.position).magnitude < tap_threshold)
                    {
                        // Raycast at touch position
                        Transform active_object_transform = Raycast(touch.position);

                        // Clicked on object, or in the air?
                        if (active_object_transform && active_object_transform.tag == "Interactable")
                        {
                            Debug.Log("1st Tapped: Interactable");
                            MovementScriptReference.leafClicked(active_object_transform);
                        }
                        else // Shoot, if no Interactable object was clicked
                        {
                            Debug.Log("1st Tapped: Empty");
                            MovementScriptReference.tryStartShooting();
                        }
                    }
                    else // if it was a swipe that just ended
                    {
                        touch_input.x = 0f;
                        touch_input.y = 0f;
                    }

                    // At the end of touch, remove it's ID from the list of known ID's
                    known_touch_ids.Remove(touch.fingerId);
                    time_touch_ended = Time.time;

                }
            }

            // Any other Touch
            else if ((known_touch_ids.Count > 0 && touch.fingerId != known_touch_ids[0]))
            {
                // Debug Text
                if (display_text2)
                    display_text2.text = touch.phase.ToString();

                // On first sensing the touch
                if (touch.phase == TouchPhase.Began)
                    touch2_start_position = touch.position;

                //// On swiping
                //else if ( touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary )
                //{
                //    mouse_point = touch.position;
                //    mouse_point.y = MovementScriptReference.transform.position.y;
                //}

                // When the touch contact is ending
                else if (touch.phase == TouchPhase.Ended)
                {
                    // Check if tapped
                    if ((touch2_start_position - touch.position).magnitude < tap_threshold)
                    {
                        // Raycast at touch position
                        Transform active_object_transform = Raycast(Input.mousePosition);

                        // Clicked on object, or in the air?
                        if (active_object_transform && active_object_transform.tag == "Interactable")
                        {
                            Debug.Log("2nd Tapped: Interactable");
                            MovementScriptReference.leafClicked(active_object_transform);
                        }
                        else // Shoot, if no Interactable object was clicked
                        {
                            Debug.Log("2nd Tapped: Empty");
                            MovementScriptReference.tryStartShooting();
                        }
                    }

                    // At the end of touch, remove it's ID from the list of known ID's
                    known_touch_ids.Remove(touch.fingerId);

                }
            }

            // add unknown touch id to the list of known ids
            if (!known_touch_ids.Contains(touch.fingerId))
                known_touch_ids.Add(touch.fingerId);

        }

        // Debug
        if (Time.time % 5f > -0.02f && Time.time % 5f < 0.02f)
        {
            string ids_string = "";
            foreach (int id in current_touch_ids)
                ids_string += id + ", ";
            Debug.Log("Current Touch ID's: " + ids_string);

            ids_string = "";
            foreach (int id in known_touch_ids)
                ids_string += id + ", ";
            Debug.Log("Known Touch ID's: " + ids_string);
        }

        // after all touches have been treated, delete finger touch id's that are no longer viable
        for (int i = 0; i < known_touch_ids.Count; i++)
        {
            if (!current_touch_ids.Contains(known_touch_ids[i]))
            {
                known_touch_ids.RemoveAt(i);
                i--;
            }
        }
    }

    void setTouchInput(Touch touch)
    {
        // get the magnitude of the swipe based on the distance to the start
        Vector2 touch_vector = (touch.position - touch_start_position);
        Vector3 touch_vector_3 = new Vector3(touch_vector.x, 0f, touch_vector.y);
        float touch_distance = touch_vector.magnitude;
        float magnitude = Mathf.Min(max_input_distance, touch_distance) / max_input_distance;

        mouse_point = MovementScriptReference.transform.position + touch_vector_3.normalized;
        mouse_point.y = MovementScriptReference.transform.position.y;

        // get the direction of the swipe and combine it with the magnitude
        Vector2 movement_direction = (touch.position - touch_start_position).normalized;
        touch_input = movement_direction * magnitude;
    }

    void mousePointAndClick()
    {
        if (MainCamera == null || MovementScriptReference == null) return;

        // Shoot a ray from the camera through the mouse position into the scene and get the collision point
        // Also get the object that was hit
        Transform _activeObjectTransform = Raycast(Input.mousePosition);

        // Debug Clicking
        //Vector3 cameraDirection = (_mainCamera.transform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition)).normalized;
        //if (_activeObjectTransform) Debug.DrawRay(MainCamera.ScreenToWorldPoint(Input.mousePosition), MainCamera.transform.forward * 20f, Color.red, 25f);
        //else Debug.DrawRay(MainCamera.ScreenToWorldPoint(Input.mousePosition), MainCamera.transform.forward * 35f, Color.white, 10f);

        // Mouse clicked?
        if (Input.GetMouseButton(0) && Time.time > last_click_time + click_delay)
        {
            last_click_time = Time.time;

            // Clicked on object, or in the air?
            if (_activeObjectTransform && _activeObjectTransform.tag == "Interactable")
                MovementScriptReference.leafClicked(_activeObjectTransform);
            else // Shoot, if no Interactable object was clicked
                MovementScriptReference.tryStartShooting();
        }
    }

    // Do a raycast from the main camera to the specified position, set the mouse position and return the object hit
    Transform Raycast(Vector3 screen_point)
    {
        if (MainCamera == null || MovementScriptReference == null) return null;

        // is point in front of camera?
        Vector3 viewport_point = MainCamera.ScreenToViewportPoint(screen_point);
        if (viewport_point.x > 1 || viewport_point.x < 0 ||
            viewport_point.y > 1 || viewport_point.y < 0 ||
            viewport_point.z < 0)
        {
            return null;
        }

        ray = MainCamera.ScreenPointToRay(screen_point);
        if (Physics.Raycast(ray, out ray_hit))
        {
            mouse_point = ray_hit.point;
            mouse_point.y = MovementScriptReference.transform.position.y;
            return ray_hit.transform;
        }
        else return null;
    }

    //Vector3 RaycastPosition ( Vector3 point )
    //{
    //    ray = main_camera.ScreenPointToRay (point);
    //    if ( Physics.Raycast (ray, out ray_hit) )
    //        return ray_hit.point;
    //    else return new Vector3 (0f, 5f, 0f);
    //}

    //Vector3 RaycastPosition ( )
    //{
    //    ray = main_camera.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));
    //    if ( Physics.Raycast (ray, out ray_hit) )
    //        return ray_hit.point;
    //    else return new Vector3 (0f, 5f, 0f);
    //}

}
