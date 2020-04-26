using System;
using UnityEngine;

public class LevelGeneratorListener : MonoBehaviour {

  public float xDifference;
  
  public LevelGenerator levelGenerator;

  private SpriteRenderer _SpriteRenderer;

  private void Awake() {
    _SpriteRenderer = GetComponent<SpriteRenderer>();
    Debug.Log(Camera.main.ScreenToWorldPoint(Camera.main.transform.position));
    Debug.Log(Camera.main.transform.position);
  }

  private void Update() {
    if (_SpriteRenderer) {
      if (_SpriteRenderer.isVisible) {
        SpawnSegmentAndMoveTransform();
      }
    }
  }

  private void SpawnSegmentAndMoveTransform() {
    transform.position = new Vector3(Camera.main.transform.position.x + xDifference, transform.position.y, 0f);
    levelGenerator.SpawnAnotherSegment();
  }

}
