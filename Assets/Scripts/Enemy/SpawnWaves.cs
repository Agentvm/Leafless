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
    void Start ()
    {
        enemy_object = Resources.Load ("Enemy");
        player = GameObject.FindObjectOfType<Movement>().transform;
        start_time = Time.time;
        //InvokeRepeating ("spawnEnemy", 0f, 10f);
    }

    // Update is called once per frame
    void Update ()
    {
        // start of game: if leaves have been eaten, spawn enemies
        if (GameState.Instance.InGameScore > 2 && NumberOfActiveEnemies == 0)
        {
            spawnEnemy ();
        }

        // if enemies are less than possible max (tied to award), eneble spawning
        int current_max_enemies = (Mathf.Min (1, GameState.Instance.InGameScore) + GameState.Instance.InGameScore / 10);
        if (NumberOfActiveEnemies < current_max_enemies)
        {
            // as award increases, decrease spawn time of enemies
            float award_progress = (GameState.Instance.InGameScore) / 100f;
            int spawn_time_reduction = (int)(7 * Mathf.Min (1, award_progress));
            if (Time.time - start_time > spawn_time + (10 - spawn_time_reduction))
            {
                spawnEnemy ();
                spawn_time = Time.time - start_time;
            }
        }
    }

    Vector3 randomPointNearPlayer ()
    {
        Vector2 point_on_circle_2d = Random.insideUnitCircle.normalized * 35f;
        Vector3 point_on_circle = Vector3.zero;
        point_on_circle.x = point_on_circle_2d.x;
        point_on_circle.z = point_on_circle_2d.y;

        return player.transform.position + point_on_circle;
    }

    public void spawnEnemy (Vector3 global_position)
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
        foreach (Transform enemy in transform)
        {
            enemy.GetComponent<Follow> ().CurrentlyFollowing = false;
        }
    }

    public void AllEnemiesAttack ()
    {
        foreach (Transform enemy in transform)
        {
            enemy.GetComponent<Follow> ().CurrentlyFollowing = true;
        }
    }

    public void StopSpawning ()
    {
        Debug.LogError ("SpawnWaves>StopSpawning is not implemented yet");
    }
}
