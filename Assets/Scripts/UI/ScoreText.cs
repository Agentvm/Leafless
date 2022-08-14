﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ScoreText : MonoBehaviour
{
    // Serialized Fields
    [SerializeField]
    [Range(0.1f, 2f)] float _riseHeight = 1.0f;
    [SerializeField]
    [Range(0.1f, 10f)] float _riseTimeSeconds = 1.0f;

    // Variables
    private float _riseTimeStart = -1f;
    private Vector3 _originPosition = Vector3.zero;
    private Color _tempColor;

    // Properties
    private bool _activated;
    public bool Activated
    {
        get => _activated;
        private set
        {
            _activated = value;
            if (value) _riseTimeStart = Time.time;
            else
            {
                _riseTimeStart = -1f;
                this.transform.position = _originPosition;
            }
        }
    }

    #region CachedProperties
    private TextMesh _textMesh;
    public TextMesh TextMesh
    {
        get
        {
            if (_textMesh == null) _textMesh = this.GetComponent<TextMesh>();
            return _textMesh;
        }
        set { _textMesh = value; }
    }

    private MeshRenderer _meshRenderer;
    public MeshRenderer MeshRenderer
    {
        get
        {
            if (_meshRenderer == null) _meshRenderer = this.GetComponent<MeshRenderer>();
            return _meshRenderer;
        }
        set { _meshRenderer = value; }
    }
    #endregion

    private async void Start()
    {
        this.TextMesh.text = "";
        _originPosition = this.transform.position;

        await Task.Delay(2000);
        this.ActivateAndFade();
    }

    private void Update()
    {
        if (Activated)
        {
            if (Time.time < _riseTimeStart + _riseTimeSeconds)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + Vector3.up * _riseHeight / 10, (Time.time - _riseTimeStart) / _riseTimeSeconds);
                _tempColor = this.TextMesh.color;
                _tempColor.a = 1 - ((Time.time - _riseTimeStart) / _riseTimeSeconds);
                this.TextMesh.color = _tempColor;
                Debug.Log("this.TextMesh.color.a: " + this.TextMesh.color.a);
            }
            else
                Activated = false;
        }
    }

    public void ActivateAndFade (int baseAwardValue = 1)
    {
        this.TextMesh.text = $"+ {(baseAwardValue * GameState.Instance.GameIntensity).ToString()}";
        Activated = true;
    }
}