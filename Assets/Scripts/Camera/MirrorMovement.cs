using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class MirrorMovement : MonoBehaviour
{

    [SerializeField] private Transform master;
    private Vector3 offset;

    // Cached Properties
    #region CachedProperties
    private Transform _player;
    public Transform Player
    {
        get
        {
            if (_player == null) _player = GameObject.FindObjectOfType<Movement>().transform;
            return _player;
        }
    }

    private Vector3 _offset;
    public Vector3 Offset
    {
        get
        {
            if (_offset == Vector3.zero) _offset = master.position - this.transform.position;
            return _offset;
        }
    }
    #endregion


    // Update is called once per frame
    void Update ()
    {
        if (master)
            this.transform.position = master.position - Offset;
        else master = Player;
    }
}
