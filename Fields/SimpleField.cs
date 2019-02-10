using System.Drawing;

namespace PacMan
{
    public class SimpleField : IField
    {
        public SimpleField()
        {
            Bitmap = new Bitmap("../../Pictures/Fields/SimpleField.png");
        }
        
        public Bitmap Bitmap { get; }
    }
}