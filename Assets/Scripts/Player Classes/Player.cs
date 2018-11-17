namespace PlayerClasses
{
    public class Player
    {
        private PlayerInterface pInterface;
        public int gold;
        public Player(PlayerInterface pInterface){
            this.pInterface = pInterface;
        }

        public void StartTurn(){
            pInterface.StartTurn();
        }
    }
}