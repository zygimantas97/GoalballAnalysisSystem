using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.APIBasedObjectDetectionStrategy
{
    public partial class Prediction
    {
        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("tagId")]
        public Guid TagId { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }

        [JsonProperty("boundingBox")]
        public BoundingBox BoundingBox { get; set; }
    }
}
