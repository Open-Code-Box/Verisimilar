using UnityEngine;

namespace TTTSC_Character_Controller_V2.Core.Scripts
{
    public class CharacterFST : MonoBehaviour
    {
        public CharacterState characterState; 
        public MovementType movementType;
        public bool ceilingDetected;
        public bool eligibleForStep;
        [Header("Ladder bools")]
        public bool onLadder;
        public bool topOnLadder;
        public bool bottomOnLadder;
        public bool enteredLadderFromBottom;
        public bool enteredLadderFromTop;

        [HideInInspector]
        public Transform topLadder;
        [HideInInspector]
        public Transform bottomLadder;
        public enum CharacterState
        {
            OnGround,
            InAir,
            InWater
        }

        public enum MovementType
        {
            Walk,
            Crouch,
            Sprint
        }

        public enum ActionState
        {
            InCar,
            InBoat,
            InProp
        }
    }
}
