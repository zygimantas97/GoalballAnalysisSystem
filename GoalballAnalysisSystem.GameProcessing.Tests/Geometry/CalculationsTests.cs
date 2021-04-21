using GoalballAnalysisSystem.GameProcessing.Geometry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Tests.Geometry
{
    [TestFixture]
    class CalculationsTests
    {
        [Test]
        public void GetDistanceBetweenPoints_ReturnsDistance()
        {
            // Arrange
            var startPoint = new Point(0, 0);
            var endPoint = new Point(3, 4);
            double expectedDistance = 5;

            // Act
            double actualDistance = Calculations.GetDistanceBetweenPoints(startPoint, endPoint);

            // Assert
            Assert.AreEqual(expectedDistance, actualDistance);
        }

        [Test]
        public void GetMiddlePoint_WithRectangle_ReturnsMiddlePoint()
        {
            // Arrange
            var rectangle = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = 10,
                Height = 10
            };
            var expectedPoint = new Point(5, 5);

            // Act
            var actualPoint = Calculations.GetMiddlePoint(rectangle);

            // Assert
            Assert.AreEqual(expectedPoint.X, actualPoint.X);
            Assert.AreEqual(expectedPoint.Y, actualPoint.Y);
        }

        [Test]
        public void GetMiddlePoint_WithTwoPoints_ReturnsMiddlePoint()
        {
            // Arrange
            var startPoint = new Point(0, 0);
            var endPoint = new Point(10, 10);
            var expectedPoint = new Point(5, 5);

            // Act
            var actualPoint = Calculations.GetMiddlePoint(startPoint, endPoint);

            // Assert
            Assert.AreEqual(expectedPoint.X, actualPoint.X);
            Assert.AreEqual(expectedPoint.Y, actualPoint.Y);
        }

        public void GetRotationSin()
        {
            
        }

        public void GetRotationCos()
        {
            
        }

        public void RotatePoint()
        {
            
        }
    }
}
