using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Components
    Animator animator;

    // References
    Follow FollowScriptReference;
    SpawnWaves SpawnWavesReference;

    // variables
    bool dying = false;
    float death_time = 0;
    bool end_game = false;
    float end_game_time = 0f;

    public bool Dying { get => dying; }

    // Start is called before the first frame update
    void Start()
    {
        FollowScriptReference = GetComponent<Follow> ();
        SpawnWavesReference = this.transform.parent.GetComponent<SpawnWaves> ();
        animator = GetComponent<Animator> ();

        RaycastHit ray_hit;
        Ray ray = new Ray (this.transform.position, -Vector3.up);
        if ( !Physics.Raycast (ray, out ray_hit, 5f) ) Respawn ();
        else
        {
            if ( ray_hit.transform.tag == "PlantBody" ) Respawn ();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if Distance to player gets too large, respawn
        if ( Vector3.Distance (this.transform.position, FollowScriptReference.FollowedObject.position) > 50f )
            Respawn ();

        if ( Dying && Time.time > death_time + 1f )
        {
            Destroy (this.gameObject);
        }
        else if ( end_game && Time.time > end_game_time + 0.7f )
        {
            SceneLoader.Instance.loadNextScene ();
        }
    }

    private void Respawn ()
    {
        SpawnWavesReference.spawnEnemy ();
        SpawnWavesReference.NumberOfActiveEnemies--;
        Destroy (this.gameObject);
    }

    public void Die ()
    {
        // stop movement
        FollowScriptReference.stopFollowing ();
        
        //
        this.GetComponent<SphereCollider> ().enabled = false;
        this.GetComponent<CharacterController> ().enabled = false;
        this.gameObject.AddComponent<BoxCollider> ();
        Rigidbody death_rigidbody = this.gameObject.AddComponent<Rigidbody> ();
        if (death_rigidbody)
            death_rigidbody.AddExplosionForce (650f, this.transform.position + this.transform.forward * 1f - this.transform.up * 0.6f, 100f );

        // 
        death_time = Time.time;
        dying = true;
        SpawnWavesReference.NumberOfActiveEnemies--;
        SceneLoader.Instance.Award = 2;
    }

    // Kill Player
    private void OnTriggerEnter ( Collider collider )
    {
        if ( collider.transform.tag == "Player")
        {
            // Stop player and all enemies
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
