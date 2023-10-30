using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Application.GUIWpf.Models;
using Common.Base.Interfaces;

namespace Application.GUIWpf.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double xAxisStart = 100, yAxisStart = 100;
        static double rightBoudary, topBoudary;
        static double minX, maxX, minY, maxY;
        static List<SolidColorBrush> solidColors = new List<SolidColorBrush>() {  Brushes.Red, Brushes.Yellow, Brushes.Red, Brushes.Brown, Brushes.Blue, Brushes.Yellow, Brushes.Green, Brushes.Orange, Brushes.Azure, Brushes.Cyan };
      


        public MainWindow()
        {
            InitializeComponent();
            this.StateChanged += (sender, e) => DrawGraphics();
            this.SizeChanged += (sender, e) => DrawGraphics();
        }

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TODO: переделать на движение только через верхнюю панель
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DrawGraphics();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (UploadSavePanel.Visibility == Visibility.Collapsed)
            {
                UploadSavePanel.Visibility = Visibility.Visible;
                var rotateTransform = new RotateTransform(180, 7, 3);
                Triangle.RenderTransform = rotateTransform;
            }
            else
            {
                UploadSavePanel.Visibility = Visibility.Collapsed;
                var rotateTransform = new RotateTransform(0);
                Triangle.RenderTransform = rotateTransform;
            }
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is Location p)
            {
                for (int i = 0; i < 2; i++)
                {
                    typeof(System.Windows.Controls.Primitives.ButtonBase)
 .GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)
 .Invoke(Button, new object[0]);
                }
                // TODO: сюда вставить реализизацию автоматического изменения отрисованного графика при изменении координат
                // MessageBox.Show($"{p.PointX} {p.PointY}");
            }
        }
        void DrawGraphics()
        {
            rightBoudary = chart.ActualWidth - xAxisStart;
            topBoudary = chart.ActualHeight - yAxisStart;
            if (TempData.datas.Count == 0)
                return;
            maxY = TempData.datas.Max(maxYInSubsequece => maxYInSubsequece.Max(data => data.Y));
            maxX = TempData.datas.Max(maxXInSubsequece => maxXInSubsequece.Max(data => data.X));
            minY = TempData.datas.Min(minYInSubsequece => minYInSubsequece.Min(data => data.Y));
            minX = TempData.datas.Min(minXInSubsequece => minXInSubsequece.Min(data => data.X));

            chart.Children.Clear();

            Line xAxisLine = new Line()
            {
                X1 = xAxisStart,
                Y1 = topBoudary,
                X2 = rightBoudary,
                Y2 = topBoudary,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
            };
            Line yAxisLine = new Line()
            {
                X1 = xAxisStart,
                Y1 = yAxisStart - 50,
                X2 = xAxisStart,
                Y2 = topBoudary,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
            };
            chart.Children.Add(xAxisLine);
            chart.Children.Add(yAxisLine);

                for (int i = 0; i < TempData.datas.Count; i++)
                {
                    PathFigure myPathFigure = new PathFigure();
                    PathGeometry myPathGeometry = new PathGeometry();
                    Path myPath = new Path();

                    myPathFigure.StartPoint = TempData.datas[i][0].ToPoint(rightBoudary, topBoudary);
                    var startPoint = TempData.datas[i][0].ToPoint(rightBoudary, topBoudary);

                    var textBlock = new TextBlock();
                    textBlock.Text = TempData.datas[i][0].X.ToString();
                    chart.Children.Add(textBlock);
                    Canvas.SetLeft(textBlock, startPoint.X);
                    Canvas.SetTop(textBlock, yAxisLine.Y2);


                    for (int j = 1; j < TempData.datas[i].Count; j++)
                    {
                        var point = TempData.datas[i][j].ToPoint(rightBoudary, topBoudary);
                        myPathFigure.Segments.Add(
                            new LineSegment(point,
                            true /* IsStroked */ ));


                        textBlock = new TextBlock();
                        textBlock.Text = TempData.datas[i][j].X.ToString();
                        chart.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, point.X);
                        Canvas.SetTop(textBlock, yAxisLine.Y2);

                        textBlock = new TextBlock();
                        textBlock.Text = TempData.datas[i][j].Y.ToString();
                        chart.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, xAxisLine.X1 - 10);
                        Canvas.SetTop(textBlock, point.Y - 10);
                    }
                    myPathGeometry.Figures.Add(myPathFigure);


                    myPath.Stroke = solidColors[i];
                    myPath.StrokeThickness = 2;
                    myPath.Data = myPathGeometry;
                    chart.Children.Add(myPath);
                }           
           
        }
        public static double DeltaX(double x)
        {
            return (x - minX) / (maxX - minX);
        }
        public static double DeltaY(double y)
        {
            return (maxY - y) / (maxY - minY);
        }
    }
}