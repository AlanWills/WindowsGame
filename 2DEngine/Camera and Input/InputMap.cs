using Microsoft.Xna.Framework.Input;

namespace _2DEngine
{
    public static class InputMap
    {
        // Movement
        public static Keys MoveLeft = Keys.A;
        public static Keys MoveRight = Keys.D;

        // Gymnastics
        public static Keys Jump = Keys.Space;
        public static Keys ForwardRoll = Keys.E;

        // Weapons
        public static MouseButton Shoot = MouseButton.kLeftButton;
        public static MouseButton Melee = MouseButton.kRightButton;
    }
}