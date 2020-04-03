﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
    private float player_origin_distance;
    
    // tutorial state
    public bool tutorial_toggle = true;
    public bool tutorial_completed = false;

    public int Award { get => award; set => award += Mathf.Min (value, 30); } // max 30 award points for one action
    public float PlayerOriginDistance { get => Vector3.Distance (player_transform.position, origin_position);}

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
    }

    private void Start ()
    {
        if ( GameObject.FindWithTag ("Player") )
            player_transform = GameObject.FindWithTag ("Player").transform;
        Debug.Log ("Game Core Started.");
    }

    private void Update ()
    {
        if ( award_text )
            if ( SceneLoader.Instance.CurrentSceneName == "Menu")
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

    public void sceneChanged ()
    {
        if ( Award > ewerd )
        {
            ewerd = Award;
            PlayerPrefs.SetInt ("Ewerd", ewerd);
        }
    }
}
