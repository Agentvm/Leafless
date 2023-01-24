using System.Collections.Generic;
using UnityEngine;

public enum SceneIdentifiers
{
    Menu,
    Game

}


public class SceneLoader : MonoBehaviour
{
    // Static instance of SceneLoader which allows it to be accessed by any other script.
    public static SceneLoader Instance = null;

    private readonly Dictionary<SceneIdentifiers, string> _sceneNames =
        new Dictionary<SceneIdentifiers, string> { { SceneIdentifiers.Game, "Game" }, { SceneIdentifiers.Menu, "Menu" } };

    // Properties
    private string CurrentSceneName { get => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; }
    public SceneIdentifiers CurrentScene { get; set; }


    // Start is called before the first frame update
    void Awake()
    {
        // check that there is only one instance of this and that it is not destroyed on load
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public string GetSceneName(SceneIdentifiers sceneIdentifier)
    {
        return _sceneNames[sceneIdentifier];
    }

    public void loadNextScene()
    {
        GameState.Instance.sceneChange();

        if (CurrentSceneName == GetSceneName(SceneIdentifiers.Menu))
            CurrentScene = (SceneIdentifiers.Game);
        else
            CurrentScene = (SceneIdentifiers.Menu);
        UnityEngine.SceneManagement.SceneManager.LoadScene(GetSceneName(CurrentScene));

    }
}
