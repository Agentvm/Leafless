using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStageSequence : MonoBehaviourSingleton<ColorStageSequence>
{
    [Tooltip("Configures the stage sequence for all ChangeColorScripts. Marks the points where the next material will be fully blended in. Set first entry to 0.")]
    [Range(0f, 1f)] [SerializeField] float[] _globalStageSequence;

    public float[] GlobalStageSequence { get => this._globalStageSequence;}
}
