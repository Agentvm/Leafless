using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPanel : MonoBehaviour
{
    Object plant_object;
    float damp_variable = 0f;

    private void Awake ()
    {
        plant_object = Resources.Load ("Plant");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3 random_position in create_random_positions (3) )
        {
            Transform new_plant = (
                (GameObject)(Instantiate (plant_object,
                this.transform.position + random_position,
                new Quaternion (),
                this.transform)) ).transform;
        }
    }

    List<Vector3> create_random_positions (int number_of_positions)
    {
        List<Vector3> random_positions = new List<Vector3> ();

        int current_number_of_positions = 0;
        Vector3 origin = new Vector3 (0, 1.9f, 0);

        for (int i = 0; i <= 100; i++ )
        {
            if ( current_number_of_positions >= number_of_positions )
                break;

            // get a new random position
            Vector3 direction = Random.insideUnitSphere;
            Vector3 random_position = direction * this.transform.localScale.x * 35f;
            random_position.y = 1.9f; // Make sure plant is above ground

            // check it is not too close to another pick
            if ( random_positions.Count > 0 )
            {
                bool suitable_position = true;

                // check with all existing positions if this one is suitable
                foreach ( Vector3 picked_position in random_positions )
                {
                    // discard if too close to another position or the origin
                    if ( Vector3.Distance (picked_position, random_position) < 8 || Vector3.Distance (random_position, origin) < 20f )
                    {
                        suitable_position = false;
                        break;
                    }
                }

                // add the random position, if it is suitable
                if ( suitable_position )
                {
                    random_positions.Add (random_position);
                    current_number_of_positions++;
                }
            }
            // check if the first selected position is too close to the world origin
            else if ( Vector3.Distance (random_position, origin) > 20f )
            {
                // add the first position
                random_positions.Add (random_position);
                current_number_of_positions++;
            }
        }

        return random_positions;
    }

    private void Update ()
    {
        if ( this.transform.position.y < 0 )
        {
            Vector3 upward_movement = Vector3.zero;
            upward_movement.y = this.transform.position.y - Mathf.SmoothDamp (this.transform.position.y, 0f, ref damp_variable, 25f * Time.deltaTime);
            this.transform.Translate (-upward_movement);
        }
    }
}
