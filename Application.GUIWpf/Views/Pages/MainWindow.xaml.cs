using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Application.GUIWpf.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (UploadSavePanel.Visibility == Visibility.Collapsed)
            {
                UploadSavePanel.Visibility = Visibility.Visible;
                RotateTransform rotateTransform = new RotateTransform(180,7,3);
                Triangle.RenderTransform = rotateTransform;
            }
            else
            {
                UploadSavePanel.Visibility = Visibility.Collapsed;
                RotateTransform rotateTransform = new RotateTransform(0);
                Triangle.RenderTransform = rotateTransform;
            }
        }
    }
}