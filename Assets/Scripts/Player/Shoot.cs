using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]int ammunition = 0;

    // references
    Transform player;
    PointAndClick PointAndClickScriptReference;
    float shoot_time = 0f;
    float shoot_delay = 0.5f;

    public int Ammunition { get => ammunition; set => ammunition = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag ("Player").transform;
        PointAndClickScriptReference = player.GetComponent<PointAndClick> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ammunition > 0 && Input.GetMouseButton (0) && Time.time > shoot_time + shoot_delay )
        {
            if ( PointAndClickScriptReference.ClickedInteractable == null )
            {
                shoot_time = Time.time;
                Ammunition -= 1;
                Instantiate(Resources.Load("Bullet"), this.transform.position, this.transform.rotation );
            }
        }
    }
    
}
