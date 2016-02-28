using System.Diagnostics;

namespace _2DEngine
{
    public class Property<T>
    {
        public delegate void ComputeFunctionEvent(Property<T> property, Property<T> parentProperty);

        // Expand on this
        // Having checking for connectedness when setting
        // Compute sets
        // Keep track of input attributes and output attributes?

        /// <summary>
        /// An optional parent who's value we will use for this Property's value
        /// </summary>
        private Property<T> Parent { get; set; }

        /// <summary>
        /// Optional event handler which can be used to perform a compute on this property and it's parent's values.
        /// Must be connected to a property to do this.
        /// </summary>
        public event ComputeFunctionEvent ComputeFunction;

        /// <summary>
        /// Returns whether this property is driven by another property.
        /// </summary>
        public bool IsOutput
        {
            get { return Parent != null; }
        }

        /// <summary>
        /// The value of this Property
        /// </summary>
        private T value;
        public T Value
        {
            get
            {
                if (Parent != null)
                {
                    if (ComputeFunction != null)
                    {
                        DebugUtils.AssertNotNull(Parent);
                    }

                    return Parent.Value;
                }

                return value;
            }
            set
            {
                if (Parent != null)
                {
                    Debug.Fail("Attempting to set a property which has an input connection.");
                    return;
                }

                this.value = value;
            }
        }

        public Property(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Connect this property to another so that this is driven by the connected property
        /// </summary>
        /// <param name="property">The Property to use to drive this Property</param>
        public void Connect(Property<T> property)
        {
            Parent = property;
        }

        /// <summary>
        /// Disconnects this Property from it's Parent so that it is free to be manually set.
        /// </summary>
        /// <param name="useParentValueBeforeDisconnecting">If true we set this property's value to it's parent before we disconnect.</param>
        public void Disconnect(bool useParentValueBeforeDisconnecting = true)
        {
            DebugUtils.AssertNotNull(Parent);

            T value = Parent.Value;
            Parent = null;

            if (useParentValueBeforeDisconnecting)
            {
                Value = value;
            }
            else
            {
                Value = default(T);
            }
        }
    }
}