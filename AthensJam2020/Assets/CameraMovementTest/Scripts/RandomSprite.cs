using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class RandomSprite : MonoBehaviour {

    private SpriteRenderer _SpriteRenderer;
    public List<Sprite> randomSprites;

    private void Awake() {
        _SpriteRenderer = GetComponent<SpriteRenderer>();

        _SpriteRenderer.sprite = randomSprites[Random.Range(0, randomSprites.Count)];
    }
}
