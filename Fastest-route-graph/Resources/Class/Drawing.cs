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

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.White;
            canvas.FillColor = Colors.White;
            canvas.StrokeSize = 6;

            for (int i = 0; i < ClickedPoints.Count; i++)
            {
                canvas.FillCircle(ClickedPoints[i].X, ClickedPoints[i].Y, 30);
            }

            for (int i = 0; i < ClickedPoints.Count - 1; i++)
            {
                canvas.DrawLine(ClickedPoints[i].X, ClickedPoints[i].Y, ClickedPoints[i + 1].X, ClickedPoints[i + 1].Y);
            }
        }
    }
}
