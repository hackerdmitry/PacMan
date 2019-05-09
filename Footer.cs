using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class Footer
    {
        public int Height { get; }
        readonly Bitmap lifePlayer = new Bitmap("../../Pictures/Player/Life/Life.png");
        readonly Queue<FruitField.Fruit> fruits = new Queue<FruitField.Fruit>();

        int countHealth = 3;

        public Footer(GameController gameController) { Height = Map.LENGTH_CELL * 2; }

        public int GetHealth() => countHealth;

        public void LoseHealth() => countHealth--;

        public void AddHealth() => countHealth++;

        public void AddFruit(FruitField.Fruit fruit)
        {
            fruits.Enqueue(fruit);
            if (fruits.Count > 6)
                fruits.Dequeue();
        }

        public void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < countHealth; i++)
                e.Graphics.DrawImage(lifePlayer,
                                     new Rectangle((int) ((2.375f + i * 1.5f) * Map.LENGTH_CELL),
                                                   (int) (25.6f * Map.LENGTH_CELL),
                                                   Map.LENGTH_CELL, Map.LENGTH_CELL));
            int iFruit = -1;
            foreach (FruitField.Fruit fruit in fruits)
                e.Graphics.DrawImage(fruit.fruit,
                                     new Rectangle(
                                         (int) ((24.25f + (iFruit++ - fruits.Count) * 1.5f) * Map.LENGTH_CELL),
                                         (int) (25.6f * Map.LENGTH_CELL),
                                         Map.LENGTH_CELL, Map.LENGTH_CELL));
        }
    }
}