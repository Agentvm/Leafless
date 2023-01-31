using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private SceneLoader scene_loader;
    private Button start_button;

    // Start is called before the first frame update
    void Start ()
    {
        start_button = this.GetComponent<Button> ();
        scene_loader = SceneLoader.Instance;
        start_button.onClick.AddListener (loadScene);
    }

    void loadScene ()
    {
        scene_loader.loadNextScene ();
    }
}
