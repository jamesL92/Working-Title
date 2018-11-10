using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
	public class GridManager : MonoBehaviour {

    //Prefab References
    public GameObject walkableTilePrefab;
    public GameObject unwalkableTilePrefab;
    public GameObject castlePrefab;

		public List<GameObject> buildings = new List<GameObject>();

		GridBuilder builder;


		void Awake() {
			builder = new StandardGridBuilder(this);
			builder.GenerateMap();
			builder.SpawnBuildings();
		}
	}
}
