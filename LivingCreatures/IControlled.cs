namespace PacMan
{
    public interface IControlled
    {
        Direction CurrentDirection { get; }
        
        Position AccuratePosition { get; }
//        Position PositionRegardingMapCells { get; }
    }
}