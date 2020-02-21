using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour
{
    //
    Movement MovementScriptReference;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        MovementScriptReference = GetComponent<Movement> ();
        animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        do approach and eat leaf
    }

    public void eatLeaf (Transform leaf )
    {
        MovementScriptReference.MovementDisabled = true;


        //animator.SetBool ("Eating", true);
        //else animator.SetBool ("Eating", false);
    }
}
