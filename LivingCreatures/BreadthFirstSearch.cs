using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace PacMan
{
    public static class BreadthFirstSearch
    {
        public static Position FindNextPosition(Map map, Position start, Position target, Creature creature)
        {
            Queue<LinkedList<Position>> queue = new Queue<LinkedList<Position>>();
            queue.Enqueue(new LinkedList<Position>(new[] {start}));
            HashSet<Position> visited = new HashSet<Position>();

            while (queue.Count != 0)
            {
                LinkedList<Position> singlyLinkedList = queue.Dequeue();
                Position point = map.GetPositionInMap(singlyLinkedList.Last.Value);
                if (map.MapFields[point.x, point.y].IsWall(creature) ||
                    visited.Contains(point)) continue;
                visited.Add(point);
                if (point.Equals(target)) return singlyLinkedList.First.Next?.Value ?? target;
                for (int dy = -1; dy <= 1; dy++)
                    for (int dx = -1; dx <= 1; dx++)
                        if ((dx == 0 || dy == 0) && !(dx == 0 && dy == 0))
                        {
                            LinkedList<Position> positions = new LinkedList<Position>(singlyLinkedList);
                            positions.AddLast(new Position(point.x + dx,
                                                           point.y + dy));
                            queue.Enqueue(positions);
                        }
            }
            return target;
        }
    }
}