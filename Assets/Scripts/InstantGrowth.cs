using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantGrowth : MonoBehaviour
{
    float plant_diameter = 10f;
    Object leaf_object;

    // Start is called before the first frame update
    void Start()
    {
        // get plant radius from child model
        plant_diameter = Vector3.Distance (this.transform.GetChild (0).transform.position, this.transform.GetChild (1).transform.position) * 2;
        leaf_object = Resources.Load ("LeafComplete");

        //Vector3 point_on_radius = this.transform.position + this.transform.forward * plant_diameter;
        //instantiateLeafOnRadius (point_on_radius);

        foreach (Vector3 position in getPositionsOnRadius ((int)(plant_diameter * 1.3f) ))
        {
            instantiateLeafOnRadius (position );
        }

    }

    List<Vector3> getPositionsOnRadius (int number_of_positions)
    {
        List<Vector3> list_of_points = new List<Vector3> ();

        for ( int i = 0; i < number_of_positions; ++i )
        {
            // calculate angle that points to the edge of the unit circle
            float theta = ((2 * Mathf.PI) / number_of_positions) * i;

            // create vector containing new point
            Vector3 position = Vector3.zero;
            position.x = Mathf.Cos (theta); // calculate x of theta
            position.z = Mathf.Sin (theta); // calculate y of theta

            // add current point, multiplying it with radius
            list_of_points.Add (this.transform.position + position * plant_diameter);
        }

        return list_of_points;
    }

    void instantiateLeafOnRadius (Vector3 point_on_radius)
    {
        Transform new_leaf = ((GameObject)Instantiate ( leaf_object,
                                                        point_on_radius,
                                                        Quaternion.LookRotation (point_on_radius - this.transform.position, Vector3.up))).transform;

        float leaf_offset = 0f;
        if ( new_leaf.GetChild (1) )
            // get leaf-joint distance
            leaf_offset = Vector3.Distance (new_leaf.GetChild (1).transform.position, new_leaf.position);
        new_leaf.position -= new_leaf.forward.normalized * leaf_offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
