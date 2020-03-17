using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPanel : MonoBehaviour
{
    Object plant_object;

    private void Awake ()
    {
        plant_object = Resources.Load ("Plant");
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <3; i++ )
        {
            Vector3 random_position = Random.insideUnitSphere * this.transform.localScale.x * 4f;
            random_position.y = 1.9f;

            Transform new_plant = (
                (GameObject)(Instantiate (plant_object,
                this.transform.position + random_position,
                new Quaternion (),
                this.transform.parent)) ).transform;
        }
    }

    private void Update ()
    {
        
    }
}
