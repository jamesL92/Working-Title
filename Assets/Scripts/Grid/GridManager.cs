using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
	public class GridManager : MonoBehaviour {
    //Prefab References
    public GameObject walkableTilePrefab;
    public GameObject unwalkableTilePrefab;
    public GameObject castlePrefab;
    public GameObject unitPrefab;

		public List<GameObject> buildings = new List<GameObject>();
		public List<GameObject> units = new List<GameObject>();

		GridBuilder builder;


		void Awake() {
			builder = new StandardGridBuilder(this);
			builder.GenerateMap();
			builder.SpawnBuildings();
			builder.SpawnUnits();
		}
	}
}
