using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Components
    Animator animator;

    // References
    Follow FollowScriptReference;

    // variables
    bool dying = false;
    float death_time = 0;

    // Start is called before the first frame update
    void Start()
    {
        FollowScriptReference = GetComponent<Follow> ();
        animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        if ( dying && Time.time > death_time + 1f )
        {
            this.gameObject.SetActive (false);
        }
    }

    public void Die ()
    {
        FollowScriptReference.stopFollowing ();
        //animator.Play ("Armature|Die");
        death_time = Time.time;
        dying = true;
    }

    private void OnTriggerEnter ( Collider collider )
    {
        if ( collider.transform.tag == "Player")
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit ();
        }
    }
}
