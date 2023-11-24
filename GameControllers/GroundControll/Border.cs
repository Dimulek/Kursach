using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderer = new SpriteRenderer[4];

    public void BorderApear(in Vector3 vec) =>
        transform.position = vec;
}
