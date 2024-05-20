using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastest_route_graph.Resources.Class
{
    internal class Drawing : IDrawable
    {
        public List<System.Drawing.PointF> ClickedPoints { get; set; } = new List<System.Drawing.PointF>();
        public List<System.Drawing.PointF> PathToDraw { get; set; } = new List<System.Drawing.PointF>();
        public System.Drawing.PointF TargetPoint = new System.Drawing.PointF();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.White;
            canvas.FillColor = Colors.White;
            canvas.StrokeSize = 6;

            // draw line
            for (int i = 0; i < ClickedPoints.Count - 1; i++)
            {
                canvas.DrawLine(ClickedPoints[i].X, ClickedPoints[i].Y, ClickedPoints[i + 1].X, ClickedPoints[i + 1].Y);
            }

            // draw circle
            for (int i = 0; i < ClickedPoints.Count; i++)
            {
                canvas.FillColor = Colors.White;

                // marks the first node with a different color
                if (ClickedPoints[i].X == ClickedPoints[0].X && ClickedPoints[i].Y == ClickedPoints[0].Y)
                {
                    canvas.FillColor = Colors.Khaki;
                }
                // marks the target node with a different color
                if (ClickedPoints[i].X == TargetPoint.X && ClickedPoints[i].Y == TargetPoint.Y)
                {
                    canvas.FillColor = Colors.Blue;
                }

                canvas.FillCircle(ClickedPoints[i].X, ClickedPoints[i].Y, 30);
            }

            // draw target path if exists
            if (PathToDraw.Count == 0)
                return;

            canvas.StrokeColor = Colors.Red;
            canvas.FillColor = Colors.Red;
            canvas.StrokeSize = 8;

            for (int i = 0; i < PathToDraw.Count - 1; i++)
            {
                canvas.DrawLine(PathToDraw[i].X, PathToDraw[i].Y, PathToDraw[i + 1].X, PathToDraw[i + 1].Y);
                canvas.FillCircle(PathToDraw[i].X, PathToDraw[i].Y, 35);
            }
            canvas.FillCircle(PathToDraw[PathToDraw.Count - 1].X, PathToDraw[PathToDraw.Count - 1].Y, 35);
        }
    }
}
