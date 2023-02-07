using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    // Static instance of GameState which allows it to be accessed by any other script.
    public static GameState Instance = null;

    private int award = 0;
    private int ewerd = 0;
    private Text award_text;

    // game difficulty / intensity
    private Vector3 origin_position = new Vector3(0f, 0.5f, 0f);
    private float maxIntensity = 3.0f;

    public int Award { get => award; private set => award = value; }

    // document this formula - fast
    public float GameIntensity { get => getGameIntensity(); }
    public float MaxIntensity { get => maxIntensity; }

    // Cached Properties
    #region CachedProperties
    private static GameObject _scoreTextPrefab;
    public static GameObject ScoreTextPrefab
    {
        get
        {
            if (_scoreTextPrefab == null)
                _scoreTextPrefab = Resources.Load(nameof(ScoreText)) as GameObject;
            return _scoreTextPrefab;
        }
    }

    private Transform _playerTransform;
    public Transform PlayerTransform
    {
        get
        {
            if (_playerTransform == null)
                _playerTransform = GameObject.FindObjectOfType<Movement>().transform;
            return _playerTransform;
        }
    }
    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        // check that there is only one instance of this and that it is not destroyed on load
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("Ewerd"))
            ewerd = PlayerPrefs.GetInt("Ewerd");

        StartCoroutine(logIntensity());
    }

    private void Update()
    {
        if (award_text)
            if (SceneLoader.Instance.CurrentScene == SceneIdentifiers.Menu)
            {
                award_text.text = "Maximum Score: " + ewerd.ToString();
                award = 0;
            }
            else
                award_text.text = "Score: " + Award.ToString();
        else
        {
            award_text = GameObject.FindWithTag("Score").GetComponent<Text>();
        }
    }

    private float getGameIntensity()
    {
        if (!PlayerTransform) return 1f;
        float playerOriginDistance = Vector3.Distance(PlayerTransform.position, origin_position);
        //return Mathf.Min (Mathf.Max (Vector3.Distance (player_transform.position, origin_position) /
        //                  (100f + 2f * Vector3.Distance (player_transform.position, origin_position)) * 4,
        //                  1f), maxIntensity);

        // Minimum Intensity is 1
        // It rises exponentially from below 1 to maxIntensity
        return Mathf.Min(Mathf.Max(0.219f * Mathf.Pow(playerOriginDistance, 0.35f), 1f), maxIntensity);
    }

    public static void AddScore(Vector3 position, int scoreValue)
    {
        // Add Intensity bonus
        scoreValue = (int)(scoreValue * Instance.GameIntensity);

        // Add Score
        if (scoreValue <= 10) Instance.Award += scoreValue;

        // Display the added score
        Instantiate(ScoreTextPrefab, position, Quaternion.identity).
            GetComponent<ScoreText>().DisplayValue = scoreValue;
    }

    IEnumerator logIntensity()
    {
        while (true)
        {
            //if ( player_transform )
            //Debug.Log ("Distance/Intensity: " + Vector3.Distance (player_transform.position, origin_position) + " / " + getGameIntensity ());
            yield return new WaitForSeconds(0.8f);
        }
    }

    public void sceneChange()
    {
        if (Award > ewerd)
        {
            ewerd = Award;
            PlayerPrefs.SetInt("Ewerd", ewerd);
        }
    }
}
