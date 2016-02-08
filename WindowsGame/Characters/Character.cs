using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace WindowsGame
{
    public enum CharacterBehaviours
    {
        kIdle,

        kNumBehaviours
    }

    public delegate void BehaviourChangeHandler(uint newState);

    public class Character : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// An event handler used with the StateMachine.
        /// Call EmitBehaviourChanged() when a character changes behaviour to update the State Machine for that character.
        /// </summary>
        public event BehaviourChangeHandler BehaviourChanged;

        /// <summary>
        /// A property to indicate the current behaviour that this character is in.
        /// </summary>
        protected uint CurrentBehaviour { get; set; }

        #endregion

        public Character(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            CurrentBehaviour = (uint)CharacterBehaviours.kIdle;
        }

        #region Behaviour Functions

        /// <summary>
        /// 
        /// </summary>
        public void EmitBehaviourChanged(uint oldState, uint newState)
        {
            // Behaviour Changed should not be null if we are calling this
            Debug.Assert(BehaviourChanged != null);
            BehaviourChanged();
        }

        #endregion
    }
}
