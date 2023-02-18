using UnityEngine;

public class Movement : MonoBehaviour
{
    // Statemachine
    enum State { FreeMovement, ApproachLeaf, EatLeaf, Shoot, MoveToPosition }
    State state;

    // References
    //PointAndClick PointAndClickScriptReference;

    // Components
    CharacterController controller;
    Animator animator;

    // Configuration
    [Range(0f, 1600f)][SerializeField] float max_movement_speed = 800f;
    [Range(0f, 6f)][SerializeField] float leaf_distance = 4f;
    [Range(1f, 10f)][SerializeField] float acceleration = 4f;
    [Range(1f, 10f)][SerializeField] float rotation_acceleration = 6f;
    [Range(0f, 4f)][SerializeField] float shoot_rotation_acceleration = 2f;
    [Range(0f, 20f)][SerializeField] float touch_shoot_rotation_acceleration = 2f;
    [Range(0f, 20f)][SerializeField] float touch_aim_correction = 2.0f;

    // Movement
    bool movement_disabled = false;
    float current_movement_speed = 0f;
    Vector3 targetPosition = Vector3.zero;

    // Damping variables
    float damp_movement = 0f;

    // Shoot
    Object bullet_object;
    Vector3 aim_position;
    [SerializeField] int ammunition_capacity = 3;
    [SerializeField] int ammunition = 0;
    float shoot_time = 0f;
    float shoot_delay = .5f * (1f / 1.5f);

    // Interaction with objects
    Transform leaf_to_approach = null;
    float time_start_of_leaf_eating = 0f;
    float leaf_eat_delay = 1.1f;

    // Properties
    public bool MovementDisabled { get => movement_disabled; set => movement_disabled = value; }
    public int Ammunition { get => ammunition; set => ammunition = Mathf.Min(value, ammunition_capacity); }


    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Get References
        //PointAndClickScriptReference = GetComponent<PointAndClick> ();

        // Preload bullet object
        bullet_object = Resources.Load("Bullet");

        // set initial state
        state = State.FreeMovement;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log ("Intensity: " + GameState.Instance.GameIntensity);

        if (MovementDisabled) return;

