using System;
using _2DEngine;
using Microsoft.Xna.Framework;

namespace WindowsGame
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            AddGameObject(new Character(GetScreenCentre(), ""));
        }
    }
}
