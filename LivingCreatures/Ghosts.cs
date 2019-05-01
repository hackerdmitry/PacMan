using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace PacMan
{
    public abstract class Ghost
    {
        int x { get; set; }
        int y { get; set; }

        Position target { get; set; }

        Random random = new Random();
        public readonly Position InitialPosition;
        public readonly Position Exit;
        public readonly Position[,] PacmanCell;

        public Ghost(int x, int y, Position initialPosition, Position exit, Position[,] pacmanCell)
        {
            this.x = x;
            this.y = y;
            InitialPosition = initialPosition;
            Exit = exit;
            PacmanCell = pacmanCell;
        }

        public Position FindPaths(Map map, Position start, Position pacmanCell)
        {
            LinkedList<Position> linkedList = new LinkedList<Position>();
            var queue = new Queue<LinkedList<Position>>();
            queue.Enqueue(new LinkedList<Position>(new[] {start}));
            var visited = new HashSet<Position>();

            while (queue.Count != 0)
            {
                var singlyLinkedList = queue.Dequeue();
                var point = singlyLinkedList.Last.Value;
                if (point.x < 0 || point.x >= map.fields.GetLength(0) ||
                    point.y < 0 || point.y >= map.fields.GetLength(1) ||
                    !(map.fields[point.x, point.y] is SimpleField) || visited.Contains(point)) continue;
                visited.Add(point);
                if (point == target) return singlyLinkedList.First.Value;
                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx == 0 || dy == 0)
                        {
                            LinkedList<Position> positions = new LinkedList<Position>(singlyLinkedList);
                            positions.AddLast(new Position(point.x + dx, point.y + dy));
                            queue.Enqueue(positions);
                        }               
            }
            return null;
        }
    }
}