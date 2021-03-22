using GoalballAnalysisSystem.Tracking;
using NUnit.Framework;
using System;

namespace GoalballAnalysisSystem.Tracking.Tests.TrackingCalculations
{
    [TestFixture]
    public class EquationTests
    {
        private Equation CreateEquationObjective(int x1, int y1, int x2, int y2, bool pointsAreBellow)
        {
            return new Equation(new CustomPoint(x1, y1), new CustomPoint(x2, y2), pointsAreBellow);
        }
        private Equation CreateEquation(int x1, int y1, int x2, int y2)
        {
            return new Equation(x1, y1, x2, y2);
        }

        [Test]
        public void Constructor_WithSameCoordinates_ReturnsSameEquation()
        {
            // Arrange
            var equation1 = CreateEquation(1, 1, 5, 5);
            var equation2 = CreateEquationObjective(1, 1, 5, 5, true);

            // Act

            // Assert
            Assert.AreEqual(equation1.a, equation2.a);
            Assert.AreEqual(equation2.b, equation2.b);
        }
        
        [Test]
        public void SetAnotherPoint_WithSameCoordinates_ReturnsSameEquations()
        {
            // Arrange
            var equation1 = CreateEquation(1, 1, 5, 5);
            var equation2 = new Equation(1, 1);
            var x = 5;
            var y = 5;

            // Act
            equation2.SetAnotherPoint(x, y);

            // Assert
            Assert.AreEqual(equation1.a, equation2.a);
            Assert.AreEqual(equation2.b, equation2.b);
        }
        
        [Test]
        public void GetX_WithNegativeY_ReturnsCorrectResult()
        {
            // Arrange
            var equation = CreateEquation(1, 1, 5, 9);
            double y = -1;
            double expectedX = 0;

            // Act
            var result = equation.GetX(y);

            // Assert
            Assert.AreEqual(expectedX, result);
        }

        [Test]
        public void GetX_WithYEqual0_ReturnsCorrectResult()
        {
            // Arrange
            var equation = CreateEquation(1, 1, 5, 9);
            double y = 0;
            double expectedX = 0.5;

            // Act
            var result = equation.GetX(y);

            // Assert
            Assert.AreEqual(expectedX, result);
        }

        [Test]
        public void GetX_WithPositiveY_ReturnsCorrectResult()
        {
            // Arrange
            var equation = CreateEquation(1, 1, 5, 9);
            double y = 1;
            double expectedX = 1;

            // Act
            var result = equation.GetX(y);

            // Assert
            Assert.AreEqual(expectedX, result);
        }

        [Test]
        public void GetY_WithNegativeX_ReturnsCorrectResult()
        {
            // Arrange
            var equation = CreateEquation(1, 1, 5, 9);
            double x = -1;
            double expectedY = -3;

            // Act
            var result = equation.GetY(x);

            // Assert
            Assert.AreEqual(expectedY, result);
        }

        [Test]
        public void GetY_WithXEquals0_ReturnsCorrectResult()
        {
            // Arrange
            var equation = CreateEquation(1, 1, 5, 9);
            double x = 0;
            double expectedY = -1;

            // Act
            var result = equation.GetY(x);

            // Assert
            Assert.AreEqual(expectedY, result);
        }

        [Test]
        public void GetY_WithPositiveX_ReturnsCorrectResult()
        {
            // Arrange
            var equation = CreateEquation(1, 1, 5, 9);
            double x = 10;
            double expectedY = 19;

            // Act
            var result = equation.GetY(x);

            // Assert
            Assert.AreEqual(expectedY, result);
        }

        [Test]
        public void IsPointSuitable_WithPointsAreAboveAndPointBellowEquation_ReturnsFalse()
        {
            // Arrange
            var equation = CreateEquationObjective(1, 1, 5, 5, false);
            CustomPoint point = new CustomPoint(3, 2);
            var expectedResult = false;

            // Act
            var result = equation.IsPointSuitable(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void IsPointSuitable_WithPointsAreAboveAndPointOnEquation_ReturnsTrue()
        {
            // Arrange
            var equation = CreateEquationObjective(1, 1, 5, 5, false);
            CustomPoint point = new CustomPoint(3, 3);
            var expectedResult = true;

            // Act
            var result = equation.IsPointSuitable(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void IsPointSuitable_WithPointsAreAboveAndPointAboveEquation_ReturnsTrue()
        {
            // Arrange
            var equation = CreateEquationObjective(1, 1, 5, 5, false);
            CustomPoint point = new CustomPoint(3, 4);
            var expectedResult = true;

            // Act
            var result = equation.IsPointSuitable(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
