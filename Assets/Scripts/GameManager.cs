using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;
using GridGame;

namespace Working_Title.Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private Player currentPlayer = null;
        private GameObject gridManager;
        private Queue<Player> playerQueue = new Queue<Player>();

        void Awake()
        {
            // Enforce singleton
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);

            // Initialize the game with two human players
            InitGame(2, 0);
        }
        void InitGame(int numHumanPlayers, int numAIPlayers){
            
            // Should we also singleton the gridManager?  Guessing we only want one in a game right?
            Instantiate(gridManager);

            // Create human players
            for(int i=0; i < numHumanPlayers; i++){
                playerQueue.Enqueue(new Player(new HumanInterface()));
            }

            // Create AI players
            for(int i=0; i < numAIPlayers; i++){
                playerQueue.Enqueue(new Player(new AIInterface()));
            }

            // Trigger the first turn
            TriggerNextTurn();
        }

        bool CheckGameOver(){
            // Return true when winning criteria is met
            return false;
        }

        void TriggerNextTurn(){
            if(!CheckGameOver()){
                if (currentPlayer != null){
                    // Add current player to the end of the queue if this isn't the first turn
                    playerQueue.Enqueue(currentPlayer);
                }
                currentPlayer = playerQueue.Dequeue();
                currentPlayer.StartTurn();
            }
            else {
                // Handle game over, unless we want to just handle it in CheckGameOver?
                Debug.Log("Game over");
            }
        }

    }
}