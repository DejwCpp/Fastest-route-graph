using Microsoft.Maui.Graphics;
using System.Drawing;
using Fastest_route_graph.Resources.Class;

namespace Fastest_route_graph
{
    /* To Do:
     * 
     * podswietlaj kolo na najechanie (probowalem, ale sie nie udalo)
     * 
     */

    public partial class MainPage : ContentPage
    {
        private List<System.Drawing.PointF> ClickedPoints = new List<System.Drawing.PointF>();
        private int CircleRadius = 30;

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

                // doing a set of condition depending when user clicked
                p = PointPlacementConditions(p);

                if (p.X == -1 && p.Y == -1) { return; }

                // updates ClickedPoints List in this class
                ClickedPoints.Add(p);

                // updates clickedPoints List in Drawing class
                var drawable = (Drawing)this.Resources["MyDrawable"];
                drawable.ClickedPoints = ClickedPoints;

                // the Draw method is called each time when mouse left is clicked thanks to this
                DrawSurface.Invalidate();
            }
        }

        // when clicked in existing circle connects to the middle of it
        private System.Drawing.PointF PointPlacementConditions(System.Drawing.PointF p)
        {
            // prevents nodes from being placed too close to the side of the window
            if (p.X < CircleRadius || p.Y < CircleRadius || p.X > DrawSurface.Width - CircleRadius || p.Y > DrawSurface.Height - CircleRadius)
            {
                return new System.Drawing.PointF(-1, -1);
            }
            // prevent creating new node when clicked in the area of last added node
            if (ClickedPoints.Count >= 1)
            {
                double lastNodeDistance = Math.Sqrt(Math.Pow(p.X - ClickedPoints[ClickedPoints.Count - 1].X, 2) + Math.Pow(p.Y - ClickedPoints[ClickedPoints.Count - 1].Y, 2));

                if (lastNodeDistance <= CircleRadius)
                {
                    return new System.Drawing.PointF(-1, -1);
                }
            }

            for (int i = 0; i < ClickedPoints.Count; i++)
            {
                double distance = Math.Sqrt(Math.Pow(p.X - ClickedPoints[i].X, 2) + Math.Pow(p.Y - ClickedPoints[i].Y, 2));

                // when clicked in existing circle connect to the middle of it
                if (distance <= CircleRadius)
                {
                    p.X = (int)ClickedPoints[i].X;
                    p.Y = (int)ClickedPoints[i].Y;

                    return p;
                }
                // prevents nodes from being placed too close from each other
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
