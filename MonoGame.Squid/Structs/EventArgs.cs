using MonoGame.Squid.Controls;

namespace MonoGame.Squid.Structs
{
    /// <summary>
    /// Class SquidEventArgs
    /// </summary>
    public class SquidEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SquidEventArgs"/> is cancel.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        public bool Cancel ;
    }

    /// <summary>
    /// Class MouseEventArgs
    /// </summary>
    public class MouseEventArgs : SquidEventArgs
    {
        /// <summary>
        /// Gets or sets the button.
        /// </summary>
        /// <value>The button.</value>
        public int Button ;
    }

    /// <summary>
    /// Class KeyEventArgs
    /// </summary>
    public class KeyEventArgs : SquidEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        public KeyEventArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        internal KeyEventArgs(KeyData key)
        {
            Key = key.Key;
            //Scancode = key.Scancode;
            Char = key.Char;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Keys Key ;

        /// <summary>
        /// Gets or sets the scancode.
        /// </summary>
        /// <value>The scancode.</value>
        //public int Scancode ;

        /// <summary>
        /// Gets or sets the char.
        /// </summary>
        public char? Char ;

    }

    /// <summary>
    /// Class DragDropEventArgs
    /// </summary>
    public sealed class DragDropEventArgs : SquidEventArgs
    {
        /// <summary>
        /// The source
        /// </summary>
        public Control Source;
        /// <summary>
        /// The dragged control
        /// </summary>
        public Control DraggedControl;
    }
}
