using _2DEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace Test2DEngine
{
    [TestClass]
    public class TestProperty
    {
        [TestMethod]
        public void TestPropertyCreation()
        {
            BaseObject expectedBaseObjectValue = null;
            Property<BaseObject> baseObjectProperty = new Property<BaseObject>(null);
            Assert.AreEqual(expectedBaseObjectValue, baseObjectProperty.Value);

            float expectedFloatValue = 10;
            Property<float> floatProperty = new Property<float>(10);
            Assert.AreEqual(expectedFloatValue, floatProperty.Value);

            Vector2 expectedVector2Value = new Vector2(-10, 10);
            Property<Vector2> vector2Property = new Property<Vector2>(new Vector2(-10, 10));
            Assert.AreEqual(expectedVector2Value, vector2Property.Value);
        }

        [TestMethod]
        public void TestFloatPropertyConnection()
        {
            TestPropertyConnection(10, -10);
        }

        [TestMethod]
        public void TestVector2PropertyConnection()
        {
            TestPropertyConnection(new Vector2(10, -10), new Vector2(-20, 20));
        }

        [TestMethod]
        public void TestBoolPropertyConnection()
        {
            TestPropertyConnection(true, false);
        }

        [TestMethod]
        public void TestColourPropertyConnection()
        {
            TestPropertyConnection(Color.Blue, Color.Red);
        }

        private void TestPropertyConnection<T>(T value, T newValue)
        {
            Assert.AreNotEqual(value, default(T));

            T expectedValue = value;
            Property<T> parentProperty = new Property<T>(expectedValue);
            Property<T> childProperty = new Property<T>(default(T));
            Assert.AreNotEqual(expectedValue, childProperty.Value);

            childProperty.Connect(parentProperty);
            Assert.AreEqual(expectedValue, childProperty.Value);

            T newExpectedValue = newValue;
            parentProperty.Value = newExpectedValue;
            Assert.AreEqual(newExpectedValue, parentProperty.Value);
            Assert.AreEqual(newExpectedValue, childProperty.Value);

            Assert.IsTrue(childProperty.IsOutput);
        }

        [TestMethod]
        public void TestVector2ComputeFunction()
        {
            Vector2 expectedValue = new Vector2(500, 500);
            Property<Vector2> parentProperty = new Property<Vector2>(new Vector2(1000, 1000));
            Property<Vector2> childProperty = new Property<Vector2>();
            childProperty.Connect(parentProperty);
            childProperty.ComputeFunction += TestVector2ComputeFunction;

            Assert.AreEqual(expectedValue, childProperty.Value);
        }

        private Vector2 TestVector2ComputeFunction(Property<Vector2> property, Property<Vector2> parentProperty)
        {
            return parentProperty.Value - new Vector2(500, 500);
        }
    }
}