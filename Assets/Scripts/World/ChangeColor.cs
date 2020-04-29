using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    Renderer renderer_;
    Color minColor;
    [SerializeField]Color intermediateColor;
    [SerializeField]Color maxColor;

    float colorProgress;

    // Emission
    [SerializeField]float maxEmission = 1;

    // Start is called before the first frame update
    void Start()
    {
        renderer_ = this.GetComponent<Renderer> ();
        minColor = renderer_.material.color;
        colorProgress = arduinoMap (GameState.Instance.getGameIntensity (), 1f, GameState.Instance.MaxIntensity, 0f, 1f);

        // Enable Emission map
        renderer_.material.EnableKeyword ("_EMISSION");

        // Set Color according to Game Intensity
        if (colorProgress < 0.5f)
            renderer_.material.color = mapColor (minColor, intermediateColor, colorProgress);
        else
            renderer_.material.color = mapColor (intermediateColor, maxColor, colorProgress);

        // Add emission
        renderer_.material.SetColor ("_EmissionColor", renderer_.material.color * arduinoMap (colorProgress, 0, 1, 0, maxEmission));
    }

    // 
    Color mapColor (Color minColor, Color maxColor, float mapValue)
    {
        // According from 
        Color resultingColor = new Color ();
        resultingColor.r = arduinoMap (mapValue, 0, 1, minColor.r, maxColor.r);
        resultingColor.g = arduinoMap (mapValue, 0, 1, minColor.g, maxColor.g);
        resultingColor.b = arduinoMap (mapValue, 0, 1, minColor.b, maxColor.b);
        resultingColor.a = minColor.a;

        //Debug.Log ("colorProgress: " + colorProgress);
        //Debug.Log ("minColor.r: " + minColor.r);
        //Debug.Log ("maxColor.r: " + maxColor.r);
        //Debug.Log ("resultingColor.r: " + resultingColor.r);

        return resultingColor;
    }

    // Maps x, that was originally between fromLow and fromHigh, to the corresponding value in the range between toLow and toHigh
    // Source: https://www.arduino.cc/reference/de/language/functions/math/map/
    float arduinoMap ( float x, float fromLow, float fromHigh, float toLow, float toHigh )
    {
        return (x - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
