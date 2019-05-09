using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PacMan
{
    public class MapFields : IEnumerable<IField>
    {
        readonly IField[,] fields;
        public readonly char[,] CharFields;
        readonly Dictionary<char, Func<int, int, IField>> dictionaryFields;
        readonly Map map;

        public MapFields(char[,] charFields, Map map)
        {
            CharFields = charFields;
            this.map = map;
            fields = new IField[map.WidthCountCell, map.HeightCountCell];
            dictionaryFields = new Dictionary<char, Func<int, int, IField>>
            {
                {'w', (x, y) => new Wall(map, x, y)},
                {'s', (x, y) => new SpawnWall(map, x, y)},
                {'l', (x, y) => new SpawnLine(x, y)},
                {' ', (x, y) => new SimpleField(x, y)},
                {'0', (x, y) => new VoidField(x, y)},
                {'r', (x, y) => new SpawnField(x, y)}
            };
        }

        public IField this[int i, int j] => fields[i, j];

        public void MakeFields()
        {
            for (int i = 0; i < map.WidthCountCell; i++)
                for (int j = 0; j < map.HeightCountCell; j++)
                    fields[i, j] = dictionaryFields[CharFields[j, i]](j, i);
        }

        public IEnumerator<IField> GetEnumerator() => fields.Cast<IField>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}