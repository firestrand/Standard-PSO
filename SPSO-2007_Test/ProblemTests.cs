using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPSO_2007;

namespace SPSO_2007_Test
{
    [TestClass]
    public class ProblemTests
    {
        [TestMethod]
        public void TripodOptimumIsCorrect()
        {
            Position position = new Position(2);
            position.size = 2;
            position.x = new[] {0.0, -50.0};

            double actual = Problem.perf(position, 4, 0.0);
            double expected = 0.0;
            Assert.AreEqual(expected,actual);
        }
        [TestMethod]
        public void GearTrainOptimumIsCorrect()
        {
            Position position = new Position();
            position.size = 4;
            position.x = new double[] {19, 16, 43, 49};

            double actual = Math.Round(Problem.perf(position, 18, 0.0),14);
            double expected = 2.7e-12;
            Assert.AreEqual(expected,actual);
        }
    }
}
