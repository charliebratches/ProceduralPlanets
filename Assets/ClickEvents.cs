using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvents : MonoBehaviour
{

    private void OnMouseOver()
    {
        Debug.Log("enter");
        Debug.Log(gameObject.name);
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }

    void OnMouseDown()
    {
        Debug.Log("click");
        Debug.Log(gameObject.name);
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }

    private void OnMouseExit()
    {
        Debug.Log("exit");
    }
}
