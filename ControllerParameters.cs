using UnityEngine;
using System.Collections;

[System.Serializable]
public class ControllerParameter {

        public enum JumpBehaviour
        {
            canJumpOnGround,
            canJumpAnywhere,
            cantJump
        }

        public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);

        [Range(0, 90)]
        public float SlopeLimit = 30;

        public float Gravity = -25f;

        public JumpBehaviour JumpRestriction;

        public float JumpFrequency = 0.25f;

        public float JumpMagnitude = 12;

}
