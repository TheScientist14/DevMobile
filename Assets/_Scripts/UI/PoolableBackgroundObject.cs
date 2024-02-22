using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PoolableBackgroundObject : Poolable
{
    public SpriteRenderer SpriteRenderer;
}
