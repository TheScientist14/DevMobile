using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
	[SerializeField] private BackgroundObjects m_Objects;
	[SerializeField] private float m_ScrollSpeed = 1;
	[SerializeField] private int m_RenderLayer = 0;
	[SerializeField] private float m_TickDurationInSeconds = 1;
	[SerializeField] private float m_SpawnProbabilityPerTick = 0.05f;

	private Vector2 m_BottomLeft;
	private Vector2 m_TopRight;

	// Start is called before the first frame update
	void Start()
	{
		Camera camera = Camera.main;
		m_BottomLeft = camera.ViewportToWorldPoint(new Vector2(0, -1));
		m_TopRight = camera.ViewportToWorldPoint(Vector2.one);

		StartCoroutine(SpawnObjects());
	}

	private IEnumerator SpawnObjects()
	{
		var delay = new WaitForSecondsRealtime(m_TickDurationInSeconds);
		while(true)
		{
			if(Random.Range(0f, 1f) < m_SpawnProbabilityPerTick)
				SpawnObject();
			yield return delay;
		}
	}

	private void SpawnObject()
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = transform;
		gameObject.transform.localScale = Vector3.one;

		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		Sprite sprite = m_Objects.Sprites[Random.Range(0, m_Objects.Sprites.Count)];
		spriteRenderer.sprite = sprite;
		spriteRenderer.sortingOrder = m_RenderLayer;

		gameObject.transform.position = new Vector2(Random.Range(m_BottomLeft.x, m_TopRight.x), m_TopRight.y + sprite.bounds.extents.y);
	}

	// Update is called once per frame
	void Update()
	{
		float dist = Time.deltaTime * m_ScrollSpeed;
		foreach(Transform child in transform)
		{
			child.transform.position += Vector3.down * dist;
			if(child.transform.position.y < m_BottomLeft.y)
				Destroy(child.gameObject);
		}
	}
}
