using UnityEngine;

// Todo: find a more performant way to close this
public class Credits : MonoBehaviour
{
    [SerializeField]
    GameObject _menuPanel = null;

    private void Start ()
    {
        this.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonUp (0) || Input.anyKeyDown)
        {
            if (_menuPanel != null)
                _menuPanel.SetActive (true);
            this.gameObject.SetActive (false);
        }
    }
}
