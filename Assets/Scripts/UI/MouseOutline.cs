using UnityEngine;

public class MouseOutline : MonoBehaviour
{
    // Components
    Renderer renderer_;
    Material defaultMaterial = null;
    //Shader _newShader;
    [SerializeField]
    Material _mouseOverMaterial;

    // Start is called before the first frame update
    void Start ()
    {
        renderer_ = GetComponent<Renderer> ();
    }

    private void OnMouseOver ()
    {
        if (defaultMaterial == null)
            defaultMaterial = renderer_.material;
        renderer_.material = _mouseOverMaterial;
    }

    private void OnMouseExit ()
    {
        renderer_.material = defaultMaterial;
        defaultMaterial = null;
    }


}
