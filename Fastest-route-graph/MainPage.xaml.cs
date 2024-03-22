using Microsoft.Maui.Graphics;
using System.Drawing;

namespace Fastest_route_graph
{
    /* To Do:
     * 
     * TranslatePointToTheMiddleOfaCircle doesnt work. It worked when it wasnt seperated method tho
     * 
     * jak kliknieto w kolo to polacz ze srodkiem
     * podswietlaj kolo na najechanie
     * nie lacz z obecnym i przedostatnim nodem
     * nie podswietlaj obecnego i przedostatniego noda
     * 
     */

    public partial class MainPage : ContentPage
    {
        private List<System.Drawing.PointF> clickedPoints = new List<System.Drawing.PointF>();

        public MainPage()
        {
            InitializeComponent();
            RightSide.SizeChanged += BoxViewSizeChanged;
        }

        private void MouseLeftClick(Object sender, TappedEventArgs e)
        {
            // Get the position relative to the tapped UI element.
            Microsoft.Maui.Graphics.Point? point = e.GetPosition((View)sender);

            if (point.HasValue)
            {
                int x = (int)point.Value.X;
                int y = (int)point.Value.Y;

                System.Drawing.PointF p = new System.Drawing.PointF(x, y);

                // when clicked in existing circle connect to the middle of it
                p = TranslatePointToTheMiddleOfaCircle(p);

                // updates clickedPoints List in this class
                clickedPoints.Add(new System.Drawing.PointF(x, y));

                // updates clickedPoints List in Drawing class
                var drawable = (Drawing)this.Resources["MyDrawable"];
                drawable.clickedPoints = clickedPoints;

                // the Draw method is called each time when mouse left is clicked thanks to this
                DrawSurface.Invalidate();
            }
        }

        // when clicked in existing circle connects to the middle of it
        private System.Drawing.PointF TranslatePointToTheMiddleOfaCircle(System.Drawing.PointF p)
        {
            for (int i = 0; i < clickedPoints.Count; i++)
            {
                double distance = Math.Sqrt(Math.Pow(p.X - clickedPoints[i].X, 2) + Math.Pow(p.Y - clickedPoints[i].Y, 2));

                if (distance <= 30)
                {
                    p.X = (int)clickedPoints[i].X;
                    p.Y = (int)clickedPoints[i].Y;

                    return p;
                }
            }
            return p;
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
            canvas.FillColor = Colors.White;
            canvas.StrokeSize = 6;

            for (int i = 0; i < clickedPoints.Count; i++)
            {
                canvas.FillCircle(clickedPoints[i].X, clickedPoints[i].Y, 30);
            }

            for (int i = 0; i < clickedPoints.Count - 1; i++)
            {
                canvas.DrawLine(clickedPoints[i].X, clickedPoints[i].Y, clickedPoints[i + 1].X, clickedPoints[i + 1].Y);
            }
        }
    }
}
