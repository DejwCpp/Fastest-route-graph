using Microsoft.Maui.Graphics;
using System.Drawing;
using Fastest_route_graph.Resources.Class;

namespace Fastest_route_graph
{
    /* To Do:
     * 
     * zapobiegaj stawianiu noda na skraju scianki
     * podswietlaj kolo na najechanie
     * nie dodawaj do clickedPoints kiedy polaczono z obecnym i przedostatnim nodem
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

                if (p.X == -1 && p.Y == -1) { return; }

                // updates clickedPoints List in this class
                clickedPoints.Add(p);

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

                if (distance <= 90)
                {
                    return new System.Drawing.PointF(-1, -1);
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
}
