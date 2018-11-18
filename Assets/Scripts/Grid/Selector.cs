using UnityEngine;

namespace GridGame
{
    public class Selector
    {
        public static Tile ReturnSelectedTile() {
            // Layermast ignores all but the Tile layer
            int layerMask = 1 << 8;
            if(Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask)){
                    // I could use the gridmanager helper function here to retrieve coords
                }
            }
        }
    }
}