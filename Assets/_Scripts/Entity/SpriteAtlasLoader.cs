using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[System.Serializable]
public class SpriteLoader
{
    [SerializeField]
    SpriteAtlas m_Atlas;
    [SerializeField]
    string m_SpriteName;

    Sprite m_Sprite;

    public Sprite Sprite {  get { if (m_Sprite) return m_Sprite; else m_Sprite = m_Atlas.GetSprite(m_SpriteName); return m_Sprite; } }
}

public class SpriteAtlasLoader : MonoBehaviour
{
    [SerializeField]
    SpriteLoader m_SpriteLoader;

    private void Awake()
    {
        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        if (SR != null)
        {
            SR.sprite = m_SpriteLoader.Sprite;
        }
    }

}
