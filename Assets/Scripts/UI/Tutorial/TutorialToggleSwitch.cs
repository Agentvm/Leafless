using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialToggleSwitch : MonoBehaviour
{
    private Toggle toggle;
    //bool status_before = true;

    // Start is called before the first frame update
    void Start()
    {
        toggle = this.GetComponent<Toggle> ();
        toggle.isOn = SceneLoader.Instance.tutorial_toggle;
    }

    // Update is called once per frame
    void Update()
    {
        //if (status_before != toggle.isOn)
        //{
            SceneLoader.Instance.tutorial_toggle = toggle.isOn;
        //}
    }
}
