using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour
{
    float plant_radius = 10f;
    Object leaf_object;

    [SerializeField] float max_grow_delay = 10f;
    [SerializeField] float min_grow_delay = 2f;
    float current_grow_delay = 0f;

    Queue<Transform> leaf_regrow_queue = new Queue<Transform> ();
    int number_of_leaves = 0;
    int max_leaves = 0;

    bool regrowing_started = false;
    float regrow_start_time = 0f;

    float leaf_offset = 0f;

    //private float GrowDelay { get => max_grow_delay - ((max_grow_delay - min_grow_delay) * number_of_leaves / max_leaves); }

    // Start is called before the first frame update
    void Start()
    {
        // get plant radius from child model
        //plant_diameter = Vector3.Distance (this.transform.position, this.transform.GetChild (0).GetChild (0).transform.position) * 2;
        //plant_diameter = this.transform.GetChild (0).localScale.x;
        plant_radius = this.transform.GetChild (0).transform.GetComponent<Renderer> ().bounds.extents.magnitude / 2;
        plant_radius *= 0.81f; // At this point, I really don't care how I fix this

        // preload leaf object
        leaf_object = Resources.Load ("LeafComplete");

        // single leaf test
        // Debug.Log ("Diameter " + this.name + ": " + plant_radius);
        //Vector3 point_on_radius = this.transform.position + this.transform.forward * plant_radius;
        //instantiateLeafOnRadius (point_on_radius);

        // grow all leafs at the start of the game
        int leaf_count = (int)((Mathf.PI * (plant_radius/.81f)) / 2.2f);
        foreach (Vector3 position in getPositionsOnRadius ((int)(leaf_count) ))
        {
            instantiateLeafOnRadius (position );
        }

        // setup the regrow mechanism
        number_of_leaves = max_leaves = this.transform.childCount;
        reCalculateGrowDelay ();
    }

    //15.734:  -0.5 13.1
    //10:       0.6 7.4

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
            list_of_points.Add (this.transform.position + position * plant_radius);
        }

        return list_of_points;
    }

    void instantiateLeafOnRadius (Vector3 point_on_radius)
    {
        Transform new_leaf = ((GameObject)Instantiate ( leaf_object,
                                                        point_on_radius,
                                                        Quaternion.LookRotation (point_on_radius - this.transform.position, Vector3.up),
                                                        this.transform)).transform;


        if ( leaf_offset == 0f && new_leaf.GetChild (1) ) // leaf offset is the same for each leaf
            leaf_offset = Vector3.Distance (new_leaf.GetChild (1).transform.position, new_leaf.position); // get leaf-joint distance
        new_leaf.position += new_leaf.forward.normalized * leaf_offset;
    }

    // Update is called once per frame
    void Update()
    {
        if ( number_of_leaves < max_leaves )
        {
            if ( !regrowing_started )
            {
                regrowing_started = true;
                regrow_start_time = Time.time;
            }
            else if (Time.time > regrow_start_time + current_grow_delay )
            {
                leaf_regrow_queue.Dequeue ().GetComponent<Leaf> ().Grow ();
                number_of_leaves++;
                reCalculateGrowDelay ();
                if (leaf_regrow_queue.Count > 0) regrow_start_time = Time.time;
            }
        }
        else regrowing_started = false;

    }

    void reCalculateGrowDelay ()
    {
        current_grow_delay = max_grow_delay - ((max_grow_delay - min_grow_delay) * number_of_leaves / max_leaves);
    }

    public void noticeEatenLeaf (Transform eaten_leaf)
    {
        leaf_regrow_queue.Enqueue (eaten_leaf);
        number_of_leaves--;
    }
}
