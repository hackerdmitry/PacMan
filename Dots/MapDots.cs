using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PacMan
{
    public class MapDots : IEnumerable<KeyValuePair<Position, IDots>>
    {
        readonly Dictionary<Position, IDots> dots;
        public readonly char[,] CharDots;
        readonly Map map;

        public MapDots(char[,] charDots, Map map)
        {
            CharDots = charDots;
            this.map = map;
            dots = new Dictionary<Position, IDots>();
            MakeDots();
        }

        public IDots this[Position position] => dots[position];

        void MakeDots()
        {
            string[] notParsedEnergizers = new StreamReader($"{map.PacManWindow.folderLevelPath}/energizers.txt")
                .ReadToEnd()
                .Split('\n', '\r').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            foreach (string energizerPos in notParsedEnergizers)
            {
                string[] pos = energizerPos.Split();
                dots.Add(new Position((int) (float.Parse(pos[0]) * Map.LENGTH_CELL),
                                      (int) (float.Parse(pos[1]) * Map.LENGTH_CELL)), new Energizer());
            }
            List<Position> directions = Enum.GetValues(typeof(Direction)).Cast<Direction>()
                .Select(GameController.GetPosition).ToList();

            for (int i = 0; i < map.HeightCountCell; i++)
                for (int j = 0; j < map.WidthCountCell; j++)
                    if (CharDots[i, j] == 'd')
                        directions.ForEach(x => MakeDotRay(new Position(i, j), x));
//            dots[i, j] = dictionaryFields[CharFields[j, i]](j, i);
        }

        void MakeDotRay(Position start, Position direction)
        {
            Position startDir = start + direction;
            if (CharDots[startDir.x, startDir.y] == ' ') return;
            int countCells = 1;
            for (int x = startDir.x, y = startDir.y; CharDots[x, y] != 'd'; x += direction.x, y += direction.y)
                countCells++;
            Position curPos = start * Map.LENGTH_CELL;
            direction *= Map.LENGTH_CELL;
            for (int i = 0; i < countCells + 1; i++, curPos += direction * ((float) countCells / (countCells + 1)))
                if (!dots.ContainsKey(curPos.Flip()))
                    dots.Add(curPos.Flip(), new Dot());
        }

        public IEnumerator<KeyValuePair<Position, IDots>> GetEnumerator() => dots.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}