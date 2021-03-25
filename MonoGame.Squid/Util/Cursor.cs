using System.Collections.Generic;
using MonoGame.Squid.Structs;

namespace MonoGame.Squid.Util
{
    /// <summary>
    /// A collection of Cursors
    /// </summary>
    public class CursorCollection : Dictionary<string, Cursor> { }

    /// <summary>
    /// Represents the image that is displayed at the mouse position.
    /// </summary>
    public class Cursor
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        [IntColor]
        public int Color ;

        string _texture;
        
        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        public string Texture
        {
            get => _texture; set
            {
                _texture = value;
                Size = Gui.Renderer.GetTextureSize(value);
            }
        }
        /// <summary>
        /// Gets or sets the hot spot.
        /// </summary>
        public Point HotSpot ;

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Point Size ;
        
        /// <summary>
        /// Gets or sets the texture rect.
        /// </summary>
        public Rectangle TextureRect ;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        public Cursor()
        {
            Color = -1;
        }

        /// <summary>
        /// Draws the cursor at the specified position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public virtual void Draw(int x, int y)
        {
            var p = new Point(x, y) - HotSpot;
            Gui.Renderer.DrawTexture(Texture, p.X, p.Y, Size.X, Size.Y, TextureRect, Color);
        }
    }

    /// <summary>
    /// A flibook based implementation of an animated Cursor.
    /// </summary>
    public class FlipbookCursor : Cursor
    {
        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public int Rows ;
        
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public int Columns ;
        
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed ;

        /// <summary>
        /// The flip
        /// </summary>
        private readonly Flipbook _flip = new Flipbook();

        /// <summary>
        /// Initializes a new instance of the <see cref="FlipbookCursor"/> class.
        /// </summary>
        public FlipbookCursor()
        {
            Color = -1;
            Rows = 1;
            Columns = 1;
            Speed = 60;
        }

        /// <summary>
        /// Draws the cursor at the specified position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public override void Draw(int x, int y)
        {
            var p = new Point(x, y) - HotSpot;

            _flip.Speed = Speed;
            _flip.Rows = Rows;
            _flip.Columns = Columns;
            _flip.Draw(Texture, p.X, p.Y, Size.X, Size.Y, Color);
        }
    }
}
