using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // Static instance of SceneLoader which allows it to be accessed by any other script.
    public static SceneLoader Instance = null;

    private int award = 0;
    private int ewerd = 0;
    private Text award_text;
    private string current_scene_name = "";
    public bool tutorial_toggle = true;
    public bool tutorial_completed = false;

    public int Award { get => award; set => award += value; }

    // Start is called before the first frame update
    void Awake ()
    {
        // check that there is only one instance of this and that it is not destroyed on load
        if ( Instance == null )
            Instance = this;
        else if ( Instance != this )
            Destroy (gameObject);
        DontDestroyOnLoad (gameObject);

        if (PlayerPrefs.HasKey ("Ewerd"))
            ewerd = PlayerPrefs.GetInt ("Ewerd");
        current_scene_name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    //private void Start ()
    //{
    //    award_text = GameObject.FindWithTag ("Score").GetComponent<Text> ();
    //}

    private void Update ()
    {
        if ( award_text )
            if ( current_scene_name == "Menu")
            {
                award_text.text = "Maximum Score: " + ewerd.ToString ();
                award = 0;
            }
            else
                award_text.text = "Score: " + Award.ToString ();
        else
        {
            award_text = GameObject.FindWithTag ("Score").GetComponent<Text> ();
            current_scene_name = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
        }
            
    }

    public void loadNextScene ()
    {
        if ( Award > ewerd )
        {
            ewerd = Award;
            PlayerPrefs.SetInt ("Ewerd", ewerd);
        }

        if ( current_scene_name == "Menu" )
            UnityEngine.SceneManagement.SceneManager.LoadScene ("SampleScene");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu");
    }
}
