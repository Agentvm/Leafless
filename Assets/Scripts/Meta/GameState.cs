using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    // Static instance of GameState which allows it to be accessed by any other script.
    public static GameState Instance = null;

    private int award = 0;
    private int ewerd = 0;
    private Text award_text;

    // game difficulty / intensity
    private Transform player_transform;
    private Vector3 origin_position = new Vector3 (0f, 0.5f, 0f);
    private float maxIntensity = 3.0f;

    // tutorial state
    public bool tutorial_toggle = true;
    public bool tutorial_completed = false;

    public int Award { get => award; set => award += (int)(Mathf.Min (value, 10) * GameIntensity); } // max 10 flat award points for one action

    // document this formula - fast
    public float GameIntensity { get => getGameIntensity (); }
    public float MaxIntensity { get => maxIntensity; }

    // Start is called before the first frame update
    void Awake ()
    {
        // check that there is only one instance of this and that it is not destroyed on load
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy (gameObject);
        DontDestroyOnLoad (gameObject);

        if (PlayerPrefs.HasKey ("Ewerd"))
            ewerd = PlayerPrefs.GetInt ("Ewerd");

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged; ;

        StartCoroutine (logIntensity ());
    }

    private void SceneManager_activeSceneChanged (UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        // Search for player at Scene Change
        if (GameObject.FindWithTag ("Player"))
            player_transform = GameObject.FindWithTag ("Player").transform;

        if (arg1.name == "Game")
            Debug.Assert (player_transform != null);
    }

    // Called once at the start of the game, since this is DontDestroyOnLoad
    private void Start ()
    {
        // Find player at game start
        if (GameObject.FindWithTag ("Player"))
            player_transform = GameObject.FindWithTag ("Player").transform;
    }

    private void Update ()
    {
        if (award_text)
            if (SceneLoader.Instance.CurrentSceneName == "Menu")
            {
                award_text.text = "Maximum Score: " + ewerd.ToString ();
                award = 0;
            }
            else
                award_text.text = "Score: " + Award.ToString ();
        else
        {
            award_text = GameObject.FindWithTag ("Score").GetComponent<Text> ();
        }
    }

    private float getGameIntensity ()
    {
        if (!player_transform) return 1f;
        float playerOriginDistance = Vector3.Distance (player_transform.position, origin_position);
        //return Mathf.Min (Mathf.Max (Vector3.Distance (player_transform.position, origin_position) /
        //                  (100f + 2f * Vector3.Distance (player_transform.position, origin_position)) * 4,
        //                  1f), maxIntensity);

        // Minimum Intensity is 1
        // It rises exponentially from below 1 to maxIntensity
        return Mathf.Min (Mathf.Max (0.219f * Mathf.Pow (playerOriginDistance, 0.35f), 1f), maxIntensity);
    }

    IEnumerator logIntensity ()
    {
        while (true)
        {
            //if ( player_transform )
            //Debug.Log ("Distance/Intensity: " + Vector3.Distance (player_transform.position, origin_position) + " / " + getGameIntensity ());
            yield return new WaitForSeconds (0.8f);
        }
    }

    public void sceneChange ()
    {
        if (Award > ewerd)
        {
            ewerd = Award;
            PlayerPrefs.SetInt ("Ewerd", ewerd);
        }
    }
}
