using System.Drawing;

namespace PacMan
{
    public class VoidField : IField
    {
        public VoidField()
        {
            Bitmap = new Bitmap("../../Pictures/Fields/VoidField.png");
        }
        
        public Bitmap Bitmap { get; }
    }
}