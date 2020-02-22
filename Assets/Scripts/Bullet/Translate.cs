using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    [Range (1f, 30f)][SerializeField]float bullet_speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate (this.transform.forward * bullet_speed * Time.deltaTime, Space.World);
    }
}
