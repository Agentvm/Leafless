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
    bool end_game = false;
    float end_game_time = 0f;

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
        else if ( end_game && Time.time > end_game_time + 0.7f )
        {
            SceneLoader.Instance.loadNextScene ();
        }
    }

    public void Die ()
    {
        transform.parent.GetComponent<SpawnWaves> ().NumberOfActiveEnemies--;
        this.GetComponent<SphereCollider> ().enabled = false;
        FollowScriptReference.stopFollowing ();
        //animator.Play ("Armature|Die");
        death_time = Time.time;
        dying = true;
        SceneLoader.Instance.Award = 2;
    }

    // Kill Player
    private void OnTriggerEnter ( Collider collider )
    {
        if ( collider.transform.tag == "Player")
        {
            // Stop player an all enemies
            collider.GetComponent<Movement> ().MovementDisabled = true;
            if (transform.parent)
            {
                this.transform.GetComponentInParent<SpawnWaves> ().StopAllEnemies ();
                //this.transform.GetComponentInParent<SpawnWaves> ().StopSpawning ();
            }
            this.GetComponent<Animator> ().SetBool ("Eating", true);

            end_game = true;
            end_game_time = Time.time;
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit ();
        }
    }
}
