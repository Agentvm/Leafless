using UnityEngine;

public class Rotate : MonoBehaviour
{

    //Vector3 rotation_axis;
    float rotation_angle = 500f;

    // Start is called before the first frame update
    void Start ()
    {
        //rotation_axis = this.transform.forward;
    }

    // Update is called once per frame
    void Update ()
    {
        this.transform.Rotate (this.transform.forward, rotation_angle * Time.deltaTime);
    }
}
