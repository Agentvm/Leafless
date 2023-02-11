using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour
{
    float plant_radius = 10f;
    Object leaf_object;

    [SerializeField] float max_size = 20f;
    [SerializeField] float min_size = 5f;
    Vector3 plant_scale = Vector3.zero;
    [Range (20f, 40f)] [SerializeField] float max_grow_delay = 30f;
    [SerializeField] float min_grow_delay = 6f;
    float current_grow_delay = 0f;

    Queue<Transform> leaf_regrow_queue = new Queue<Transform> ();
    int number_of_leaves = 0;
    int max_leaves = 0;

    bool regrowing_started = false;
    float regrow_start_time = 0f;

    float leaf_offset = 0f;

    //private float GrowDelay { get => max_grow_delay - ((max_grow_delay - min_grow_delay) * number_of_leaves / max_leaves); }

    // Start is called before the first frame update
    void Start ()
    {
        max_size /= 4f;
        if (max_size < min_size)
            max_size = min_size + 1f;
        max_size *= GameState.Instance.GameIntensity;

        // Resize plant randomly
        float size = max_size * fakeGaussian () + min_size;
        plant_scale.Set (size, size, size);
        transform.GetChild (0).localScale = plant_scale;

        // increase max_grow_delay for big plants
        max_grow_delay -= max_size;
        max_grow_delay += size;
        if (max_grow_delay < 0) max_grow_delay = 10f;

        // get plant radius from child model
        plant_radius = this.transform.GetChild (0).transform.GetComponent<Renderer> ().bounds.extents.magnitude / 2;
        //plant_radius *= 0.81f; // At this point, I really don't care how I fix this

        // preload leaf object
        leaf_object = Resources.Load ("LeafComplete");

        // single leaf test
        // Debug.Log ("Diameter " + this.name + ": " + plant_radius);
        //Vector3 point_on_radius = this.transform.position + this.transform.forward * plant_radius;
        //instantiateLeafOnRadius (point_on_radius);

        // grow all leafs at the start of the game
        int leaf_count = (int)((Mathf.PI * (plant_radius / .81f)) / 2.2f);
        foreach (Vector3 position in getPositionsOnRadius ((int)(leaf_count)))
        {
            instantiateLeaf (position);
        }

        // setup the regrow mechanism
        number_of_leaves = max_leaves = this.transform.childCount;
        reCalculateGrowDelay ();
    }

    // returns a value from a distribution made from random.InsideUnitCircle
    float fakeGaussian ()
    {
        Vector2 cirlce_coordinates = Random.insideUnitCircle;
        return cirlce_coordinates.x / 2f + 0.5f;
    }

    void testFakeGaussian ()
    {
        // draw samples
        List<float> liste = new List<float> ();
        for (int i = 0; i < 20000; i++)
        {
            liste.Add (fakeGaussian ());
        }

        // fill histogram
        float[] histogram = new float[10];
        foreach (float sample in liste)
        {
            if (sample < 0.1f) histogram[0]++;
            else if (sample < 0.2f) histogram[1]++;
            else if (sample < 0.3f) histogram[2]++;
            else if (sample < 0.4f) histogram[3]++;
            else if (sample < 0.5f) histogram[4]++;
            else if (sample < 0.6f) histogram[5]++;
            else if (sample < 0.7f) histogram[6]++;
            else if (sample < 0.8f) histogram[7]++;
            else if (sample < 0.9f) histogram[8]++;
            else histogram[9]++;
        }

        // normalise
        for (int i = 0; i < 10; i++)
        {
            histogram[i] /= 20000;
        }

        // print
        Debug.Log ("fakeGaussian Hist: " + histogram[0] + "-" + histogram[1]
                + "-" + histogram[2] + "-" + histogram[3] + "-" + histogram[4]
                + "-" + histogram[5] + "-" + histogram[6] + "-" + histogram[7]
                + "-" + histogram[8] + "-" + histogram[9] + "-");
    }

    // returns even-spaced positions on a unit circle
    List<Vector3> getPositionsOnRadius (int number_of_positions)
    {
        List<Vector3> list_of_points = new List<Vector3> ();

        for (int i = 0; i < number_of_positions; ++i)
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

    // build new leaf and orient it according to plant center
    void instantiateLeaf (Vector3 point_on_radius)
    {
        Transform new_leaf = ((GameObject)Instantiate (leaf_object,
                                                        point_on_radius,
                                                        Quaternion.LookRotation (point_on_radius - this.transform.position, Vector3.up),
                                                        this.transform)).transform;


        if (leaf_offset == 0f && new_leaf.GetChild (1)) // leaf offset is the same for each leaf
            leaf_offset = Vector3.Distance (new_leaf.GetChild (1).transform.position, new_leaf.position) + 0.15f * plant_radius; // get leaf-joint distance
        new_leaf.position += new_leaf.forward.normalized * leaf_offset;
    }

    // Update is called once per frame
    void Update ()
    {
        if (number_of_leaves < max_leaves)
        {
            if (!regrowing_started)
            {
                regrowing_started = true;
                regrow_start_time = Time.time;
            }
            else if (Time.time > regrow_start_time + current_grow_delay)
            {
                if (leaf_regrow_queue.Count > 0)
                {
                    leaf_regrow_queue.Dequeue ()?.GetComponent<Leaf> ()?.ReGrow ();
                    number_of_leaves++;
                    reCalculateGrowDelay ();
                    if (leaf_regrow_queue.Count > 0) regrow_start_time = Time.time;
                }
            }
            //if (number_of_leaves == 1)
            //    AudioManager.Instance.Play ("Chime");
        }
        else regrowing_started = false;

    }

    public void destroyOneLeaf ()
    {
        max_leaves--;
        number_of_leaves--;
        reCalculateGrowDelay ();
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
