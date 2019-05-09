using System;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class FruitField : SimpleField
    {
        readonly int[] fruitSpawnScores = {1, 170};
        int iFruitSpawnScores;
        bool pickedUp = true;
        long startFruitSpawn;
        long durationFruit;
        Fruit fruit;
        readonly Random rand = new Random();

        readonly Brush brush = new SolidBrush(Color.FromArgb(0xFF, 0xBD, 0xFF));
        readonly Font font = new Font("Joystix", Map.LENGTH_CELL * 0.333f);

        public FruitField(int x, int y) : base(x, y) { }

        static readonly Bitmap[] fruits =
        {
            new Bitmap("../../Pictures/Fruits/Cherry.png"),
            new Bitmap("../../Pictures/Fruits/Strawberry.png"),
            new Bitmap("../../Pictures/Fruits/Orange.png"),
            new Bitmap("../../Pictures/Fruits/Cherry.png"),
            new Bitmap("../../Pictures/Fruits/Cherry.png"),
            new Bitmap("../../Pictures/Fruits/Cherry.png"),
            new Bitmap("../../Pictures/Fruits/Cherry.png"),
        };

        public Fruit GetFruit(GameController gameController)
        {
            if (gameController.Level == 1) return new Fruit(fruits[0], 100);
            return new Fruit(fruits[1], 300);
        }

        public void Inform(int informedScore, GameController gameController)
        {
            if (iFruitSpawnScores < fruitSpawnScores.Length && fruitSpawnScores[iFruitSpawnScores] <= informedScore)
            {
                iFruitSpawnScores++;
                pickedUp = false;
                startFruitSpawn = DateTime.Now.Ticks;
                durationFruit = (long) ((9 + rand.NextDouble()) * 1000 * 10000);
                fruit = GetFruit(gameController);
            }
        }

        public void OnPaint(PaintEventArgs e, GameController gameController)
        {
            if (DateTime.Now.Ticks < startFruitSpawn + durationFruit)
                if (!pickedUp)
                    if ((gameController.Player.AccuratePosition - new Position(Y, X) * Map.LENGTH_CELL).Length() <
                        gameController.Player.Speed)
                    {
                        pickedUp = true;
                        startFruitSpawn = DateTime.Now.Ticks;
                        durationFruit = 3 * 1000 * 10000;
                        gameController.Score.AddScore(fruit.score);
                    }
                    else
                        gameController.DrawImageRegardingMapCells(e, fruit.fruit, new Position(Y, X));
                else
                    e.Graphics.DrawString(fruit.score.ToString(), font, brush,
                                          Y * Map.LENGTH_CELL, (int)((X + 3.333) * Map.LENGTH_CELL));
        }

        public class Fruit
        {
            public readonly Bitmap fruit;
            public readonly int score;

            public Fruit(Bitmap fruit, int score)
            {
                this.fruit = fruit;
                this.score = score;
            }
        }
    }
}