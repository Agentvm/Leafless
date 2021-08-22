using UnityEngine;

public class MirrorRotation : MonoBehaviour
{
    [SerializeField] Transform look_at_object;

    // Start is called before the first frame update
    void Start ()
    {
        if (!look_at_object)
            look_at_object = Camera.main.transform;
    }

    // Update is called once per frame
    void Update ()
    {
        if (look_at_object)
            this.transform.rotation = look_at_object.rotation;
        //this.transform.LookAt (2 * transform.position - look_at_object.position);
    }
}
