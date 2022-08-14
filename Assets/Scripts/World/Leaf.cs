using System.Collections;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    // Constants
    public const int BaseAwardValue = 1;

    // Components
    AudioSource _audioSource;
    [SerializeField] AudioClip[] _regrowSounds;
    [SerializeField] AudioClip[] _getEatenSounds;
    Renderer _renderer;
    Rigidbody _rigidBody;

    // References
    Growth GrowthScriptReference;

    // Transform
    Vector3 _originalPosition;
    Quaternion _originalRotation;
    Vector3 _originalScale;
    Vector3 _originalPanelPosition = new Vector3 (0, 0, 0);

    // Properties
    public bool AboutToBeDestructed { get; private set; }

    //// Start is called before the first frame update
    void Start ()
    {
        // Get Components
        _audioSource = this.GetComponent<AudioSource> ();
        _renderer = this.GetComponent<Renderer> ();
        _rigidBody = this.GetComponent<Rigidbody> ();

        // Get References
        GrowthScriptReference = this.transform.parent.parent.GetComponent<Growth> ();

        // save original transform values
        _originalPosition = this.transform.position;
        _originalRotation = this.transform.rotation;
        _originalScale = this.transform.localScale;
        if (this.transform.parent.parent.parent)
            _originalPanelPosition = this.transform.parent.parent.parent.transform.position;
    }

    public void ReGrow ()
    {
        // reset transform
        Vector3 panel_correction = new Vector3 (0, 0, 0);
        if (this.transform.parent.parent.parent)
            panel_correction = this.transform.parent.parent.parent.transform.position - _originalPanelPosition;
        this.transform.position = _originalPosition + panel_correction;
        this.transform.rotation = _originalRotation;
        _rigidBody.isKinematic = false;

        // activate object and play sound
        this.gameObject.SetActive (true);
        if (_regrowSounds.Length > 0) PlayAudio(_regrowSounds[Random.Range (0, _regrowSounds.Length)]);

        // play grow animation
        StopAllCoroutines ();
        StartCoroutine (GrowCoroutine ());
    }

    IEnumerator GrowCoroutine ()
    {
        for (float strength = 0; strength < 1; strength += 0.01f)
        {
            // change alpha
            //Color color = renderer.material.color;
            //color.a = strength;

            // change size
            this.transform.localScale = _originalScale * strength;

            yield return new WaitForSeconds (.1f);
        }
    }

    public void GetEaten ()
    {
        // play get eaten animation
        _rigidBody.isKinematic = true;
        StopAllCoroutines ();
        StartCoroutine (GetEatenCoroutine ());

        // audio
        if (_getEatenSounds.Length > 0) PlayAudio (_getEatenSounds[Random.Range (0, _getEatenSounds.Length)]);

        // logic
        GrowthScriptReference.noticeEatenLeaf (this.transform);
        if (GameState.Instance)
        {
            GameState.AddScore(this.transform.position, BaseAwardValue);
        }
    }

    IEnumerator GetEatenCoroutine ()
    {
        for (float strength = 1; strength > 0; strength -= 0.01f)
        {
            // change alpha
            //renderer.material.color *= strength;

            // change size
            this.transform.localScale *= strength;
            this.transform.Translate (this.transform.forward * 15f * Time.deltaTime);

            yield return new WaitForSeconds (.02f);
        }
    }

    void PlayAudio (AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play ();
    }

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.transform.tag == "PlantBody" || collision.transform.tag == "Interactable")
        {
            // Destroy Leaves that collide with each other
            if (collision.transform.tag == "Interactable")
            {
                Leaf otherLeaf = collision.GetComponent<Leaf> ();
                if (!otherLeaf.AboutToBeDestructed)
                {
                    Destroy (this.transform.parent.gameObject);
                    if (GrowthScriptReference)
                        GrowthScriptReference.destroyOneLeaf ();
                }
            }
            else
                Destroy (this.transform.parent.gameObject);
        }
    }

    private void OnDestroy ()
    {
        AboutToBeDestructed = true;
    }
}
