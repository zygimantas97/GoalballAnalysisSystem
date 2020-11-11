using GoalballAnalysisSystem.Tracking;
using GoalballAnalysisSystem.Tracking.Enums;
using NUnit.Framework;
using System;

namespace GoalballAnalysisSystem.Tracking.Tests.TrackingCalculations
{
    [TestFixture]
    public class CustomVectorTests
    {
        [Test]
        public void TryAddPoint_WithOnePointAndDistanceLessThanMaxDistance_ReturnsTrueAndAddPoint()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion-1);
            var expectedResult = true;
            var expectedCountOfPoints = customVector.pointsOfVector.Count + 1; ;

            // Act
            var result = customVector.TryAddPoint(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedCountOfPoints, customVector.pointsOfVector.Count);
        }

        [Test]
        public void TryAddPoint_WithOnePointAndDistanceEqualMaxDistance_ReturnsTrueAndAddPoint()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion);
            var expectedResult = true;
            var expectedCountOfPoints = customVector.pointsOfVector.Count + 1;

            // Act
            var result = customVector.TryAddPoint(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedCountOfPoints, customVector.pointsOfVector.Count);
        }

        [Test]
        public void TryAddPoint_WithOnePointAndDistanceGreaterThanMaxDistance_ReturnsFalseAndNotAddPoint()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion+1);
            var expectedResult = false;
            var expectedCountOfPoints = customVector.pointsOfVector.Count;

            // Act
            var result = customVector.TryAddPoint(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedCountOfPoints, customVector.pointsOfVector.Count);
        }

        [Test]
        public void TryAddPoint_WithMoreThanOnePointAndDistanceLessThanMaxDistance_ReturnsTrueAndAddPoint()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(-1, -1));
            customVector.pointsOfVector.Add(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion - 1);
            var expectedResult = true;
            var expectedCountOfPoints = customVector.pointsOfVector.Count + 1; ;

            // Act
            var result = customVector.TryAddPoint(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedCountOfPoints, customVector.pointsOfVector.Count);
        }

        [Test]
        public void TryAddPoint_WithMoreThanOnePointAndDistanceEqualMaxDistance_ReturnsTrueAndAddPoint()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(-1, -1));
            customVector.pointsOfVector.Add(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion);
            var expectedResult = true;
            var expectedCountOfPoints = customVector.pointsOfVector.Count + 1;

            // Act
            var result = customVector.TryAddPoint(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedCountOfPoints, customVector.pointsOfVector.Count);
        }

        [Test]
        public void TryAddPoint_WithMoreThanOnePointAndDistanceGreaterThanMaxDistance_ReturnsFalseAndNotAddPoint()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(-1, -1));
            customVector.pointsOfVector.Add(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion + 1);
            var expectedResult = false;
            var expectedCountOfPoints = customVector.pointsOfVector.Count;

            // Act
            var result = customVector.TryAddPoint(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedCountOfPoints, customVector.pointsOfVector.Count);
        }

        [Test]
        public void CheckDistantion_WithDistanceLessThanMaxDistance_ReturnsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion-1);
            var expectedResult = true;

            // Act
            var result = customVector.CheckDistantion(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDistantion_WithDistanceEqualMaxDistance_ReturnsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion);
            var expectedResult = true;

            // Act
            var result = customVector.CheckDistantion(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDistantion_WithDistanceGreaterThanMaxDistance_ReturnsFalse()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point = new CustomPoint(0, CustomVector.maxDistantion+1);
            var expectedResult = false;

            // Act
            var result = customVector.CheckDistantion(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetDistation_WithEqualCoordinates_Returns0()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point1 = new CustomPoint(0, 0);
            var point2 = new CustomPoint(0, 0);
            var expectedResult = 0;

            // Act
            var result = customVector.GetDistation(point1, point2);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetDistation_WithNotEqualCoordinates_ReturnsResultByPitagor()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            var point1 = new CustomPoint(0, 0);
            var point2 = new CustomPoint(3, 4);
            var expectedResult = 5;

            // Act
            var result = customVector.GetDistation(point1, point2);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TryToComplete_WithLastYLessThanMinVal_SetsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(1000, 1000));
            customVector.pointsOfVector.Add(new CustomPoint(1000, CustomVector.minVal-1));
            var expectedResult = true;

            // Act
            customVector.TryToComplete();

            // Assert
            Assert.AreEqual(expectedResult, customVector.isCompleted);
        }

        [Test]
        public void TryToComplete_WithLastYGreaterThanMaxVal_SetsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(1000, 1000));
            customVector.pointsOfVector.Add(new CustomPoint(1000, CustomVector.maxVal + 1));
            var expectedResult = true;

            // Act
            customVector.TryToComplete();

            // Assert
            Assert.AreEqual(expectedResult, customVector.isCompleted);
        }

        [Test]
        public void TryToComplete_WithLastYGreaterThanMinValAndLastYLessThanMaxVal_SetsFalse()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(1000, 1000));
            customVector.pointsOfVector.Add(new CustomPoint(1000, CustomVector.maxVal-1));
            var expectedResult = false;

            // Act
            customVector.TryToComplete();

            // Assert
            Assert.AreEqual(expectedResult, customVector.isCompleted);
        }

        [Test]
        public void CheckDirection_WithFirstYGreaterThanLastYAndLastYGreaterThanPointYAndFirstXLessThanLastXAndLastXLessThanPointX_ReturnsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(3, 5));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(6, 2);
            var expectedResult = true;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYGreaterThanLastYAndLastYGreaterThanPointYAndFirstXLessThanLastXAndLastXGreaterThanPointX_ReturnsFalse()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(3, 5));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(4, 2);
            var expectedResult = false;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYGreaterThanLastYAndLastYGreaterThanPointYAndFirstXGreaterThanLastXAndLastXGreaterThanPointX_ReturnsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(6, 5));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(4, 2);
            var expectedResult = true;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYGreaterThanLastYAndLastYGreaterThanPointYAndFirstXGreaterThanLastXAndLastXLessThanPointX_ReturnsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(6, 5));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(7, 2);
            var expectedResult = false;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYGreaterThanLastYAndLastYLessThanPointY_ReturnsFalse()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(3, 5));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(6, 4);
            var expectedResult = false;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYLessThanLastYAndLastYLessThanPointYAndFirstXLessThanLastXAndLastXLessThanPointX_ReturnsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(2, 2));
            customVector.pointsOfVector.Add(new CustomPoint(3, 3));
            var point = new CustomPoint(5, 5);
            var expectedResult = true;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYLessThanLastYAndLastYLessThanPointYAndFirstXLessThanLastXAndLastXGreaterThanPointX_ReturnsFalse()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(2, 2));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(3, 5);
            var expectedResult = false;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYLessThanLastYAndLastYLessThanPointYAndFirstXGreaterThanLastXAndLastXGreaterThanPointX_ReturnsTrue()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(7, 2));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(3, 5);
            var expectedResult = true;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYLessThanLastYAndLastYLessThanPointYAndFirstXGreaterThanLastXAndLastXLessThanPointX_ReturnsFalse()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(7, 2));
            customVector.pointsOfVector.Add(new CustomPoint(5, 3));
            var point = new CustomPoint(6, 5);
            var expectedResult = false;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CheckDirection_WithFirstYLessThanLastYAndLastYGreaterThanPointY_ReturnsFalse()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(7, 2));
            customVector.pointsOfVector.Add(new CustomPoint(5, 5));
            var point = new CustomPoint(6, 4);
            var expectedResult = false;

            // Act
            var result = customVector.CheckDirection(point);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SetEquation_WithFirstXLessThanLastXAndFirstYLessThanLastY_ReturnsEquationWithPositiveDirectionCoefficientAndDirectionDown()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Down;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.Greater(customVector.equationOfVector.a, 0);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void SetEquation_WithFirstXLessThanLastXAndFirstYEqualLastY_ReturnsEquationWithDirectionCoefficientEqual0AndDirectionDown()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 1));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Up;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.AreEqual(0, customVector.equationOfVector.a);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void SetEquation_WithFirstXLessThanLastXAndFirstYGreaterThanLastY_ReturnsEquationWithNegativeDirectionCoefficientAndDirectionUp()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 2));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Up;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.Less(customVector.equationOfVector.a, 0);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void SetEquation_WithFirstXEqualLastXAndFirstYLessThanLastY_ReturnsEquationWithPositiveDirectionCoefficientAndDirectionDown()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(1, 0));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Down;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.Greater(customVector.equationOfVector.a, 0);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void SetEquation_WithFirstXEqualLastXAndFirstYEqualLastY_ReturnsEquationWithNanDirectionCoefficientAndDirectionUp()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(1, 1));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Up;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.IsNaN(customVector.equationOfVector.a);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void SetEquation_WithFirstXEqualLastXAndFirstYGreaterThanLastY_ReturnsEquationWithNegativeDirectionCoefficientAndDirectionUp()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(1, 2));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Up;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.Less(customVector.equationOfVector.a, 0);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void SetEquation_WithFirstXGreaterThanLastXAndFirstYEqualLastY_ReturnsEquationWithDirectionCoefficientEqual0AndDirectionUp()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(2, 1));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Up;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.AreEqual(0, customVector.equationOfVector.a);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void SetEquation_WithFirstXGreaterThanLastXAndFirstYGreaterThanLastY_ReturnsEquationWithPositiveDirectionCoefficientAndDirectionUp()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(2, 2));
            customVector.pointsOfVector.Add(new CustomPoint(1, 1));
            var expectedDirection = Direction.Up;

            // Act
            customVector.SetEquation();

            // Assert
            Assert.Greater(customVector.equationOfVector.a, 0);
            Assert.AreEqual(expectedDirection, customVector.direction);
        }

        [Test]
        public void TgOfAngle_WithK1EqualK2_Returns0()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            double k1 = 1;
            double k2 = 1;
            var expectedResult = 0;

            // Act
            var result = customVector.TgOfAngle(k1, k2);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TgOfAngle_WithK1NotEqualK2_ReturnsNot0()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            double k1 = 1;
            double k2 = -2;
            var expectedResult = -3;

            // Act
            var result = customVector.TgOfAngle(k1, k2);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TgOfAngle_WithK1AndK2OpositAndReverseNumbers_ReturnsInfinity()
        {
            // Arrange
            var customVector = new CustomVector(new CustomPoint(0, 0));
            double k1 = 1;
            double k2 = -1;
            var expectedResult = double.PositiveInfinity;

            // Act
            var result = customVector.TgOfAngle(k1, k2);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetBeginPoint_WithBottomUpVector_ReturnsPointWithY3800()
        {
            // Arrange
            var firstPoint = new CustomPoint(1000, 1000);
            var lastPoint = new CustomPoint(500, 500);
            var expectedPoint = new CustomPoint(3800, 3800);
            var customVector = new CustomVector(firstPoint);
            customVector.pointsOfVector.Add(lastPoint);
            customVector.SetEquation();

            // Act
            var result = customVector.GetBeginPoint();

            // Assert
            Assert.IsInstanceOf<CustomPoint>(result);
            Assert.AreEqual(expectedPoint.X, result.X);
            Assert.AreEqual(expectedPoint.Y, result.Y);
        }

        [Test]
        public void GetBeginPoint_WithTopDownVector_ReturnsPointWithY200()
        {
            // Arrange
            var firstPoint = new CustomPoint(500, 500);
            var lastPoint = new CustomPoint(1000, 1000);
            var expectedPoint = new CustomPoint(200, 200);
            var customVector = new CustomVector(firstPoint);
            customVector.pointsOfVector.Add(lastPoint);
            customVector.SetEquation();

            // Act
            var result = customVector.GetBeginPoint();

            // Assert
            Assert.IsInstanceOf<CustomPoint>(result);
            Assert.AreEqual(expectedPoint.X, result.X);
            Assert.AreEqual(expectedPoint.Y, result.Y);
        }

        [Test]
        public void GetEndPoint_WithBottomUpVector_ReturnsPointWithY200()
        {
            // Arrange
            var firstPoint = new CustomPoint(1000, 1000);
            var lastPoint = new CustomPoint(500, 500);
            var expectedPoint = new CustomPoint(200, 200);
            var customVector = new CustomVector(firstPoint);
            customVector.pointsOfVector.Add(lastPoint);
            customVector.SetEquation();

            // Act
            var result = customVector.GetEndPoint();

            // Assert
            Assert.IsInstanceOf<CustomPoint>(result);
            Assert.AreEqual(expectedPoint.X, result.X);
            Assert.AreEqual(expectedPoint.Y, result.Y);
        }

        [Test]
        public void GetEndPoint_WithTopDownVector_ReturnsPointWithY3800()
        {
            // Arrange
            var firstPoint = new CustomPoint(500, 500);
            var lastPoint = new CustomPoint(1000, 1000);
            var expectedPoint = new CustomPoint(3800, 3800);
            var customVector = new CustomVector(firstPoint);
            customVector.pointsOfVector.Add(lastPoint);
            customVector.SetEquation();

            // Act
            var result = customVector.GetEndPoint();

            // Assert
            Assert.IsInstanceOf<CustomPoint>(result);
            Assert.AreEqual(expectedPoint.X, result.X);
            Assert.AreEqual(expectedPoint.Y, result.Y);
        }
    }
}
