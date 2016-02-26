using Microsoft.Xna.Framework.Input;

namespace _2DEngine
{
    public static class InputMap
    {
        // Movement
        public static Keys MoveLeft = Keys.A;
        public static Keys MoveRight = Keys.D;
        public static Keys Run = Keys.LeftShift;

        // Gymnastics
        public static Keys Jump = Keys.Space;
        public static Keys ForwardRoll = Keys.LeftAlt;

        // Weapons
        public static MouseButton Shoot = MouseButton.kLeftButton;
        public static MouseButton Melee = MouseButton.kRightButton;

        // Camera
        public static Keys ZoomIn = Keys.Add;
        public static Keys ZoomOut = Keys.Subtract;
        public static Keys PanLeft = Keys.Left;
        public static Keys PanRight = Keys.Right;
        public static Keys PanUp = Keys.Up;
        public static Keys PanDown = Keys.Down;

        // Menu Navigation
        public static Keys BackToPreviousScreen = Keys.Escape;
    }
}