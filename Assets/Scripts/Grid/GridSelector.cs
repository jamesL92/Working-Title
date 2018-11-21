using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame {
    public class Selector : MonoBehaviour
    {
        public Coordinate? GetCoordinateFromClick(int layerMask=Physics.DefaultRaycastLayers) {
             if(Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask)){
					return GridManager.instance.WorldPosToCoordinate(hit.point);
				}
            }
			return null;
        }
        public Tile GetTileFromClick() {
			// Layermask ignores all but the Tile layer
            int layerMask = LayerMask.GetMask("Tile");
            
            Coordinate? clickedCoordinate = GetCoordinateFromClick(layerMask);
            if (clickedCoordinate != null) {
                Debug.Log("Tile coordinate retrieved, getting object");
                return GridManager.instance.GetTileFromCoord((Coordinate)clickedCoordinate);
            }
			return null;
		}

        public GridOccupier GetOccupierFromClick() {
            // Layermask ignores all but the Tile layer
            int layerMask = LayerMask.GetMask("Tile");

            Coordinate? clickedCoordinate = GetCoordinateFromClick(layerMask);
            if (clickedCoordinate != null) {
                Debug.Log("Tile coordinate retrieved, getting object");
                return GridManager.instance.GetOccupierFromCoord((Coordinate)clickedCoordinate);
            }
			return null;
        }
    }
}