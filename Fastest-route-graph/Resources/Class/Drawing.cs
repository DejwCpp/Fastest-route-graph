using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastest_route_graph.Resources.Class
{
    internal class Drawing : IDrawable
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
