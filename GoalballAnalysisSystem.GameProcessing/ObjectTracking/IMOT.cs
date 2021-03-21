using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectTracking
{
    public interface IMOT<T> where T : class
    {
        public void Add(T obj, Mat frame, Rectangle roi);
        Task<Dictionary<T, Rectangle>> Update(Mat frame);
        public void Remove(T obj);
        public void RemoveAt(Point location);
        public void RemoveAll();
    }
}
