using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace PacMan
{
    public static class BreadthFirstSearch
    {
        public static Position FindNextPosition(Map map, Position start, Position target)
        {
            var queue = new Queue<LinkedList<Position>>();
            queue.Enqueue(new LinkedList<Position>(new[] {start}));
            var visited = new HashSet<Position>();

            while (queue.Count != 0)
            {
                var singlyLinkedList = queue.Dequeue();
                var point = singlyLinkedList.Last.Value;
                if (point.x < 0 || point.x >= map.fields.GetLength(0) ||
                    point.y < 0 || point.y >= map.fields.GetLength(1) ||
                    map.fields[point.x, point.y].IsWall ||
                    visited.Contains(point)) continue;
                visited.Add(point);
                if (point.Equals(target)) return singlyLinkedList.First.Next?.Value ?? target;
                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
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