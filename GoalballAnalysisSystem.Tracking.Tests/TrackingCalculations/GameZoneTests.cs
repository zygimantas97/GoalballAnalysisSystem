using GoalballAnalysisSystem.Tracking;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GoalballAnalysisSystem.Tracking.Tests.TrackingCalculations
{
    [TestFixture]
    public class GameZoneTests
    { 

        public GameZone createGameZoneAscending()
        {
            List<CustomPoint> pointsList = new List<CustomPoint>();
            pointsList.Add(new CustomPoint(0, 0));
            pointsList.Add(new CustomPoint(1, 1));
            pointsList.Add(new CustomPoint(2, 2));
            pointsList.Add(new CustomPoint(3, 3));
            return new GameZone(pointsList);
        }

        public GameZone createGameZoneDecending()
        {
            List<CustomPoint> pointsList = new List<CustomPoint>();
            pointsList.Add(new CustomPoint(3, 3));
            pointsList.Add(new CustomPoint(2, 2));
            pointsList.Add(new CustomPoint(1, 1));
            pointsList.Add(new CustomPoint(0, 0));
            return new GameZone(pointsList);
        }

        public List<CustomPoint> AddedPointsDecending()
        {
            List<CustomPoint> pointsList = new List<CustomPoint>();
            pointsList.Add(new CustomPoint(3, 3));
            pointsList.Add(new CustomPoint(2, 2));
            pointsList.Add(new CustomPoint(1, 1));
            pointsList.Add(new CustomPoint(0, 0));
            return pointsList;
        }

        public List<CustomPoint> AddedPointsAscending()
        {
            List<CustomPoint> pointsList = new List<CustomPoint>();
            pointsList.Add(new CustomPoint(0, 0));
            pointsList.Add(new CustomPoint(1, 1));
            pointsList.Add(new CustomPoint(2, 2));
            pointsList.Add(new CustomPoint(3, 3));
            return pointsList;
        }


        [Test]
        public void GetLengthOfLine_WithPositiveValues_ReturnsADistance()
        {
            // Arrange
            var gameZone = createGameZoneDecending();

            // Act
            var result = gameZone.GetLengthOfLine(
                new CustomPoint(4, 3),
                new CustomPoint(8, 6));
            var actualResult = 5;

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void GetLengthOfLine_WithZeroValues_ReturnsADistance()
        {
            // Arrange
            var gameZone = createGameZoneDecending();

            // Act
            var result = gameZone.GetLengthOfLine(
                new CustomPoint(0, 0),
                new CustomPoint(0, 0));
            var actualResult = 0;

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void GetPoints_WithTheFourInitialisedPoints_ReturnsAListOfPoints()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            var actualResult = AddedPointsAscending();

            // Act
            var result = gameZone.GetPoints();

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void videoOXY2gameZoneOXY_WithCustomPointAndRotationFalse_ReturnsACustomPoint()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(10, 10);
            bool rotation = false;
            CustomPoint actualResult = new CustomPoint(15400, -2147479848);

            // Act
            var result = gameZone.videoOXY2gameZoneOXY(point, rotation);

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void videoOXY2gameZoneOXY_WithCustomPointAndRotationTrue_ReturnsACustomPoint()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(10, 10);
            bool rotation = true;
            CustomPoint actualResult = new CustomPoint(22600, -2147479848);

            // Act
            var result = gameZone.videoOXY2gameZoneOXY(point, rotation);

            // Assert
            Assert.AreEqual(actualResult.Y, result.Y);
        }

        [Test]
        public void RotatePoint_WithPositiveCoordinates_ReturnsACustomPoint()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(20, 80);
            CustomPoint actualResult = new CustomPoint(69, 40);

            // Act
            var result = gameZone.RotatePoint(point);

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void RotatePoint_WithZeroCoordinates_ReturnsACustomPoint()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(0, 0);
            CustomPoint actualResult = new CustomPoint(0, 0);

            // Act
            var result = gameZone.RotatePoint(point);

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void RotatePoint_WithNegativeCoordinates_ReturnsACustomPoint()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(-10, -10);
            CustomPoint actualResult = new CustomPoint(-14, 2);

            // Act
            var result = gameZone.RotatePoint(point);

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void IsPointSuitable_WithPositiveSuitablePoint_RetursIfPointSuitable()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(10, 10);
            var actualResult = true;

            // Act
            var result = gameZone.IsPointSuitable(point);

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void IsPointSuitable_WithPositiveAndNegativeNotSuitablePoint_RetursIfPointSuitable()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(-100000, 100000);
            var actualResult = false;

            // Act
            var result = gameZone.IsPointSuitable(point);

            // Assert
            Assert.AreEqual(actualResult, result);
        }

        [Test]
        public void AddPointToVectors_WhenRemainingVectorIsEmpty_ExpectedBehavior()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(150, 200);
            int expectedResult = 0;

            // Act
            gameZone.AddPointToVectors(point);

            // Assert
            Assert.AreEqual(expectedResult, gameZone.remainingVectors.Count);
        }

        [Test]
        public void AddPointToVectors_WhenRemainingVectorIsNotEmpty_ExpectedBehavior()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            CustomPoint point = new CustomPoint(150, 200);
            int expectedResult = 0;

            // Act
            gameZone.AddPointToVectors(point);

            // Assert
            Assert.AreEqual(expectedResult, gameZone.remainingVectors.Count);
        }

        [Test]
        public void GetZone_WhenPointIsZero_ReturnsPointZone()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            int X = 0;
            int expectedResult = 0;

            // Act
            var result = gameZone.GetZone(X);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetZone_WhenPointIsPositive_ReturnsPointZone()
        {
            // Arrange
            var gameZone = createGameZoneAscending();
            int X = 10;
            int expectedResult = 1;

            // Act
            var result = gameZone.GetZone(X);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetZone_WhenPointIsPositiveBig_ReturnsPointZone()
        {
            // Arrange
            var gameZone = createGameZoneAscending();

            int X = 10000;
            int expectedResult = 8;

            // Act
            var result = gameZone.GetZone(X);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetZone_WhenPointIsNegative_ReturnsPointZone()
        {
            // Arrange
            var gameZone = createGameZoneAscending();

            int X = -10;
            int expectedResult = 0;

            // Act
            var result = gameZone.GetZone(X);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
