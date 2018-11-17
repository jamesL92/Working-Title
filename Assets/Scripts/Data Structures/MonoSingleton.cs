using System.Collections;
using System.Collections.Generic;

namespace UnityEngine {
	public abstract class MonoSingleton<T> : MonoBehaviour where T:MonoBehaviour {

		public static T instance;

		protected virtual void Awake() {
			// Enforce singleton
			if (instance == null) instance = this as T;
			else Destroy(gameObject);

			DontDestroyOnLoad(gameObject);
		}

		void OnApplicationQuit() {
			instance = null;
		}
	}
}
