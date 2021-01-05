using GoalballAnalysisSystem.GameProcessing.BallTracker;
using GoalballAnalysisSystem.GameProcessing.PlayersTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Microsoft.Win32;

namespace GoalballAnalysisSystem.GameProcessing.Developer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameAnalyzer GameAnalyzer { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectVideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? result = fileDialog.ShowDialog();
            if(result == true)
            {
                string fileName = fileDialog.FileName;

                var topLeft = new System.Drawing.Point(0, 0);
                var topRight = new System.Drawing.Point(0, 0);
                var bottomRight = new System.Drawing.Point(0, 0);
                var bottomLeft = new System.Drawing.Point(0, 0);

                IBallTracker ballTracker = new CNNBasedBallTracker();
                IPlayersTracker playersTracker = new EmguCVTrackersBasedPlayersTracker();

                GameAnalyzer = new GameAnalyzer(fileName,
                                              topLeft, topRight, bottomRight, bottomLeft,
                                                ballTracker, playersTracker);
                
                VideoStackPanel.Visibility = Visibility.Visible;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            GameAnalyzer.Pause();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            GameAnalyzer.Resume();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            GameAnalyzer.Stop();
        }
    }
}
