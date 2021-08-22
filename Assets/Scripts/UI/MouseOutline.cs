using UnityEngine;

public class MouseOutline : MonoBehaviour
{
    // Components
    Renderer renderer_;
    Shader defaultShader;

    // Start is called before the first frame update
    void Start ()
    {
        renderer_ = GetComponent<Renderer> ();
        defaultShader = renderer_.material.shader;
    }

    private void OnMouseOver ()
    {
        renderer_.material.shader = Shader.Find ("Self-Illumin/Outlined Diffuse");
    }

    private void OnMouseExit ()
    {
        renderer_.material.shader = defaultShader;
        //renderer_.material.shader = Shader.Find ("Nature/SpeedTree8"); // Standard
    }


}
