using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.CustomVision.Models
{
    public class CustomVisionPredictionResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("project")]
        public Guid Project { get; set; }

        [JsonProperty("iteration")]
        public Guid Iteration { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("predictions")]
        public Prediction[] Predictions { get; set; }
    }
}
