using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
	public class GridManager : MonoSingleton<GridManager> {

		public List<GameObject> buildings = new List<GameObject>();
		public List<GameObject> units = new List<GameObject>();

		GridBuilder builder;


		protected override void Awake() {
			base.Awake();
			builder = new StandardGridBuilder(this);
			builder.GenerateMap();
			builder.SpawnBuildings();
			builder.SpawnUnits();
		}
	}
}