        if (state == State.FreeMovement)
        {
            if (leaf_to_approach != null)
                changeState(State.ApproachLeaf);

            Move(InputModule.Instance.MousePoint);
        }
        else if (state == State.MoveToPosition)
        {
            // Update target
            if (Input.GetMouseButton(1)) targetPosition = InputModule.Instance.MousePoint;

            // Distance Check
            if (Vector3.Distance(this.transform.position, targetPosition) < .5f || !Input.GetMouseButton(1))
            {
                changeState(State.FreeMovement);
            }
            else
            {
                MoveToPosition(targetPosition, true);
            }
        }
        else if (state == State.ApproachLeaf)
        {
            // Abort eating the leaf when you're still on the way
            if (Mathf.Abs(InputModule.Instance.UserInput.x) > .5f || Mathf.Abs(InputModule.Instance.UserInput.x) > .5f)
            {
                changeState(State.FreeMovement);
                leaf_to_approach = null;
                return;
            }

            // Figure out leaf position
            Vector3 modified_leaf_position = leaf_to_approach.position;

            // Todo: fix this by changing the leaf model origin // Or cache this - we only need it once
            if (leaf_to_approach.parent)
            {
                modified_leaf_position = leaf_to_approach.parent.position;
                modified_leaf_position += leaf_distance * leaf_to_approach.parent.forward;
                modified_leaf_position.y = this.transform.position.y; // set on same height, so goal can always be reached by walking the ground
            }

            // Distance Check
            if (Vector3.Distance(this.transform.position, modified_leaf_position) < .5f)
            {
                animator.SetBool("Eating", true);
                changeState(State.EatLeaf);
                time_start_of_leaf_eating = Time.time;
            }
            else
            {
                MoveToPosition(modified_leaf_position, true);
            }
        }
        else if (state == State.EatLeaf)
        {
            if (leaf_to_approach == null) changeState(State.FreeMovement);

            // Stop the leaf eating "animation"
            if (Time.time > time_start_of_leaf_eating + leaf_eat_delay / 2)
                leaf_to_approach.transform.GetComponent<Leaf>().gameObject.SetActive(false);

            // play eat animation, then change state
            if (Time.time > time_start_of_leaf_eating + leaf_eat_delay)
            {
                //leaf_to_approach.transform.GetComponent<Leaf> ().gameObject.SetActive (false);
                leaf_to_approach = null;
                Ammunition += 1;
                animator.SetBool("Eating", false);
                changeState(State.FreeMovement);
            }
        }
        else if (state == State.Shoot)
        {
            // Make speed changes less fast
            if (InputModule.Instance.TouchInputActive)
            {
                aim_position = InputModule.Instance.MousePoint + Vector3.forward * touch_aim_correction;
                Move(aim_position, 0.7f, touch_shoot_rotation_acceleration);
            }
            else
                Move(InputModule.Instance.MousePoint, 0.7f, shoot_rotation_acceleration);

            // at the right time in the animation, instantiate the bullet
            if (Time.time > shoot_time + shoot_delay)
            {
                Ammunition -= 1;
                Quaternion shoot_rotation = this.transform.rotation;
                shoot_rotation.eulerAngles = new Vector3(0f, shoot_rotation.eulerAngles.y, 0f);
                Instantiate(bullet_object, this.transform.position + this.transform.forward * .5f, shoot_rotation);
                animator.SetBool("Shooting", false);
                changeState(State.FreeMovement);
            }
        }

    }

    // Move by UserInput while looking at look_position, speed can be varied
    void Move(Vector3 look_position, float acceleration_modifier = 1f, float rotation_acceleration_modifier = 1f)
    {
        // gather speed // a max speed of 1f ensures that character does not move faster in diagon alley
        dampedAccelerate(Mathf.Min(InputModule.Instance.UserInput.magnitude, 1f) * max_movement_speed, acceleration_modifier);

        // move according to input, but keep a height of 0
        controller.SimpleMove(InputModule.Instance.UserInput.normalized * Time.deltaTime * current_movement_speed);
        this.transform.position.Set(this.transform.position.x, 0f, this.transform.position.z);

        // turn towards current mouse position
        Quaternion look_rotation;
        look_rotation = Quaternion.LookRotation(look_position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, look_rotation, Time.deltaTime * rotation_acceleration * rotation_acceleration_modifier);
    }

    void MoveToPosition(Vector3 position, bool lookAtTarget = false)
    {
        // figure out move vector towards position
        Vector3 move_vector = position - this.transform.position;
        move_vector.Normalize();

        // look to target
        if (lookAtTarget) this.transform.LookAt(position);

        // accelerate towards
        dampedAccelerate(max_movement_speed); // Todo: This is not really damped, though
        controller.SimpleMove(move_vector * Time.deltaTime * current_movement_speed);
        this.transform.position.Set(this.transform.position.x, 0f, this.transform.position.z);
    }

    // damp the acceleration of the movement speed
    void dampedAccelerate(float speed_to_be, float acceleration_modifier = 1f)
    {
        current_movement_speed = Mathf.SmoothDamp(current_movement_speed, speed_to_be, ref damp_movement, acceleration * acceleration_modifier * Time.deltaTime);
    }

    // this is the place for state entry instructions
    void changeState(State new_state)
    {
        if (new_state == State.EatLeaf)
        {
            leaf_to_approach.transform.GetComponent<Leaf>().GetEaten();
            this.transform.LookAt(leaf_to_approach.transform);
        }
        else if (new_state == State.Shoot && animator)
        {
            animator.SetBool("Shooting", true);
            shoot_time = Time.time;
        }
        else if (new_state == State.MoveToPosition)
            targetPosition = InputModule.Instance.MousePoint;

        //Debug.Log ("Changing State to: " + new_state);
        state = new_state;
    }

    public void leafClicked(Transform clicked_leaf)
    {
        if (InputModule.Instance.TouchInputActive)
        {
            Debug.Log("Eating Triggered through Touch, state: " + state);
        }

        if (state != State.ApproachLeaf && state != State.EatLeaf)
            leaf_to_approach = clicked_leaf;
    }

    public void tryStartShooting()
    {
        if (state == State.FreeMovement &&
             Ammunition > 0 &&
             Time.time > shoot_time + shoot_delay * 2)
        {
            changeState(State.Shoot);
        }
    }

    public void TryMoveToPosition()
    {
        if (state != State.FreeMovement) return;

        changeState(State.MoveToPosition);
    }

}
