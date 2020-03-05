using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionIndicator : MonoBehaviour
{

    [SerializeField] int indicator_number = 0;
    [SerializeField] Material deactivated_material;
    [SerializeField] Material active_material;
    Renderer renderer = null;
    Shoot shootScriptReference = null;

    // Start is called before the first frame update
    void Start()
    {
        shootScriptReference = this.transform.parent.GetComponent<Shoot> ();
        renderer = this.GetComponent<Renderer> ();

        if (!deactivated_material)
            renderer.material.SetColor ("_Color", Color.white );
    }

    // Update is called once per frame
    void Update()
    {
        if (shootScriptReference.Ammunition >= indicator_number)
        {
            if ( !active_material )
                renderer.material.SetColor ("_Color", Color.red);
            else
                renderer.material = active_material;
        }
        else
        {
            if ( !deactivated_material )
                renderer.material.SetColor ("_Color", Color.white);
            else
                renderer.material = deactivated_material;
        }
            
    }
}
