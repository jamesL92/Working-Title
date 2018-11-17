using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {

	Button[] buttons;
	public float distanceFromCenter;

	void Start() {
		buttons = GetComponentsInChildren<Button>();
	}

	void Update() {
		int numButtons = buttons.Length;
		float angleBetween = 360f / numButtons;

		for(int i=0; i < numButtons; i++) {
			buttons[i].transform.position = transform.position + new Vector3(
				distanceFromCenter * Mathf.Cos(Mathf.Deg2Rad * -angleBetween * i),
				distanceFromCenter * Mathf.Sin(Mathf.Deg2Rad * -angleBetween * i),
				transform.position.z
			);
		}
	}
}
