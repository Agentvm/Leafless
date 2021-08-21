using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // Components
    Renderer renderer_;
    Material startMaterial;

    // Costomization
    [Tooltip("Contains Materials that will be slowly blended into each other (color-wise)")]
    [SerializeField] Material[] materialStages;

    // Variables
    static float[] stageSequence;
    int activeColorStage = 0;
    float colorProgress;
    Material _activeMaterial;

    // This Script only changes Color once, at startup
    void Start()
    {
        // Get Components
        renderer_ = this.GetComponent<Renderer>();
        startMaterial = renderer_.material;
        colorProgress = arduinoMap(GameState.Instance.GameIntensity, 1f, GameState.Instance.MaxIntensity, 0f, 1f);
        stageSequence = new float[ColorStageSequence.Instance.GlobalStageSequence.Length];
        ColorStageSequence.Instance.GlobalStageSequence.CopyTo(stageSequence, 0);

        // Check Setup
        if (materialStages.Length == 0 || stageSequence.Length == 0)
            return;

        if (materialStages.Length > stageSequence.Length)
            System.Array.Resize(ref materialStages, stageSequence.Length);
        else if (stageSequence.Length > materialStages.Length)
            System.Array.Resize(ref stageSequence, materialStages.Length);

        // Set Progress
        while (activeColorStage + 1 < stageSequence.Length && colorProgress > stageSequence[activeColorStage + 1])
        {
            activeColorStage++;
        }

        // No more Material specified
        if(activeColorStage == stageSequence.Length - 1)
        {
            renderer_.material = materialStages[materialStages.Length - 1];
            return;
        }


        // Set Color according to Game Intensity
        if (activeColorStage < materialStages.Length - 1 && materialStages[activeColorStage] != null && materialStages[activeColorStage + 1] != null)
        {
            // Normalize
            if (colorProgress > stageSequence[activeColorStage])
                colorProgress -= stageSequence[activeColorStage];
            //Debug.Log("Normalized colorProgress: " + colorProgress);

            float stageValue = stageSequence[activeColorStage + 1] - stageSequence[activeColorStage];
            //Debug.Log("stageValues: " + stageValue);

            // Remap the progress to 0 - 1
            float progress = arduinoMap(colorProgress,
                0, stageValue,
                0, 1);

            Material blendedMaterial = blendMaterial(materialStages[activeColorStage], materialStages[activeColorStage + 1], progress);

            if (blendedMaterial != null)
            {
                //Debug.Log("blendedMaterial: " + blendedMaterial.ToString());
                renderer_.material = blendedMaterial;
            }
        }
    }

    // Lerp between two given Materials
    Material blendMaterial(Material fromMaterial, Material toMaterial, float mapValue)
    {
        //Debug.Log("mapValue: " + mapValue);
        //Debug.Log("fromMaterial: " + fromMaterial.color.ToString());
        //Debug.Log("toMaterial: " + toMaterial.color.ToString());

        _activeMaterial = new Material(fromMaterial);
        _activeMaterial.Lerp(fromMaterial, toMaterial, mapValue);
        //Debug.Log("_activeMaterial: " + _activeMaterial.color.ToString());
        return _activeMaterial;

    }

    // Maps x, that was originally between fromLow and fromHigh, to the corresponding value in the range between toLow and toHigh
    // Source: https://www.arduino.cc/reference/de/language/functions/math/map/
    float arduinoMap(float x, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (x - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
