using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithOffset : MonoBehaviour
{

    [SerializeField] private Transform master;
    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        offset = master.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = master.position - offset;
    }
}
