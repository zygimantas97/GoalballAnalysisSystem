using Emgu.CV;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.GameProcessing.Models;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.MLBasedObjectDetection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.PlayersTracker
{
    public class ONNXBasedMOT : IMOT
    {
        private readonly List<ONNXTrackingObject> _trackingObjects = new List<ONNXTrackingObject>();
        private readonly MLBasedObjectDetectionStrategy objectDetectionStrategy = new MLBasedObjectDetectionStrategy(new List<string>() { "player" }, 0.2f);

        public List<Rectangle> UpdateTrackingObjects(Mat frame)
        {
            List<Rectangle> objects = new List<Rectangle>();
            if (_trackingObjects.Count >= 1)
            {

                var predictedPlayers = objectDetectionStrategy.DetectAllObjects(frame.ToBitmap());

                foreach (var predictedPlayer in predictedPlayers)
                {
                    int playerIndex = DetermineTrackingPlayer(_trackingObjects, predictedPlayer);
                    _trackingObjects[playerIndex].AddPredictionBoundary(predictedPlayer);
                }

                foreach (ONNXTrackingObject trackingObject in _trackingObjects)
                {
                    trackingObject.DetermineAndUpdateMostFittingPrediction();
                    trackingObject.ClearPredictionBoundaries();
                    objects.Add(trackingObject.ROI);
                }

                _trackingObjects[0].ClearPredictionBoundaries();
            }

            return objects;
        }

        public void AddTrackingObject(Mat frame, Rectangle roi, int objectId = 0)
        {
            ONNXTrackingObject trackingObject = new ONNXTrackingObject(objectId, roi);

            _trackingObjects.Add(trackingObject);

        }

        /// <summary>
        /// Determine the player, which best fits the given prediction boundary
        /// </summary>
        /// <param name="trackedObjects">all tracked players</param>
        /// <param name="boundary">prediction boundary that needs to be determined for one of the players</param>
        /// <returns>index of player in the list</returns>
        private int DetermineTrackingPlayer(List<ONNXTrackingObject> trackedObjects, Rectangle boundary)
        {
            Dictionary<int, double> distances = new Dictionary<int, double>();
            int counter = 0;
            int indexValue = -1;

            foreach (ONNXTrackingObject trackedObject in trackedObjects)
            {
                double dist = 0;
                for (int i=1; i<3; i++)
                {
                    dist += trackedObject.DistanceToPreviousPoint(boundary, i);
                }
                distances.Add(counter, dist);
                counter++;
            }

            if (distances.Count > 0)
            {
                var keyAndValue = distances.OrderBy(kvp => kvp.Value).First();
                indexValue = keyAndValue.Key;
            }

            return indexValue;
        }
    }
}
