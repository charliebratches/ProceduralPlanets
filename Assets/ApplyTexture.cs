using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTexture : MonoBehaviour {

    public void Apply(Texture tex)
    {
        gameObject.GetComponent<Renderer>().material.SetTexture("_MyTexture", tex);
    }
}
