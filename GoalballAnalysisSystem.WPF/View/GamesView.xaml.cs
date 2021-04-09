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
        private const int PLAYGROUND_WIDTH = 900;
        private const int PLAYGROUND_HEIGHT = 1800;
        private const int SELECTION_ZONE_TOP = 300;
        private const int SELECTION_ZONE_BOTTOM = 1200;
        private const int MAX_SELECTION_DISTANCE = 100;

        
        private GamesViewModel _gamesViewModel;
        Image<Bgr, byte> _playgroundImageBoxBackground;

        public GamesView()
        {
            InitializeComponent();
            _playgroundImageBoxBackground = new Image<Bgr, byte>(PLAYGROUND_WIDTH, PLAYGROUND_HEIGHT, new Bgr(255, 255, 255));
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;

        }

        void OnPreviousProjectionButtonClick(object sender, RoutedEventArgs e)
        {
            if(_gamesViewModel==null)
                _gamesViewModel = (GamesViewModel)this.DataContext;
            var projection = _gamesViewModel.PreviousProjection();
            if(projection != null)
                DrawVector(projection);
        }

        void OnNextProjectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (_gamesViewModel == null)
                _gamesViewModel = (GamesViewModel)this.DataContext;
            var projection = _gamesViewModel.NextProjection();
            if (projection != null)
                DrawVector(projection);
        }

        void DrawVector(ProjectionResponse projection)
        {
            _gamesViewModel = (GamesViewModel)this.DataContext;
            Image<Bgr, byte> playgroundImageBoxBackground = _playgroundImageBoxBackground.Clone();

            var startPoint = new System.Drawing.Point(projection.X1, projection.Y1);
            var endPoint = new System.Drawing.Point(projection.X2, projection.Y2);

            CvInvoke.Line(playgroundImageBoxBackground, startPoint, endPoint, new MCvScalar(0, 0, 0), 5);
            PlaygroundImageBox.Image = playgroundImageBoxBackground;

        }

        void OnSelectGameButtonClick(object sender, RoutedEventArgs e)
        {
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;
        }


    }
}
