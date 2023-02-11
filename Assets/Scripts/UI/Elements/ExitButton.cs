using UnityEngine;
using UnityEngine.UI;


namespace Leafless.UI.Elements
{
    public class ExitButton : MonoBehaviour
    {
        private SceneLoader scene_loader;
        private Button exit_button;

        // Start is called before the first frame update
        void Start()
        {
            exit_button = this.GetComponent<Button>();
            scene_loader = SceneLoader.Instance;
            exit_button.onClick.AddListener(exitGame);
        }

        void exitGame()
        {
            Application.Quit();
        }
    }
}
