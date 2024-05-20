using Microsoft.Maui.Graphics;
using System.Drawing;
using Fastest_route_graph.Resources.Class;

namespace Fastest_route_graph
{
    /* To Do:
     * 
     * handle clicking on SetWeight button
     * make Label disapear in the right moments
     * 
     */

    public partial class MainPage : ContentPage
    {
        public int TargetPointId;
        public List<System.Drawing.PointF> PathToDraw;
        private List<System.Drawing.PointF> ClickedPoints;
        private List<System.Drawing.PointF> NodesPlacement;
        private List<List<int>> Matrix;
        private List<int> NodesIdx;
        private int CircleRadius;
        private int NumOfLeftClick;
        private int NumOfNodes;
        private int Weight;
        private bool SelectMode;
        private Button btnReset;
        private Button btnWeight;
        private Button btnFastest;
        private Label btnBottomLeftLabel;

        public MainPage()
        {
            InitializeComponent();
            RightSide.SizeChanged += BoxViewSizeChanged;

            PathToDraw = new List<System.Drawing.PointF>();
            ClickedPoints = new List<System.Drawing.PointF>();
            NodesPlacement = new List<System.Drawing.PointF>();
            Matrix = new List<List<int>>();
            NodesIdx = new List<int>();
            CircleRadius = 30;
            NumOfLeftClick = 0;
            NumOfNodes = 0;
            Weight = 1;
            SelectMode = false;
        }

        private void MouseLeftClick(Object sender, TappedEventArgs e)
        {
            NumOfLeftClick++;

            // add 'reset' button on first click
            if (NumOfLeftClick == 1)
            {
                CreateResetBtn();
            }

            // add 'Set weight' and 'Fastest route' button on second click
            if (NumOfLeftClick == 2)
            {
                CreateSetWeightBtn();

                CreateFastestRouteBtn();
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

                // anables connection to Drawing.cs
                var drawable = (Drawing)this.Resources["MyDrawable"];

                if (SelectMode == true)
                {
                    // updates matrix List in Graph.cs
                    var graph = new Graph();
                    graph.matrix = Matrix;

                    // sets nodes id | e.g. for NumOfNodes = 4 -> node.id = {0, 1, 2, 3}
                    graph.Q = GetNodesInfo(NumOfNodes);

                    Node TargetPoint = new Node();

                    TargetPoint.Position = p;

                    // updates TargetPoint Point in Drawing.cs
                    drawable.TargetPoint = TargetPoint.Position;

                    TargetPoint = TargetPoint.GetNodeFromThisLocation(TargetPoint.Position, graph.Q);

                    // the hardest part in my opinion
                    graph.FindFastestRoute(TargetPoint.Id);

                    PathToDraw = graph.PathToDraw;
                    drawable.PathToDraw = PathToDraw;
                }
                if (SelectMode == false)
                {
                    ClickedPoints.Add(p);

                    // updates clickedPoints List in Drawing.cs
                    drawable.ClickedPoints = ClickedPoints;
                }

                // calls the Draw method in Drawing.cs
                DrawSurface.Invalidate();
            }
        }

        private void CreateResetBtn()
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

        private void CreateSetWeightBtn()
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

        private void CreateFastestRouteBtn()
        {
            btnFastest = new Button
            {
                Text = "Fastest route",
                FontSize = 25,
                WidthRequest = 200,
                HeightRequest = 60,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 200, 0, 0)
            };
            btnFastest.Clicked += FastestRouteBtnClicked;

            mainGrid.SetColumn(btnFastest, 0);
            mainGrid.Children.Add(btnFastest);
        }

        private void RemoveButton(Button btn)
        {
            if (btn != null)
            {
                mainGrid.Children.Remove(btn);
                btn = null;
            }
        }

        private void CreateBottomLeftLabel(string text)
        {
            btnBottomLeftLabel = new Label
            {
                Text = text,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(0, 0, 0, 30)
            };

            mainGrid.SetColumn(btnBottomLeftLabel, 0);
            mainGrid.Children.Add(btnBottomLeftLabel);
        }

        // adds +1 row and +1 column every time a new node is created
        private void ExpandMatrix()
        {
            // create [0][0] when matrix is empty
            if (Matrix.Count == 0)
            {
                Matrix.Add(new List<int>() { 0 });

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
        }

        private void ResetBtnClicked(Object sender, EventArgs e)
        {
            NumOfNodes = 0;
            NumOfLeftClick = 0;

            ClickedPoints.Clear();

            // clears ClickedPoints in Drawing.cs
            var drawable = (Drawing)this.Resources["MyDrawable"];
            drawable.ClickedPoints.Clear();
            drawable.PathToDraw.Clear();

            // calls the Draw method in Drawing.cs
            DrawSurface.Invalidate();

            // removes menu buttons
            RemoveButton(btnReset);
            RemoveButton(btnWeight);
            RemoveButton(btnFastest);
        }

        private void WeightBtnClicked(Object sender, EventArgs e)
        {
            CreateBottomLeftLabel("Select two nodes");
        }

        private void FastestRouteBtnClicked(Object sender, EventArgs e)
        {
            // fill matrix
            for (int i = 1; i < NodesIdx.Count; i++)
            {
                Matrix[NodesIdx[i - 1]][NodesIdx[i    ]] = 1;
                Matrix[NodesIdx[i    ]][NodesIdx[i - 1]] = 1;
            }

            // here ask user for an end node and store it's id into varriable
            CreateBottomLeftLabel("Select target node");
            SelectMode = true;

            // when SelectMode = true it enables condition in leftClick method
        }
        
        // sets id and position to all the nodes
        private List<Node> GetNodesInfo(int size)
        {
            List<Node> arr = new List<Node>();

            for (int i = 0; i < size; i++)
            {
                Node node = new Node();

                node.Id = i;
                node.Position = NodesPlacement[i];

                arr.Add(node);
            }
            return arr;
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
            if (SelectMode == false)
            {
                if (ClickedPoints.Count >= 1)
                {
                    double lastNodeDistance = Math.Sqrt(Math.Pow(p.X - ClickedPoints[ClickedPoints.Count - 1].X, 2) + Math.Pow(p.Y - ClickedPoints[ClickedPoints.Count - 1].Y, 2));

                    if (lastNodeDistance <= CircleRadius)
                    {
                        return new System.Drawing.PointF(-1, -1);
                    }
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

                    NodesIdx.Add(i);

                    return p;
                }
                // prevents nodes from being placed too close from each other
                if (distance <= 90)
                {
                    return new System.Drawing.PointF(-1, -1);
                }
            }
            if (SelectMode == false)
            {
                // add indexes to the matrix
                ExpandMatrix();

                NodesPlacement.Add(p);
                NodesIdx.Add(NumOfNodes);

                NumOfNodes++;
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
