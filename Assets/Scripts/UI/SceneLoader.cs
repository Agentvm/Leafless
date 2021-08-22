using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // Static instance of SceneLoader which allows it to be accessed by any other script.
    public static SceneLoader Instance = null;

    private string current_scene_name = "";

    public string CurrentSceneName { get => current_scene_name; }

    // Start is called before the first frame update
    void Awake ()
    {
        // check that there is only one instance of this and that it is not destroyed on load
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy (gameObject);
        DontDestroyOnLoad (gameObject);

        current_scene_name = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
    }

    //private void Start ()
    //{
    //    award_text = GameObject.FindWithTag ("Score").GetComponent<Text> ();
    //}

    private void Update ()
    {
        // See if it works without this
        if (current_scene_name != UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name)
            current_scene_name = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
    }

    public void loadNextScene ()
    {
        GameState.Instance.sceneChange ();

        if (current_scene_name == "Menu")
            UnityEngine.SceneManagement.SceneManager.LoadScene ("SampleScene");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu");
    }
}
