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
    
    public CandyCoded.GameObjectPoolReference entityObjectPoolReference;

    private void Awake() {
        entityObjectPoolReference.parentTransform = transform;
        entityObjectPoolReference.Populate();

        StartCoroutine(SpawnEntity());
    }

    IEnumerator SpawnEntity() {
        yield return new WaitForSeconds(randomWaitTimeRange.Random());
        entityObjectPoolReference.Spawn(new Vector3(randomXRange.Random(), randomYRange.Random(), 0f), Quaternion.identity);
        StartCoroutine(SpawnEntity());
    }
    
}
