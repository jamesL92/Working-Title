using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
	public class GridManager : MonoBehaviour {

    //Prefab References
    [SerializeField] public GameObject walkableTilePrefab;
    [SerializeField] public GameObject unwalkableTilePrefab;
    [SerializeField] public GameObject castlePrefab;

		GridBuilder builder;
		void Awake() {
			builder = new StandardGridBuilder(this);
			builder.GenerateMap();
			builder.SpawnBuildings();
		}
	}
}
