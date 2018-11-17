namespace PlayerClasses
{
    public class Player
    {
        private IPlayerInterface pInterface;
        public int gold;
        public Player(IPlayerInterface pInterface){
            this.pInterface = pInterface;
        }

        public void StartTurn(){
            pInterface.StartTurn();
        }

    }
}