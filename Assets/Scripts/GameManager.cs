using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;
using GridGame;

namespace Working_Title.Assets.Scripts
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Player currentPlayer {
            get;
            private set;
        }
        private Queue<Player> playerQueue = new Queue<Player>();

        //Event for when the turn changes.
        public delegate void TurnStartAction(Player currentPlayer);
        public event TurnStartAction onTurnStart;

        public int playerStartingGold;

        protected override void Awake()
        {
            base.Awake();
            // Initialize the game with two human players
            InitGame(2, 0);
        }
        void InitGame(int numHumanPlayers, int numAIPlayers){
            CreatePlayers(numHumanPlayers, numAIPlayers);
            // Trigger the first turn
            TriggerNextTurn();
        }

        bool CheckGameOver(){
            // Return true when winning criteria is met
            return false;
        }

        public void TriggerNextTurn(){
            if(!CheckGameOver()){
                if (currentPlayer != null){
                    // Add current player to the end of the queue if this isn't the first turn
                    playerQueue.Enqueue(currentPlayer);
                }
                currentPlayer = playerQueue.Dequeue();
                currentPlayer.StartTurn();

                //Fire an event when the turn has started.
                if(onTurnStart != null) {
                    onTurnStart(currentPlayer);
                }
            }
            else {
                // Handle game over, unless we want to just handle it in CheckGameOver?
                Debug.Log("Game over");
            }
        }

        void CreatePlayers(int numHumanPlayers, int numAIPlayers) {
            // Create human players
            for(int i=0; i < numHumanPlayers; i++){
                CreatePlayer(new HumanInterface());
            }
            // Create AI players
            for(int i=0; i < numAIPlayers; i++){
                CreatePlayer(new AIInterface());
            }
        }

        void CreatePlayer(PlayerInterface _interface) {
            Player playerToAdd = new Player(_interface);
            playerToAdd.gold = playerStartingGold;
            playerQueue.Enqueue(playerToAdd);
        }

    }
}