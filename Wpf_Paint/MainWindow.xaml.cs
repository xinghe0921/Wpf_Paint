using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wpf_Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color currentStrokeColor = Colors.Red;
        Color currentFillColor = Colors.Yellow;
        Brush currentStrokeBrush = new SolidColorBrush(Colors.Red);
        Brush currentFillBrush = new SolidColorBrush(Colors.Yellow);
        int currentStrokeThickness = 1;
        string currentShape = "Line";
        string currentAction = "Draw";
        Point start, dest;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = System.Windows.Input.Cursors.Cross;
            start = e.GetPosition(myCanvas);
            myLabel.Content = $"座標點:({start})-({dest})";

        }

        private void myCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            switch (currentAction)
            {
                case "Draw":
                    //顯示目前滑鼠游標，如果滑鼠左鍵一直按著就取終點座標
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        myCanvas.Cursor = Cursors.Pen;
                        dest = e.GetPosition(myCanvas);
                        myLabel.Content = $"座標點:({start})-({dest})";
                    }
                    break;
                case "Erase":
                    //將滑鼠遇到的物件移除
                    Shape selectedShape = e.OriginalSource as Shape;
                    myCanvas.Children.Remove(selectedShape);
                    if (myCanvas.Children.Count == 0) myCanvas.Cursor = Cursors.Arrow;
                    break;
                default:
                    return;
            }
        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (currentShape)
            {
                case "Line":
                    DrawLine();
                    break;
                case "Rectangle":
                    DrawRectangle();
                    break;
                case "Ellipse":
                    DrawEllipse();
                    break;
                default:
                    return;
            }
            //回復原本的滑鼠游標
            myCanvas.Cursor = Cursors.Arrow;
        }
        private void AdjustPoint()
        {
            double X_min, Y_min, X_max, Y_max;

            X_min = Math.Min(start.X, dest.X);
            Y_min = Math.Min(start.Y, dest.Y);
            X_max = Math.Min(start.X, dest.X);
            Y_max = Math.Max(start.Y, dest.Y);

            start.X = X_min;
            start.Y = Y_min;
            dest.X = X_max;
            dest.Y = Y_max;
        }

        private void DrawLine()
        {
            Line newLine = new Line();
            newLine.Stroke = currentStrokeBrush;
            newLine.StrokeThickness = currentStrokeThickness;
            newLine.X1 = start.X;
            newLine.Y1 = start.Y;
            newLine.X2 = dest.X;
            newLine.Y2 = dest.Y;

            myCanvas.Children.Add(newLine);
        }

        private void DrawRectangle()
        {
            AdjustPoint();
            double width = dest.X - start.X;
            double height = dest.Y - start.Y;
            Rectangle newRectangle = new Rectangle()
            {
                Stroke = currentStrokeBrush,
                StrokeThickness = currentStrokeThickness,
                Fill = currentFillBrush,
                Width = width,
                Height = height
            };

            newRectangle.SetValue(Canvas.LeftProperty, start.X);
            newRectangle.SetValue(Canvas.TopProperty, start.Y);
            myCanvas.Children.Add(newRectangle);
        }

        private void DrawEllipse()
        {
            AdjustPoint();
            double width = dest.X - start.X;
            double height = dest.Y - start.Y;
            Ellipse newEllipse = new Ellipse()
            {
                Stroke = currentStrokeBrush,
                StrokeThickness = currentStrokeThickness,
                Fill = currentFillBrush,
                Width = width,
                Height = height
            };

            newEllipse.SetValue(Canvas.LeftProperty, start.X);
            newEllipse.SetValue(Canvas.TopProperty, start.Y);
            myCanvas.Children.Add(newEllipse);
        }

        private void StrokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentStrokeThickness = Convert.ToInt32(StrokeThicknessSlider.Value);
        }

        private void StrokeButton_Click(object sender, RoutedEventArgs e)
        {
            currentStrokeColor = GetDialogColor();
            currentStrokeBrush = new SolidColorBrush(currentStrokeColor);
            StrokeButton.Background = currentStrokeBrush;
        }

        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            currentFillColor = GetDialogColor();
            currentStrokeBrush = new SolidColorBrush(currentFillColor);
            FillButton.Background = currentFillBrush;
        }

        private Color GetDialogColor()
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            dlg.ShowDialog();
            System.Drawing.Color dlgColor = dlg.Color;
            return Color.FromArgb(dlgColor.A, dlgColor.R, dlgColor.G, dlgColor.B);
        }
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            currentAction = "Draw";
            RadioButton btn = sender as RadioButton;
            currentShape = btn.Content.ToString();
            myCanvas.Cursor = Cursors.Arrow;
            myLabel.Content = $"畫{currentShape}";
        }

        private void EraseButton_Click(object sender, RoutedEventArgs e)
        {
            currentAction = "Erase";
            if (myCanvas.Children.Count != 0)
            {
                myCanvas.Cursor = Cursors.Hand;
                myLabel.Content = "橡皮擦模式";
            }
        }


        //-----客製功能表開始-----
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("小畫家\n版本1.0\nCopyright © 小畫家作者\n保留一切權利。", "關於小畫家", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void bye(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(881);
        }
        //-----客製功能表結束-----

    }
}