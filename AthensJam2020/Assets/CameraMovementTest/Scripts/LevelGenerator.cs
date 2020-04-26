using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {
    
    public List<GameObject> levelSegments;
    public List<GameObject> currentSegments;

    public float xDifference;
    public float spawnY;

    public void SpawnAnotherSegment () {
        Vector3 spawnPoint = new Vector3(currentSegments[currentSegments.Count - 1].transform.position.x + xDifference, spawnY, 0f);
        
        GameObject newSegment = Instantiate(levelSegments[Random.Range(0, levelSegments.Count)], spawnPoint, Quaternion.identity);
        newSegment.transform.parent = transform;
        
        currentSegments.Add(newSegment);
    }
}
