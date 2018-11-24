using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChildRotationController : MonoBehaviour {

	[SerializeField] private Vector3 rotation;
	// Update is called once per frame
	void Update () {
		foreach(Transform child in transform) {
			child.rotation = Quaternion.Euler(rotation);
		}
	}
}
