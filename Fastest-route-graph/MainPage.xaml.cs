using Microsoft.Maui.Graphics;
using System.Drawing;

namespace Fastest_route_graph
{
    public partial class MainPage : ContentPage
    {
        private List<System.Drawing.PointF> clickedPoints = new List<System.Drawing.PointF>();

        public MainPage()
        {
            InitializeComponent();
            RightSide.SizeChanged += BoxViewSizeChanged;
        }

        private void MouseLeftClick(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
        {
            // Get the position relative to the tapped UI element.
            Microsoft.Maui.Graphics.Point? point = e.GetPosition((View)sender);

            if (point.HasValue)
            {
                int x = (int)point.Value.X;
                int y = (int)point.Value.Y;

                // updates clickedPoints List in this class
                clickedPoints.Add(new System.Drawing.PointF(x, y));

                // updates clickedPoints List in Drawing class
                var drawable = (Drawing)this.Resources["MyDrawable"];
                drawable.clickedPoints = clickedPoints;

                // the Draw method is called each time when mouse left is clicked thanks to this
                DrawSurface.Invalidate();
            }
        }

        // sets drawable field to the area of BoxView
        private void BoxViewSizeChanged(object sender, EventArgs e)
        {
            double boxViewWidth = RightSide.Width;
            double boxViewHeight = RightSide.Height;

            DrawSurface.WidthRequest = boxViewWidth;
            DrawSurface.HeightRequest = boxViewHeight;
        }
    }
    public class Drawing : IDrawable
    {
        public List<System.Drawing.PointF> clickedPoints { get; set; } = new List<System.Drawing.PointF>();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 6;

            for (int i = 0; i < clickedPoints.Count - 1; i++)
            {
                canvas.DrawLine(clickedPoints[i].X, clickedPoints[i].Y, clickedPoints[i + 1].X, clickedPoints[i + 1].Y);
            }
        }
    }
}
