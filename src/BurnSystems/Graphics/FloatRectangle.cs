namespace BurnSystems.Graphics
{
    /// <summary>
    /// Eine Rechteckstruktur, bei dem Breite und Höhe in einer Fließkommazahl
    /// mit doppelter Genauigkeit gespeichert wird. 
    /// </summary>
    public class FloatRectangle
    {
        /// <summary>
        /// Initializes a new instance of the FloatRectangle class.
        /// </summary>
        public FloatRectangle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the FloatRectangle class.
        /// </summary>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        public FloatRectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the FloatRectangle class.
        /// </summary>
        /// <param name="left">Left border</param>
        /// <param name="top">Top border</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        public FloatRectangle(double left, double top, double width, double height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the x-value of the left border
        /// </summary>
        public double Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y-value of the top border
        /// </summary>
        public double Top
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the x-value of the right border
        /// </summary>
        public double Right
        {
            get { return Left + Width; }
            set { Width = value - Left; }
        }

        /// <summary>
        /// Gets or sets the y-value of the bottom border
        /// </summary>
        public double Bottom
        {
            get { return Top + Height; }
            set { Height = value - Top; }
        }

        /// <summary>
        /// Gets or sets the width of rectangle
        /// </summary>
        public double Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of rectangle
        /// </summary>
        public double Height
        {
            get;
            set;
        }
    }
}
