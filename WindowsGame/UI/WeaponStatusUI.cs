using _2DEngine;
using Microsoft.Xna.Framework;

namespace WindowsGame
{
    /// <summary>
    /// A piece of UI that shows the current information about our weapon - thumbnail, bullets left etc.
    /// </summary>
    public class WeaponStatusUI : UIContainer
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our current weapon
        /// </summary>
        private Weapon Weapon { get; set; }

        /// <summary>
        /// A reference to our current weapon's image
        /// </summary>
        private Image WeaponImage { get; set; }

        /// <summary>
        /// A reference to our current weapon's name label
        /// </summary>
        private Label WeaponNameLabel { get; set; }

        /// <summary>
        /// An array references to our bullet images used to show how many bullets are left in our magazine
        /// </summary>
        private Image[] BulletImages { get; set; }

        /// <summary>
        /// A flashing label to indicate that we are reloading
        /// </summary>
        private FlashingLabel ReloadingLabel { get; set; }

        private Vector2 padding = new Vector2(5);

        #endregion

        public WeaponStatusUI(Weapon weapon, string textureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(Vector2.Zero, textureAsset)
        {
            UsesCollider = false;

            DebugUtils.AssertNotNull(weapon);
            Weapon = weapon;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds all our UI for our weapon description
        /// </summary>
        public override void AddInitialUI()
        {
            base.AddInitialUI();

            DebugUtils.AssertNotNull(Weapon);
            DebugUtils.AssertNotNull(Weapon.WeaponData);

            WeaponImage = AddObject(new Image(new Vector2(300, 300), Vector2.Zero, Weapon.WeaponData.TextureAsset)) as Image;
            WeaponImage.Name = "Weapon Image";
            WeaponImage.UsesCollider = false;

            WeaponNameLabel = AddObject(new Label(Weapon.WeaponData.DisplayName, Vector2.Zero)) as Label;
            WeaponNameLabel.Name = "Weapon Name";

            BulletImages = new Image[Weapon.WeaponData.MagazineSize];
            for (int i = 0; i < Weapon.WeaponData.MagazineSize; i++)
            {
                // Fixup position later
                BulletImages[i] = AddObject(new Image(Vector2.Zero, Weapon.WeaponData.BulletThumbnailTextureAsset)) as Image;
                BulletImages[i].UsesCollider = false;
            }

            // Fixup position later
            ReloadingLabel = AddObject(new FlashingLabel(1.5f, "RELOADING", Vector2.Zero)) as FlashingLabel;
            ReloadingLabel.Colour = Color.Red;
            ReloadingLabel.Hide();
        }

        /// <summary>
        /// Fixup our UI so that they are spaced correctly
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            // Do fixup after initialisation - do this in begin instead?  Haven't added objects yet
            WeaponNameLabel.LocalPosition = new Vector2(0, -((WeaponImage.Size.Y + WeaponNameLabel.Size.Y) * 0.5f + padding.Y));

            // All the bullets will have the same size
            float xSpacing = BulletImages[0].Size.X + padding.X;

            // Fixup bullet images
            for (int i = 0; i < BulletImages.Length; i++)
            {
                float offset = i - (BulletImages.Length - 1) * 0.5f;
                BulletImages[i].LocalPosition = new Vector2(offset * xSpacing, (WeaponImage.Size.Y + BulletImages[i].Size.Y) * 0.5f + padding.Y);
            }

            ReloadingLabel.LocalPosition = new Vector2(0, (WeaponImage.Size.Y + ReloadingLabel.Size.Y) * 0.5f + padding.Y);

            // Fix up position and size too
            CalculateSize(padding);

            // Have this UI at the bottom left of our screen for now
            LocalPosition = (ScreenManager.Instance.ScreenDimensions * 0.9f - Size) * 0.5f;
        }

        /// <summary>
        /// Update our UI to match our weapon's current magazine size
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            for (int i = 0; i < BulletImages.Length; i++)
            {
                if (i < Weapon.CurrentBulletsInMagazine)
                {
                    BulletImages[i].Show();
                }
                else
                {
                    BulletImages[i].Hide();
                }
            }

            // Show the label only if we are reloading
            if (Weapon.CurrentWeaponState == WeaponState.kReloading)
            {
                ReloadingLabel.Show();
            }
            else
            {
                ReloadingLabel.Opacity = 1;
                ReloadingLabel.Hide();
            }
        }

        #endregion
    }
}
