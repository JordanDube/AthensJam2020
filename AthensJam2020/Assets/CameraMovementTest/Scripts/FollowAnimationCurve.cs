using System.Collections;
using CandyCoded;
using UnityEngine;
using UnityEngine.Events;

public class FollowAnimationCurve : MonoBehaviour {

  public enum ScaleTypes {
    SCALE_ON_Z_ONLY,
    SCALE_ON_X_AND_Y
  }
  
  public float spawnDelay;
  public bool triggerOnCall = false;
  
  [Tooltip("Note, you currently cannot check destination with useLocalPosition.")]
  public bool useLocalPosition = false;
  
  [Tooltip("If left blank, this will be set to the current GameObject.")]
  public GameObject gameObjectToManipulate;
  
  [Header("Animation Curve")]
  public bool useAnimationCurve = true;
  public CandyCoded.Vector3AnimationCurve animationCurve;

  [Header("Rotation Curve")]
  public bool useRotationCurve = false;
  
  public CandyCoded.Vector3AnimationCurve rotationCurve;

  public CandyCoded.Vector3AnimationCurve scaleCurve;
  public FollowAnimationCurve.ScaleTypes scaleTypes = ScaleTypes.SCALE_ON_Z_ONLY;

  public CandyCoded.GameObjectPoolReference entityObjectPoolReference;

  [SerializeField]
  [RangedSlider(-10, 10)]
  public RangedFloat randomXRange;
    
  [SerializeField]
  [RangedSlider(-10, 10)]
  public RangedFloat randomYRange;
  
  public UnityEvent events;

  private Vector3 destination;
  private float _initializationTime;
  
  private void Awake() {
    if (gameObjectToManipulate == null) {
      gameObjectToManipulate = this.gameObject;
    }

    CalculateDestination();
  }
  
  private void CalculateDestination() {
    if (useLocalPosition) {
      animationCurve.EditKeyframeValue(0, new Vector3(gameObjectToManipulate.transform.localPosition.x, randomYRange.Random(), 0));      
    } else {
      animationCurve.EditKeyframeValue(0, new Vector3(gameObjectToManipulate.transform.position.x, gameObjectToManipulate.transform.position.y, 0));      
    }
    
    destination = animationCurve.Evaluate(animationCurve.MaxTime()); 
  }
  
  private IEnumerator Start() {
    if (triggerOnCall) {
      yield break;
    }
    
    yield return new WaitForSeconds(spawnDelay);

    _initializationTime = Time.timeSinceLevelLoad;

    if (useAnimationCurve) {
      StartCoroutine(Animate());
    }

    if (useRotationCurve) {
      StartCoroutine(AnimateRotationCurve());
    }
  }

  public void ResetInitializationTime() {
    CalculateDestination();
    _initializationTime = Time.timeSinceLevelLoad;
  }

  private void EvaluatePosition(Vector3 newPosition) {
    if (useLocalPosition) {
      gameObjectToManipulate.transform.localPosition = newPosition;      
    } else {
      gameObjectToManipulate.transform.position = newPosition;
    }
  }

  private bool HasReachedDestination() {
    if (!useLocalPosition) {
      return gameObjectToManipulate.transform.position.Equals(destination)
             && events.GetPersistentEventCount() > 0;
    } else {
      return gameObjectToManipulate.transform.localPosition.Equals(destination)
             && events.GetPersistentEventCount() > 0;
    }
  }

  public IEnumerator Animate() {
    EvaluatePosition(animationCurve.Evaluate(Time.timeSinceLevelLoad - _initializationTime));

    if (HasReachedDestination()) {
      events.Invoke();
    } else {
      yield return null;
      StartCoroutine(Animate()); 
    }
  }

  public IEnumerator AnimateRotationCurve() {
    Vector3 rotation = rotationCurve.Evaluate(Time.timeSinceLevelLoad - _initializationTime);
    gameObjectToManipulate.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    
    yield return null;
    StartCoroutine(AnimateRotationCurve());
  }

  public void DestroyGameObjectToManipulate() {
    Destroy(gameObjectToManipulate);
  }
  
  public void DestroyGameObject() {
    Destroy(gameObject);
  }
  
  public void DestroySelf() {
    Destroy(this);
  }

  public void ReleaseFromPool() {
    entityObjectPoolReference.Destroy(this.gameObject);
  }
  
}
