using Microsoft.Maui.Graphics;
using System.Drawing;
using Fastest_route_graph.Resources.Class;

namespace Fastest_route_graph
{
    /* To Do:
     * 
     * zapisuj indexy node'ow na podstawie ClickedPoints zeby moc na tej podstawie zrobic matrixa
     * 
     */

    public partial class MainPage : ContentPage
    {
        private List<System.Drawing.PointF> ClickedPoints;
        private List<List<int>> Matrix;
        private int CircleRadius;
        private int NumOfNodes;
        private Button btnReset;
        private Button btnWeight;
        private Label btnWeightLabel;

        public MainPage()
        {
            InitializeComponent();
            RightSide.SizeChanged += BoxViewSizeChanged;

            ClickedPoints = new List<System.Drawing.PointF>();
            Matrix = new List<List<int>>();
            CircleRadius = 30;
            NumOfNodes = 0;
        }

        private void MouseLeftClick(Object sender, TappedEventArgs e)
        {
            NumOfNodes++;

            // add 'reset' button on first click
            if (NumOfNodes == 1)
            {
                btnReset = new Button
                {
                    Text = "Reset",
                    FontSize = 30,
                    WidthRequest = 200,
                    HeightRequest = 60,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 30, 0, 0)
                };
                btnReset.Clicked += ResetBtnClicked;

                mainGrid.SetColumn(btnReset, 0);
                mainGrid.Children.Add(btnReset);
            }

            // add 'set weight' button on second click
            if (NumOfNodes == 2)
            {
                btnWeight = new Button
                {
                    Text = "Set weight",
                    FontSize = 25,
                    WidthRequest = 200,
                    HeightRequest = 60,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 110, 0, 0)
                };
                btnWeight.Clicked += WeightBtnClicked;

                mainGrid.SetColumn(btnWeight, 0);
                mainGrid.Children.Add(btnWeight);
            }

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

                // updates clickedPoints List in Drawing.cs
                var drawable = (Drawing)this.Resources["MyDrawable"];
                drawable.ClickedPoints = ClickedPoints;

                // calls the Draw method in Drawing.cs
                DrawSurface.Invalidate();
            }

            // add indexes to the matrix
            ExpandMatrix();
        }

        // adds +1 row and +1 column every time a new node is created
        private void ExpandMatrix()
        {
            var graph = new Graph();

            // create [0][0] when matrix is empty
            if (Matrix.Count == 0)
            {
                Matrix.Add(new List<int>() { 0 });

                // updates matrix List in Graph.cs
                graph.matrix = Matrix;

                return;
            }

            // add +1 width
            foreach (List<int> row in Matrix)
            {
                row.Add(0);
            }

            // add +1 height
            int width = Matrix[0].Count;
            List<int> newRow = new List<int>(width);

            for (int i = 0; i < width; i++)
            {
                newRow.Add(0);
            }
            Matrix.Add(newRow);

            // updates matrix List in Graph.cs
            graph.matrix = Matrix;
        }

        private void ResetBtnClicked(Object sender, EventArgs e)
        {
            NumOfNodes = 0;

            ClickedPoints.Clear();

            // clears ClickedPoints in Drawing.cs
            var drawable = (Drawing)this.Resources["MyDrawable"];
            drawable.ClickedPoints.Clear();

            // calls the Draw method in Drawing.cs
            DrawSurface.Invalidate();

            // removes menu buttons
            mainGrid.Children.Remove(btnReset);

            if (btnWeight != null)
            {
                mainGrid.Children.Remove(btnWeight);
                btnWeight = null;
            }
        }

        private void WeightBtnClicked(Object sender, EventArgs e)
        {
            btnWeightLabel = new Label
            {
                Text = "Select two nodes",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(0, 0, 0, 30)
            };

            mainGrid.SetColumn(btnWeightLabel, 0);
            mainGrid.Children.Add(btnWeightLabel);
        }

        // handle clicking on a drawing field
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
