using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]int ammunition = 0;

    // references
    
    PointAndClick PointAndClickScriptReference;

    // Components
    Transform player;
    Animator animator;

    float shoot_time = 0f;
    float shoot_delay = 1f;
    bool currently_shooting = false;
    bool shooting_disabled = false;

    public int Ammunition { get => ammunition; set => ammunition = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag ("Player").transform;
        animator = GetComponent<Animator> ();
        PointAndClickScriptReference = player.GetComponent<PointAndClick> ();
    }

    // Update is called once per frame
    void Update()
    {
        if ( shooting_disabled ) return;

        if ( Ammunition > 0 && Input.GetMouseButton (0) && Time.time > shoot_time + shoot_delay )
        {
            if ( PointAndClickScriptReference.ClickedInteractable == null )
            {
                currently_shooting = true;
                animator.SetBool ("Shooting", true);
                shoot_time = Time.time;
            }
        }

        // at the right time in the animation, instantiate the bullet
        if (currently_shooting && Time.time > shoot_time + 0.5f )
        {
                Ammunition -= 1;
                Instantiate (Resources.Load ("Bullet"), this.transform.position + this.transform.forward * .5f, this.transform.rotation);
                currently_shooting = false;
                animator.SetBool ("Shooting", false);
        }
    }

    public void disableShooting ()
    {
        shooting_disabled = true;
    }

    public void enableShooting ()
    {
        shooting_disabled = false;
    }


}
