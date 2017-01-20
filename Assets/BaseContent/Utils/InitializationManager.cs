using UnityEngine;
using System.Collections;

public class InitializationManager : Singleton<InitializationManager>
{
	public enum InitializationState {
		FirstRun,
		InProgress,
		Done
	}

	public InitializationState currentState {get;private set;}

	public void Init() {
		if (this.currentState != InitializationState.FirstRun) {
			return; // nothing to do ... in progress or already done
		}
		
		StartCoroutine(InitCoroutine());
	}

	IEnumerator InitCoroutine() {
		this.currentState = InitializationState.InProgress;		
		// start some services
		yield return null;
		
		this.currentState = InitializationState.Done;
	}
}

