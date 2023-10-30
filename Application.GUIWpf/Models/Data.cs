using Window = Application.GUIWpf.Views.Pages.MainWindow;

namespace Application.GUIWpf.Models
{

    class Data
    {
        public double X;
        public double Y;

        public Data(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public System.Windows.Point ToPoint(double rightBoundary, double topBoundary)
        {
            var y = Window.yAxisStart + Window.DeltaY(Y) * (topBoundary - Window.yAxisStart);
            var x = Window.xAxisStart + Window.DeltaX(X) * (rightBoundary - Window.xAxisStart);
            return new System.Windows.Point(x, y);
        }
    }

}