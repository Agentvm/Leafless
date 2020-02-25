using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    [Range (1f, 30f)][SerializeField]float bullet_speed = 10f;
    bool stop_translating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( stop_translating )
        {
            this.transform.Translate (Vector3.zero);
            return;
        }

        this.transform.Translate (this.transform.forward * bullet_speed * Time.deltaTime, Space.World);
    }

    public void stopTranslating ()
    {
        stop_translating = true;
    }
}
