using _2DEngine;
using _2DEngineData;
using GameData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame
{
    public class ArmedCharacter : Character
    {
        #region Properties and Fields

        /// <summary>
        /// This character's weapon.
        /// </summary>
        protected Weapon Weapon { get; private set; }

        #endregion

        public ArmedCharacter(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Loads this objects data as ArmedCharacterData
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            CharacterData = AssetManager.GetData<ArmedCharacterData>(DataAsset);
            return CharacterData;
        }

        /// <summary>
        /// Create our Weapon and load it
        /// </summary>
        public override void LoadContent()
        {
            ArmedCharacterData data = Data as ArmedCharacterData;
            DebugUtils.AssertNotNull(data);

            Weapon = new Weapon(this, data.WeaponDataAsset);
            Weapon.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialise our weapon
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            DebugUtils.AssertNotNull(Weapon);
            Weapon.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Handle input for our weapon and our character.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            Weapon.HandleInput(elapsedGameTime, mousePosition);
        }

        /// <summary>
        /// Update our weapon and our character.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Weapon.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draw the bullets in our weapon and our character.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Weapon.Draw(spriteBatch);
        }

        #endregion
    }
}
