using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOutline : MonoBehaviour
{
    // Components
    Renderer renderer_;
    Animator animator;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        renderer_ = GetComponent<Renderer> ();

        if ( !GameObject.FindWithTag ("Player") ) return;
        player = GameObject.FindWithTag ("Player").transform;
        animator = player.GetComponent<Animator> ();
    }

    private void OnMouseOver ()
    {
        if (player)
        {
            renderer_.material.shader = Shader.Find ("Self-Illumin/Outlined Diffuse");
            animator.SetBool ("HoverOverInteractable", true);
        }
    }

    private void OnMouseExit ()
    {
        if ( player )
        {
            renderer_.material.shader = Shader.Find ("Nature/SpeedTree8"); // Standard
            animator.SetBool ("HoverOverInteractable", false);
        }
    }


}
