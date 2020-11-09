using GoalballAnalysisSystem.Tracking;
using NUnit.Framework;
using System;
using System.Drawing;

namespace GoalballAnalysisSystem.Tracking.Tests.TrackingCalculations
{
    [TestFixture]
    public class CustomPointTests
    {
        private CustomPoint CreateCustomPoint(int x, int y)
        {
            return new CustomPoint(x, y);
        }

        [Test]
        public void CompareTo_CompareWithLessXLessY_ReturnsGreater()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(0, 0);
            var expectedResult = 1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithLessXEqualY_ReturnsGreater()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(0, 1);
            var expectedResult = 1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithLessXGreaterY_ReturnsGreater()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(0, 2);
            var expectedResult = 1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithGreaterXLessY_ReturnsLess()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(2, 0);
            var expectedResult = -1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithGreaterXEqualY_ReturnsLess()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(2, 1);
            var expectedResult = -1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithGreaterXGreaterY_ReturnsLess()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(2, 2);
            var expectedResult = -1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithEqualXLessY_ReturnsGreater()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(1, 0);
            var expectedResult = 1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithEqualXEqualY_ReturnsEqual()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(1, 1);
            var expectedResult = 0;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CompareTo_CompareWithEqualXGreaterY_ReturnsLess()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(2, 2);
            var expectedResult = -1;

            // Act
            var result = firstCustomPoint.CompareTo(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void ConvertoToDrawingPoint_ReturnsPointObjectWithSameXAndY()
        {
            // Arrange
            var customPoint = this.CreateCustomPoint(1, 1);

            // Act
            var result = customPoint.ConvertoToDrawingPoint();

            // Assert
            Assert.IsInstanceOf<Point>(result);
            Assert.AreEqual(customPoint.X, result.X);
            Assert.AreEqual(customPoint.Y, result.Y);
        }
        
        [Test]
        public void Equals_CompareWithEqualXAndY_ReturnsEqual()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(1, 1);
            var expectedResult = true;

            // Act
            var result = firstCustomPoint.Equals(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Equals_CompareWithNull_ReturnsFalse()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var expectedResult = false;

            // Act
            var result = firstCustomPoint.Equals(null);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Equals_CompareWithNotCustomPointObject_ReturnsFalse()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var point = new Point(1, 1);
            var expectedResult = false;

            // Act
            var result = firstCustomPoint.Equals(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Equals_CompareWithAbsX0AbsY0_ReturnsTrue()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(1, 1);
            var expectedResult = true;

            // Act
            var result = firstCustomPoint.Equals(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Equals_CompareWithAbsX4AbsY4_ReturnsTrue()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(5, 5);
            var expectedResult = true;

            // Act
            var result = firstCustomPoint.Equals(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Equals_CompareWithAbsX5AbsY5_ReturnsTrue()
        {
            // Arrange
            var firstCustomPoint = this.CreateCustomPoint(1, 1);
            var secondCustomPoint = this.CreateCustomPoint(6, 6);
            var expectedResult = false;

            // Act
            var result = firstCustomPoint.Equals(secondCustomPoint);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetHashCode_ReturnsCorrectHashCode()
        {
            // Arrange
            var x = 1;
            var y = 1;
            var customPoint = this.CreateCustomPoint(x, y);
            var expectedResult = 1861411795;
            expectedResult = expectedResult * -1521134295 + x;
            expectedResult = expectedResult * -1521134295 + y;

            // Act
            var result = customPoint.GetHashCode();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
