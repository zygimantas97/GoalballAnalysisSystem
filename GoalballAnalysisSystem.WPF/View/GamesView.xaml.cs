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

namespace GoalballAnalysisSystem.WPF.View
{
    /// <summary>
    /// Interaction logic for GamesView.xaml
    /// </summary>
    public partial class GamesView : UserControl
    {
        private const int XPADDING = 50;
        private const int YPADDING = 50;

        private const int PLAYGROUND_WIDTH = 900;
        private const int PLAYGROUND_HEIGHT = 1800;
        private const int YZONEHEIGHT = 300;

        int[] xZones = { 0, 50, 275, 325, 575, 625, 850, 900 };

        private GamesViewModel _gamesViewModel;
        private Image<Bgr, byte> _playgroundImageBoxBackground;

        public GamesView()
        {
            InitializeComponent();
            _playgroundImageBoxBackground = Playground();
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;

        }

        private void OnPreviousProjectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (_gamesViewModel == null)
                _gamesViewModel = (GamesViewModel)this.DataContext;
            var projection = _gamesViewModel.PreviousProjection();
            if (projection != null)
                DrawVector(projection);
        }

        private void OnNextProjectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (_gamesViewModel == null)
                _gamesViewModel = (GamesViewModel)this.DataContext;
            var projection = _gamesViewModel.NextProjection();
            if (projection != null)
                DrawVector(projection);
        }

        private void DrawVector(ProjectionResponse projection)
        {
            if (_gamesViewModel == null)
                _gamesViewModel = (GamesViewModel)this.DataContext;

            Image<Bgr, byte> playgroundImageBoxBackground = _playgroundImageBoxBackground.Clone();

            var startPoint = new System.Drawing.Point(projection.X1 + XPADDING, projection.Y1 + YPADDING);
            var endPoint = new System.Drawing.Point(projection.X2 + XPADDING, projection.Y2 + YPADDING);

            CvInvoke.Line(playgroundImageBoxBackground, startPoint, endPoint, new MCvScalar(0, 0, 0), 5);
            PlaygroundImageBox.Image = playgroundImageBoxBackground;

        }

        private void OnSelectGameButtonClick(object sender, RoutedEventArgs e)
        {
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;
        }

        private void OnSelectPlayerButtonClick(object sender, RoutedEventArgs e)
        {
            ;
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

    }
}
