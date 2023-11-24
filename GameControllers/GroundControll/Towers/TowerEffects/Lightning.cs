using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private Material[] textures;

    private int animationStep;

    [SerializeField]
    private float fps = 30f;

    private float fpsCounter;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            ++animationStep;
            if (animationStep == textures.Length)
                animationStep = 0;

            lineRenderer.material = textures[animationStep];

            fpsCounter = 0f;
        }
    }
}
