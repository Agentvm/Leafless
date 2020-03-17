using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    Object enemy_object;
    Transform player;

    int number_of_active_enemies = 0;
    int number_of_leaves_eaten = 0;
    float start_time;
    float spawn_time = 0f;

    public int NumberOfActiveEnemies { get => number_of_active_enemies; set => number_of_active_enemies = value; }
    public int NumberOfLeavesEaten { get => number_of_leaves_eaten; set => number_of_leaves_eaten = value; }

    // Start is called before the first frame update
    void Start()
    {
        enemy_object = Resources.Load ("Enemy");
        player = GameObject.FindWithTag ("Player").transform;
        start_time = Time.time;
        //InvokeRepeating ("spawnEnemy", 0f, 10f);
        SceneLoader.Instance.Award = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (number_of_active_enemies < (3) )
        {
            spawnEnemy ();
        }
        float award_progress = (SceneLoader.Instance.Award)/100f;
        int spawn_time_reduction = (int) (7 * Mathf.Min (1, award_progress ));
        if (Time.time - start_time > spawn_time + (10 - spawn_time_reduction ))
        {
            Debug.Log ("award_progress: " + award_progress + ", spawn_time_reduction: " + spawn_time_reduction + ", NumberOfActiveEnemies: " + NumberOfActiveEnemies);
            spawnEnemy ();
            spawn_time = Time.time - start_time;
            //SceneLoader.Instance.Award = 50;
        }
    }

    Vector3 randomPointNearPlayer ()
    {
        Vector2 point_on_circle_2d = Random.insideUnitCircle.normalized * 50f;
        Vector3 point_on_circle = Vector3.zero;
        point_on_circle.x = point_on_circle_2d.x;
        point_on_circle.z = point_on_circle_2d.y;

        return player.transform.position + point_on_circle;
    }

    public void spawnEnemy ( Vector3 global_position )
    {
        Transform new_enemy = ((GameObject)Instantiate (enemy_object, global_position, Quaternion.LookRotation (global_position - player.position, Vector3.up), this.transform)).transform;
        new_enemy.GetComponent<Follow> ().setGoal (player);
        NumberOfActiveEnemies++;
    }

    public void spawnEnemy ()
    {
        Vector3 global_position = randomPointNearPlayer ();
        Transform new_enemy = ((GameObject)Instantiate (enemy_object, global_position, Quaternion.LookRotation (global_position - player.position, Vector3.up), this.transform)).transform;
        new_enemy.GetComponent<Follow> ().setGoal (player);
        NumberOfActiveEnemies++;
    }

    public void StopAllEnemies ()
    {
        foreach ( Transform enemy in transform )
        {
            enemy.GetComponent<Follow> ().CurrentlyFollowing = false;
        }
    }

    public void AllEnemiesAttack ()
    {
        foreach ( Transform enemy in transform )
        {
            enemy.GetComponent<Follow> ().CurrentlyFollowing = true;
        }
    }

    public void StopSpawning ()
    {
        Debug.LogError ("SpawnWaves>StopSpawning is not implemented yet");
    }
}
