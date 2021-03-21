using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // Components
    Renderer renderer_;
    Material startMaterial;

    // Costomization
    [Tooltip ("Contains Materials that will be slowly blended into each other (color-wise)")]
    [SerializeField]Material[] materialStages;
    [Tooltip ("Marks the relative point that the corresponding material will be fully blended in. Must have the same number of entries as materialStages.")]
    [Range (0f, 1f)][SerializeField]float[] stageSequence;

    // Logic
    int activeColorStage = 0;
    float colorProgress;

    // Start is called before the first frame update
    void Start()
    {
        renderer_ = this.GetComponent<Renderer> ();
        startMaterial = renderer_.material;
        colorProgress = arduinoMap (GameState.Instance.getGameIntensity (), 1f, GameState.Instance.MaxIntensity, 0f, 1f);

        if ( materialStages.Length == 0 || stageSequence.Length == 0 || materialStages.Length != stageSequence.Length )
            return;

        
        Debug.Log ("colorProgress: " + colorProgress);
        Debug.Log ("stageSequence[activeColorStage]: " + stageSequence[activeColorStage]);

        // Set Color according to Game Intensity
        while ( colorProgress > stageSequence[activeColorStage] )
        {
            Debug.Log ("activeColorStage: " + activeColorStage);
            activeColorStage++;
        }

        if ( activeColorStage == 0 )
            renderer_.material = blendMaterial (startMaterial, materialStages[0], stageSequence[0]/colorProgress);
        else
            renderer_.material = blendMaterial (materialStages[activeColorStage - 1], materialStages[activeColorStage], stageSequence[activeColorStage] / colorProgress);
    }

    //
    Material blendMaterial ( Material fromMaterial, Material toMaterial, float mapValue )
    {
        Material activeMaterial = new Material (fromMaterial);
        activeMaterial.Lerp (fromMaterial, toMaterial, mapValue);
        //Debug.Log ("Blending " + fromMaterial.name + " and " + toMaterial.name + " into " + activeMaterial.name);
        //Debug.Log ("Color r: " + fromMaterial.color.r + " and " + toMaterial.color.r + " into " + activeMaterial.color.r);
        return activeMaterial;
        
    }

    // 
    //Color mapColor (Color minColor, Color maxColor, float mapValue)
    //{
    //    // According from 
    //    Color resultingColor = new Color ();
    //    resultingColor.r = arduinoMap (mapValue, 0, 1, minColor.r, maxColor.r);
    //    resultingColor.g = arduinoMap (mapValue, 0, 1, minColor.g, maxColor.g);
    //    resultingColor.b = arduinoMap (mapValue, 0, 1, minColor.b, maxColor.b);
    //    resultingColor.a = minColor.a;

    //    //Debug.Log ("colorProgress: " + colorProgress);
    //    //Debug.Log ("minColor.r: " + minColor.r);
    //    //Debug.Log ("maxColor.r: " + maxColor.r);
    //    //Debug.Log ("resultingColor.r: " + resultingColor.r);

    //    return resultingColor;
    //}

    // Maps x, that was originally between fromLow and fromHigh, to the corresponding value in the range between toLow and toHigh
    // Source: https://www.arduino.cc/reference/de/language/functions/math/map/
    float arduinoMap ( float x, float fromLow, float fromHigh, float toLow, float toHigh )
    {
        return (x - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
