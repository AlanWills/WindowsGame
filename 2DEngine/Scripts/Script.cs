using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A delegate use for a function which will be called to test whether the current script can run
    /// </summary>
    public delegate void CanScriptRun();

    /// <summary>
    /// A delegate used for a function which will be called when the script dies
    /// </summary>
    public delegate void OnScriptDeath();

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
        /// A reference to the parent screen - this will be useful for adding objects etc.
        /// </summary>
        public BaseScreen ParentScreen { get; set; }

        /// <summary>
        /// An optional parameter we can set to indicate that this script should not be able to run unless the previous script has finished.
        /// </summary>
        public Script PreviousScript { get; set; }

        /// <summary>
        /// An event handler for specifying a function to determine whether this script can run.
        /// The event must specify itself whether the script ShouldHandleInput and ShouldUpdate.
        /// </summary>
        public event CanScriptRun CanRunEvent;

        /// <summary>
        /// An event handler used to manage a function which will be called when the script is completed.
        /// Useful for resetting the game state for example.
        /// </summary>
        public event OnScriptDeath OnDeathCallback;

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Checks to see if the script can run and handle input.
        /// Will not call begin until the script can actually start running
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            CheckCanRun();

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
        protected virtual void CheckCanRun()
        {
            if (ScreenManager.Instance.CurrentScreen != ParentScreen)
            {
                ShouldHandleInput.Value = false;
                ShouldUpdate.Value = false;
            }
            else if (PreviousScript != null)
            {
                ShouldHandleInput.Value = !PreviousScript.IsAlive.Value;
                ShouldUpdate.Value = !PreviousScript.IsAlive.Value;
            }
            else if (CanRunEvent != null)
            {
                CanRunEvent();
            }
            else
            {
                ShouldHandleInput.Value = true;
                ShouldUpdate.Value = true;
            }
        }

        /// <summary>
        /// Indicates that the script has finished and calls the OnDeathCallback event.
        /// </summary>
        public override void Die()
        {
            base.Die();

            if (OnDeathCallback != null)
            {
                OnDeathCallback();
            }
        }

        #endregion
    }
}
