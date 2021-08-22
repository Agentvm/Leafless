using UnityEngine;

public enum Direction { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }

public class ExtendWorld : MonoBehaviour
{
    [SerializeField] private Direction extension_direction = Direction.East;
    private float creation_offset = 0f;
    private Vector3 creation_direction;
    private Vector3 downward_offset = Vector3.zero;

    // Start is called before the first frame update
    void Start ()
    {
        creation_offset = this.transform.parent.localScale.x * 5;
        downward_offset.y = -15f;

        if (extension_direction == Direction.North) creation_direction = -Vector3.right;
        else if (extension_direction == Direction.East) creation_direction = Vector3.forward;
        else if (extension_direction == Direction.South) creation_direction = Vector3.right;
        else if (extension_direction == Direction.West) creation_direction = -Vector3.forward;
    }

    // Update is called once per frame
    void Update ()
    {

    }

    private void OnTriggerEnter (Collider other)
    {
        // check for player collision
        if (other.tag == "Player")
        {
            // Check if the place to create a new panel is already occupied
            if (checkPanelOccupied (this.transform.position + creation_direction * creation_offset))
            {
                // never use this trigger again
                this.gameObject.SetActive (false);
                return;
            }
            else
            {
                // If not occupied, initiate a new World Panel as child of the world game Object
                Transform new_panel = (
                (GameObject)(Instantiate (Resources.Load ("Panel"),
                this.transform.position + creation_direction * creation_offset + downward_offset,
                new Quaternion (),
                this.transform.parent.parent.parent))).transform;
            }

            // if triggered by player once, disable this trigger
            this.gameObject.SetActive (false);
        }
    }

    bool checkPanelOccupied (Vector3 at_position)
    {
        GameObject[] panels_array = GameObject.FindGameObjectsWithTag ("Panel");

        foreach (GameObject panel in panels_array)
            if (Mathf.Abs (panel.transform.position.x - at_position.x) < 0.5f &
                Mathf.Abs (panel.transform.position.z - at_position.z) < 0.5f) return true;

        return false;
    }
}
