using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A script class to run a set of commands within a screen.
    /// Scripts should not really draw, update or handle input for items, but instead add them to their appropriate screen's manager.
    /// This can always be done by caching the screen as a property.
    /// Scripts are really designed to perform custom input.
    /// Call Die for the Script to be completed and cleared up by the ScriptManager.
    /// </summary>
    public class Script : Component
    {
        #region Properties and Fields

        /// <summary>
        /// A flag to indicate whether we should allow the main game loops to run - Update and HandleInput
        /// </summary>
        public bool ShouldUpdateGame { get; set; }

        /// <summary>
        /// A reference to the parent screen - this will be useful for adding objects etc.
        /// </summary>
        public BaseScreen ParentScreen { get; set; }

        /// <summary>
        /// An optional parameter we can set to indicate that this script should not be able to run unless the previous script has finished.
        /// </summary>
        public Script PreviousScript { get; set; }

        /// <summary>
        /// A custom EventHandler for specifying a function to determine whether this script can run.
        /// The event must specify itself whether the script ShouldHandleInput and ShouldUpdate.
        /// </summary>
        public EventHandler CanRunEvent;

        #endregion

        /// <summary>
        /// Checks to see if the script can run and handle input.
        /// Will not call begin until the script can actually start running
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            CheckCanRun();

            // Check to see if we should update
            if (!ShouldUpdate) { return; }

            base.Update(elapsedGameTime);
        }

        /// <summary>
        /// No drawing in scripts
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // No drawing for scripts
            Debug.Fail("No drawing in scripts");
        }

        /// <summary>
        /// Utility Function to set ShouldHandleInput and ShouldUpdate based on certain conditions.
        /// Tests if the PreviousScript is completely, or based on a custom event, otherwise true.
        /// </summary>
        /// <returns></returns>
        public virtual void CheckCanRun()
        {
            if (PreviousScript != null)
            {
                ShouldHandleInput = !PreviousScript.IsAlive;
                ShouldUpdate = !PreviousScript.IsAlive;
            }
            else if (CanRunEvent != null)
            {
                CanRunEvent(this, EventArgs.Empty);
            }
            else
            {
                ShouldHandleInput = true;
                ShouldUpdate = true;
            }
        }
    }
}
