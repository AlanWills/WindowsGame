using _2DEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace Test2DEngine
{
    [TestClass]
    public class TestMathUtils
    {
        [TestMethod]
        public void TestGenerateFloat()
        {
            float generatedFloat;

            int max = 10;
            for (int min = -10; min < 10; min++)
            {
                for (int i = 0; i < 10; i++)
                {
                    generatedFloat = MathUtils.GenerateFloat(min, max);
                    Assert.IsTrue(generatedFloat >= min && generatedFloat <= max);
                }
            }
        }

        [TestMethod]
        public void TestGenerateInt()
        {
            int generatedInt;

            int max = 15;
            for (int min = -10; min < 10; min++)
            {
                for (int i = 0; i < 10; i++)
                {
                    generatedInt = MathUtils.GenerateInt(min, max);
                    Assert.IsTrue(generatedInt >= min && generatedInt <= max);
                }
            }
        }

        [TestMethod]
        public void TestAngleBetweenPoints()
        {
            Vector2 origin = Vector2.Zero;

            Vector2 point3 = new Vector2(100, 0);
            float actualAngle = MathUtils.AngleBetweenPoints(origin, point3);
            float expectedAngle = MathHelper.PiOver2;
            Assert.AreEqual(expectedAngle, actualAngle);

            Vector2 point2 = new Vector2(0, 100);
            actualAngle = MathUtils.AngleBetweenPoints(origin, point2);
            expectedAngle = MathHelper.Pi;
            Assert.AreEqual(expectedAngle, actualAngle);

            Vector2 point4 = new Vector2(-100, 0);
            actualAngle = MathUtils.AngleBetweenPoints(origin, point4);
            expectedAngle = 1.5f * MathHelper.Pi;
            Assert.AreEqual(expectedAngle, actualAngle);

            Vector2 point5 = new Vector2(0, -100);
            actualAngle = MathUtils.AngleBetweenPoints(origin, point5);
            expectedAngle = 0;
            Assert.AreEqual(expectedAngle, actualAngle);
        }

        [TestMethod]
        public void TestCollisionFromAbove()
        {
            float collisionAngle = 0;
            bool expectedResult = true;
            bool actualResult = MathUtils.CheckCollisionFromAbove(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.PiOver2;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromAbove(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.Pi;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromAbove(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathUtils.ThreePiOver2;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromAbove(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestCollisionFromBelow()
        {
            float collisionAngle = 0;
            bool expectedResult = false;
            bool actualResult = MathUtils.CheckCollisionFromBelow(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.PiOver2;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromBelow(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.Pi;
            expectedResult = true;
            actualResult = MathUtils.CheckCollisionFromBelow(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathUtils.ThreePiOver2;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromBelow(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestCollisionFromLeft()
        {
            float collisionAngle = 0;
            bool expectedResult = false;
            bool actualResult = MathUtils.CheckCollisionFromLeft(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.PiOver2;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromLeft(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.Pi;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromLeft(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathUtils.ThreePiOver2;
            expectedResult = true;
            actualResult = MathUtils.CheckCollisionFromLeft(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestCollisionFromRight()
        {
            float collisionAngle = 0;
            bool expectedResult = false;
            bool actualResult = MathUtils.CheckCollisionFromRight(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.PiOver2;
            expectedResult = true;
            actualResult = MathUtils.CheckCollisionFromRight(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathHelper.Pi;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromRight(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);

            collisionAngle = MathUtils.ThreePiOver2;
            expectedResult = false;
            actualResult = MathUtils.CheckCollisionFromRight(collisionAngle);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
