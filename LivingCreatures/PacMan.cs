namespace PacMan
{
    public class PacMan : IControlled
    {
        public Direction CurrentDirection { get; private set; }
        public Position AccuratePosition { get; private set; }
        public Position PositionRegardingMapCells => 
            new Position((AccuratePosition.x + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL,
                         (AccuratePosition.y + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL);

        public PacMan(Position accuratePostion)
        {
            CurrentDirection = Direction.Right;
            AccuratePosition = accuratePostion;
        }
    }
}