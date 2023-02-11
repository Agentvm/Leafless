using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Leafless.UI;

public class GameState : MonoBehaviourSingleton<GameState>
{
    public const float MinIntensity = 1.0f;
    public const float MaxIntensity = 3.0f;
    private const string PlayerPrefsScoreString = "Award";

    private int _inGameScore = 0;
    private int _highScore = 0;
    private Text _awardText;

    // game difficulty / intensity
    private Vector3 _originPosition = new Vector3(0f, 0.5f, 0f);
    
    // Properties
    public int InGameScore { get => _inGameScore; private set => _inGameScore = value; }
    public float GameIntensity { get => getGameIntensity(); }

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


    public override void Awake()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsScoreString))
            _highScore = PlayerPrefs.GetInt(PlayerPrefsScoreString);
    }

    private void Update()
    {
        if (_awardText)
            if (SceneLoader.Instance.CurrentScene == SceneIdentifiers.Menu)
            {
                _awardText.text = "Maximum Score: " + _highScore.ToString();
                _inGameScore = 0;
            }
            else
                _awardText.text = "Score: " + InGameScore.ToString();
        else
        {
            _awardText = GameObject.FindWithTag("Score").GetComponent<Text>();
        }
    }

    // GameIntensity rises exponentially from below 1 (clipped to min 1) to above maxIntensity (clipped to maxIntensity)
    // Scales Score awards as well as enemy movement & spawnrate.
    // Also has an effect on the way plants grow
    private float getGameIntensity()
    {
        if (!PlayerTransform) return 1f;

        // Minimum Intensity is 1
        float playerOriginDistance = Vector3.Distance(PlayerTransform.position, _originPosition);
        return Mathf.Min(Mathf.Max(0.219f * Mathf.Pow(playerOriginDistance, 0.35f), MinIntensity), MaxIntensity);

        //return Mathf.Min (Mathf.Max (Vector3.Distance (player_transform.position, origin_position) /
        //                  (100f + 2f * Vector3.Distance (player_transform.position, origin_position)) * 4,
        //                  1f), maxIntensity);
    }

    public void AddScore(Vector3 position, int scoreValue)
    {
        // Add Intensity bonus
        scoreValue = (int)(scoreValue * Instance.GameIntensity);

        // Add Score
        if (scoreValue <= 10) InGameScore += scoreValue;

        // Display the added score
        Instantiate(ScoreTextPrefab, position, Quaternion.identity).
            GetComponent<ScoreText>().DisplayValue = scoreValue;
    }

    public void sceneChange()
    {
        if (InGameScore > _highScore)
        {
            _highScore = InGameScore;
            PlayerPrefs.SetInt(PlayerPrefsScoreString, _highScore);
        }
    }
}
