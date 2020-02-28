using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Grow ()
    {
        this.gameObject.SetActive (true);

        // play grow animation
    }

    public void getEaten ()
    {
        // play get eaten animation
        this.gameObject.SetActive (false);
        if ( this.transform.parent && this.transform.parent.parent )
            this.transform.parent.parent.GetComponent<Growth> ().noticeEatenLeaf (this.transform);
    }
}
