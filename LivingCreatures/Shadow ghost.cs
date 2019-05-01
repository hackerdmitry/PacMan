namespace PacMan
{
    public class Shadowghost : Ghost
    {
        public Shadowghost(int x, int y, Position initialPosition, Position exit, Position[,] pacmanCell) : base(
            x, y, initialPosition, exit, pacmanCell) 
        { }
    }
}