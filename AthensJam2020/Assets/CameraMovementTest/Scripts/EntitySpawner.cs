using System.Collections;
using CandyCoded;
using UnityEngine;

public class EntitySpawner : MonoBehaviour {
    
    [SerializeField]
    [RangedSlider(0, 10)]
    public RangedFloat randomWaitTimeRange;
    
    [SerializeField]
    [RangedSlider(-10, 10)]
    public RangedFloat randomXRange;
    
    [SerializeField]
    [RangedSlider(-10, 10)]
    public RangedFloat randomYRange;

    public bool spawnBeyondCamera = true;
    
    public CandyCoded.GameObjectPoolReference entityObjectPoolReference;

    private void Awake() {
        if (entityObjectPoolReference) {
            entityObjectPoolReference.parentTransform = transform;
            entityObjectPoolReference.Populate();

            StartCoroutine(SpawnEntity());   
        }
    }

    IEnumerator SpawnEntity() {
        yield return new WaitForSeconds(randomWaitTimeRange.Random());

        if (spawnBeyondCamera) {
            entityObjectPoolReference.Spawn(new Vector3(Camera.main.transform.position.x + 10, randomYRange.Random(), 0f), Quaternion.identity);
        }
        else {
            entityObjectPoolReference.Spawn(new Vector3(randomXRange.Random(), randomYRange.Random(), 0f), Quaternion.identity);
        }
        
        StartCoroutine(SpawnEntity());
    }
    
}
