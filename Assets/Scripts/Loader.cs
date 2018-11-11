using UnityEngine;

namespace Working_Title.Assets.Scripts
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameManager;
        
        void Awake(){
            if (GameManager.instance == null) Instantiate(gameManager);
        }
    }
}