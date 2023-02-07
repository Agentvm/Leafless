﻿using UnityEngine;

public class MirrorMovement : MonoBehaviour
{

    [SerializeField] private Transform master;
    private Vector3 offset;


    // Start is called before the first frame update
    void Start ()
    {
        if (!master)
            master = GameObject.FindObjectOfType<Movement>().transform;
        offset = master.position - this.transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        if (master)
            this.transform.position = master.position - offset;
    }
}
