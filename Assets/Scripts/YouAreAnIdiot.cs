using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class YouAreAnIdiot : MonoBehaviour
{
    public Material materialBlack;
    public Material materialWhite;
    public Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            renderer.material = materialBlack;
            yield return new WaitForSeconds(2);
            renderer.material = materialWhite;
            yield return new WaitForSeconds(2);
        }
    }
}
