using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }

public class ExtendWorld : MonoBehaviour
{
    [SerializeField]private Direction extension_direction = Direction.East;
    private float creation_offset = 0f;
    private Vector3 creation_direction;
    bool corner = false;

    // Start is called before the first frame update
    void Start()
    {
        creation_offset = this.transform.parent.localScale.x * 5;

        if ( extension_direction == Direction.North ) creation_direction = -Vector3.right;
        else if ( extension_direction == Direction.East ) creation_direction = Vector3.forward;
        else if ( extension_direction == Direction.South ) creation_direction = Vector3.right;
        else if ( extension_direction == Direction.West ) creation_direction = -Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter ( Collider other )
    {
        if (other.tag == "Player")
        {
            if ( extension_direction != Direction.South ) return;

            // Initiate a new World Panel as child of the world game Object
            Transform new_panel = (
                (GameObject)(Instantiate (Resources.Load ("Panel"),
                this.transform.position + creation_direction * creation_offset,
                new Quaternion (),
                this.transform.parent.parent)) ).transform;

            // Disable the direction we're coming from
            if ( extension_direction == Direction.North ) new_panel.GetChild (4).gameObject.SetActive (false);
            else if ( extension_direction == Direction.East ) creation_direction = Vector3.forward;
            else if ( extension_direction == Direction.South ) creation_direction = Vector3.right;
            else if ( extension_direction == Direction.West ) creation_direction = -Vector3.forward;

            this.gameObject.SetActive (false);
        }
    }
}
