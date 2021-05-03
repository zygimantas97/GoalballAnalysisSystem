using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.ComponentModel;
using Microsoft.Win32;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System.Collections.ObjectModel;

namespace GoalballAnalysisSystem.WPF.View
{
    /// <summary>
    /// Interaction logic for GamesView.xaml
    /// </summary>
    public partial class AnalysisView : UserControl
    {
        private const int XPADDING = 50;
        private const int YPADDING = 50;

        private const int PLAYGROUND_WIDTH = 900;
        private const int PLAYGROUND_HEIGHT = 1800;
        private const int YZONEHEIGHT = 300;

        private int[] xZones = { 0, 50, 275, 325, 575, 625, 850, 900 };

        private AnalysisViewModel _dataContext;
        /*private StackPanel previousMarked;*/
        private Image<Bgr, byte> _playgroundImageBoxBackground;

        public AnalysisView()
        {
            InitializeComponent();
            _playgroundImageBoxBackground = Playground();
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;

        }

        private async void PlaygroundImageBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_dataContext == null)
                _dataContext = (AnalysisViewModel)this.DataContext;

            if (_dataContext != null)
            {
                _playgroundImageBoxBackground = Playground();
                System.Drawing.Point coordinates = e.Location;
                double horizontalScale = (double)_playgroundImageBoxBackground.Width / (double)PlaygroundImageBox.Width;
                double verticalScale = (double)_playgroundImageBoxBackground.Height / (double)PlaygroundImageBox.Height;

                double x = coordinates.X * horizontalScale;
                double y = coordinates.Y * verticalScale;
                _dataContext.SelectedGameZone = new System.Drawing.Rectangle();

                //determine where was clicked
                for (int i = 0; i < xZones.Length - 1; i++)
                {
                    if (x >= xZones[i] + XPADDING && x <= xZones[i + 1] + XPADDING)
                    {
                        if (y >= YPADDING && y <= YZONEHEIGHT + YPADDING)
                        {
                            CvInvoke.Rectangle(_playgroundImageBoxBackground, new System.Drawing.Rectangle(xZones[i] + XPADDING, YPADDING, xZones[i + 1] - xZones[i], YZONEHEIGHT), new MCvScalar(255, 0, 0), 10);
                            _dataContext.SelectedGameZone = new System.Drawing.Rectangle(xZones[i], 0, xZones[i + 1] - xZones[i], YZONEHEIGHT);
                        }
                        else if (y >= YPADDING + PLAYGROUND_HEIGHT - YZONEHEIGHT && y <= YPADDING + PLAYGROUND_HEIGHT)
                        {
                            CvInvoke.Rectangle(_playgroundImageBoxBackground, new System.Drawing.Rectangle(xZones[i] + XPADDING, YPADDING + PLAYGROUND_HEIGHT - YZONEHEIGHT, xZones[i + 1] - xZones[i], YZONEHEIGHT), new MCvScalar(255, 0, 0), 10);
                            _dataContext.SelectedGameZone = new System.Drawing.Rectangle(xZones[i], 0 + PLAYGROUND_HEIGHT - YZONEHEIGHT, xZones[i + 1] - xZones[i], YZONEHEIGHT);
                        }
                    }
                }
                await _dataContext.RefreshProjectionsList();
                DrawAllVectors(_dataContext.ListOfProjections);
            }
        }

        private void OnPreviousProjectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (_dataContext == null)
                _dataContext = (AnalysisViewModel)this.DataContext;
            var projection = _dataContext.PreviousProjection();
            if (projection != null)
                DrawVector(projection);
        }

        private void OnNextProjectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (_dataContext == null)
                _dataContext = (AnalysisViewModel)this.DataContext;
            var projection = _dataContext.NextProjection();
            if (projection != null)
                DrawVector(projection);
        }

        private void DrawVector(ProjectionResponse projection)
        {
            Image<Bgr, byte> playgroundImageBoxBackground = _playgroundImageBoxBackground.Clone();
            var startPoint = new System.Drawing.Point(projection.X1 + XPADDING, projection.Y1 + YPADDING);
            var endPoint = new System.Drawing.Point(projection.X2 + XPADDING, projection.Y2 + YPADDING);
            CvInvoke.ArrowedLine(playgroundImageBoxBackground, startPoint, endPoint, new MCvScalar(0, 0, 0), 5, Emgu.CV.CvEnum.LineType.Filled, 0, 0.02);
            PlaygroundImageBox.Image = playgroundImageBoxBackground;
        }

        private void DrawAllVectors(ObservableCollection<ProjectionResponse> projections)
        {
            Image<Bgr, byte> playgroundImageBoxBackground = _playgroundImageBoxBackground.Clone();
            if(_dataContext.SelectedGameZone != new System.Drawing.Rectangle() || _dataContext.SelectedPlayer != null)
            {
                foreach (var projection in projections)
                {
                    var startPoint = new System.Drawing.Point(projection.X1 + XPADDING, projection.Y1 + YPADDING);
                    var endPoint = new System.Drawing.Point(projection.X2 + XPADDING, projection.Y2 + YPADDING);
                    CvInvoke.ArrowedLine(playgroundImageBoxBackground, startPoint, endPoint, new MCvScalar(0, 0, 0), 5, Emgu.CV.CvEnum.LineType.Filled, 0, 0.02);
                }
            }
            PlaygroundImageBox.Image = playgroundImageBoxBackground;
        }
        private void OnSelectGameButtonClick(object sender, RoutedEventArgs e)
        {
            _playgroundImageBoxBackground = Playground();
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;
        }

        private void OnSelectPlayerButtonClick(object sender, RoutedEventArgs e)
        {
            if (_dataContext == null)
                _dataContext = (AnalysisViewModel)this.DataContext;
            /*
            Button button = sender as Button;
            StackPanel stack = button.Content as StackPanel;

            if (previousMarked != null)
                previousMarked.Background = GetColorFromHexa("#D9E1E2");

            stack.Background = GetColorFromHexa("#edebe1");
            previousMarked = stack;*/

            _dataContext.RefreshProjectionsList();
            DrawAllVectors(_dataContext.ListOfProjections);

        }

        private async void IncomingChecked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null)
            {
                _dataContext.IncomingProjectionsChecked = true;
                await _dataContext.RefreshProjectionsList();
                DrawAllVectors(_dataContext.ListOfProjections);
            }
        }

        private async void IncomingUnchecked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null)
            {
                _dataContext.IncomingProjectionsChecked = false;
                await _dataContext.RefreshProjectionsList();
                DrawAllVectors(_dataContext.ListOfProjections);
            }
        }

        private async void OutgoingChecked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null)
            {
                _dataContext.OutgoingProjectionsChecked = true;
                await _dataContext.RefreshProjectionsList();
                DrawAllVectors(_dataContext.ListOfProjections);
            }
        }

        private async void OutgoingUnchecked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null)
            {
                _dataContext.OutgoingProjectionsChecked = false;
                await _dataContext.RefreshProjectionsList();
                DrawAllVectors(_dataContext.ListOfProjections);
            }
        }

        private Image<Bgr, byte> Playground()
        {
            Image<Bgr, byte> playgroundImageBoxBackground = new Image<Bgr, byte>(PLAYGROUND_WIDTH + 2 * XPADDING, PLAYGROUND_HEIGHT + 2 * YPADDING, new Bgr(255, 255, 255)); //two times from both sides

            //top
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, YPADDING), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, YPADDING), new MCvScalar(255, 160, 160), 10);

            //bottom
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, PLAYGROUND_HEIGHT + YPADDING), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 10);

            //left
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, YPADDING), new System.Drawing.Point(XPADDING, PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 10);

            //right
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, YPADDING), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 10);

            int zonesCount = PLAYGROUND_HEIGHT / YZONEHEIGHT;
            for (int i = 0; i < zonesCount; i++)
            {
                CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, YPADDING + (i * YZONEHEIGHT)), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, YPADDING + (i * YZONEHEIGHT)), new MCvScalar(255, 160, 160), 4);
            }

            for (int i = 0; i < xZones.Length; i++)
            {
                //top
                CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING + xZones[i], YPADDING), new System.Drawing.Point(XPADDING + xZones[i], YPADDING + YZONEHEIGHT), new MCvScalar(255, 160, 160), 4);

                //bottom
                CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING + xZones[i], PLAYGROUND_HEIGHT + YPADDING - YZONEHEIGHT), new System.Drawing.Point(XPADDING + xZones[i], PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 4);
            }

            return playgroundImageBoxBackground;
        }

        private SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, R, G, B));
            return scb;
        }


    }
}
