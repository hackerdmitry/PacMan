namespace PacMan
{
    public class Footer
    {
        public int Height { get; }
        readonly GameController gameController;

        int countHealth;
        
        public Footer(GameController gameController)
        {
            this.gameController = gameController;
            Height = Map.LENGTH_CELL * 2;
        }

        public void LoseHealth() => countHealth--;
    }
}