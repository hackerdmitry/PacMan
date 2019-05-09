using System;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class FruitField : SimpleField
    {
        readonly int[] fruitSpawnScores = {70, 170};
        int iFruitSpawnScores;
        bool pickedUp = true;
        long startFruitSpawn;
        long durationFruit;
        Fruit fruit;
        readonly Random rand = new Random();

        readonly Brush brush = new SolidBrush(Color.FromArgb(0xFF, 0xBD, 0xFF));
        readonly Font font = new Font("Joystix", Map.LENGTH_CELL * 0.333f);

        public FruitField(int x, int y) : base(x, y) { }

        static readonly Fruit[] fruits =
        {
            new Fruit(new Bitmap("../../Pictures/Fruits/Cherry.png"), 100), 
            new Fruit(new Bitmap("../../Pictures/Fruits/Strawberry.png"), 300), 
            new Fruit(new Bitmap("../../Pictures/Fruits/Orange.png"), 500), 
            new Fruit(new Bitmap("../../Pictures/Fruits/Apple.png"), 700), 
            new Fruit(new Bitmap("../../Pictures/Fruits/Melon.png"), 1000), 
            new Fruit(new Bitmap("../../Pictures/Fruits/Galaxian.png"), 2000), 
            new Fruit(new Bitmap("../../Pictures/Fruits/Bell.png"), 3000),
            new Fruit(new Bitmap("../../Pictures/Fruits/Key.png"), 5000)
        };

        static Fruit GetFruit(GameController gameController)
        {
            if (gameController.Level == 1) return fruits[0];
            if (gameController.Level == 2) return fruits[1];
            if (gameController.Level <= 4) return fruits[2];
            if (gameController.Level <= 6) return fruits[3];
            if (gameController.Level <= 8) return fruits[4];
            if (gameController.Level <= 10) return fruits[5];
            return gameController.Level <= 12 ? fruits[6] : fruits[7];
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