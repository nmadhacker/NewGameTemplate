using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

  public float timeToSelfDestruct = 1f;
  
  void Start() {
    
    StartCoroutine(WaitAndDie());
  }

  IEnumerator WaitAndDie() {

    float elapsed = 0f;
    while (elapsed < timeToSelfDestruct) {      
      yield return null;
      elapsed += Time.deltaTime;
    }

    GameObject.Destroy(this.gameObject);
    
  }
	
}
