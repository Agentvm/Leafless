using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]int ammunition_capacity = 3;
    [SerializeField]int ammunition = 0;

    // references
    
    PointAndClick PointAndClickScriptReference;

    // Components
    Transform player;
    Animator animator;

    float shoot_time = 0f;
    [SerializeField]float shoot_delay = .5f;
    bool currently_shooting = false;
    bool shooting_disabled = false;

    public int Ammunition { get => ammunition; set => ammunition = Mathf.Min (value, ammunition_capacity); }

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
        // at the right time in the animation, instantiate the bullet
        if (currently_shooting && Time.time > shoot_time + 0.5f )
        {
            Ammunition -= 1;
            Quaternion shoot_rotation = this.transform.rotation;
            shoot_rotation.eulerAngles = new Vector3 (0f, shoot_rotation.eulerAngles.y, 0f);
            Instantiate (Resources.Load ("Bullet"), this.transform.position + this.transform.forward * .5f, shoot_rotation);
            animator.SetBool ("Shooting", false);
            currently_shooting = false;
                
        }
    }

    public void startShooting ( bool forced = false)
    {
        if ( !forced && shooting_disabled ) return;

        if ( !currently_shooting && Ammunition > 0 && Time.time > shoot_time + shoot_delay && PointAndClickScriptReference.ClickedInteractable == null )
        {
            currently_shooting = true;
            animator.SetBool ("Shooting", true);
            shoot_time = Time.time;
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
