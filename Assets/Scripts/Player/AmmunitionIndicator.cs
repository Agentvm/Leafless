using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionIndicator : MonoBehaviour
{

    [SerializeField] int indicator_number = 0;
    [SerializeField] Material deactivated_material;
    [SerializeField] Material active_material;
    Renderer _renderer = null;
    Movement MovementScriptReference = null;

    // Start is called before the first frame update
    void Start()
    {
        MovementScriptReference = this.transform.parent.GetComponent<Movement> ();
        _renderer = this.GetComponent<Renderer> ();

        if (!deactivated_material)
            _renderer.material.SetColor ("_Color", Color.white );
    }

    // Update is called once per frame
    void Update()
    {
        if (MovementScriptReference.Ammunition >= indicator_number)
        {
            if ( !active_material )
                _renderer.material.SetColor ("_Color", Color.red);
            else
                _renderer.material = active_material;
        }
        else
        {
            if ( !deactivated_material )
                _renderer.material.SetColor ("_Color", Color.white);
            else
                _renderer.material = deactivated_material;
        }
            
    }
}
