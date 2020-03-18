using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOutline : MonoBehaviour
{
    // Components
    Renderer renderer_;

    // Start is called before the first frame update
    void Start()
    {
        renderer_ = GetComponent<Renderer> ();
    }

    private void OnMouseOver ()
    {
        renderer_.material.shader = Shader.Find ("Self-Illumin/Outlined Diffuse");
    }

    private void OnMouseExit ()
    {
        renderer_.material.shader = Shader.Find ("Nature/SpeedTree8"); // Standard
    }


}
