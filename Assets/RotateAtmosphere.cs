using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAtmosphere : MonoBehaviour {

    public float speed = 27f;

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
