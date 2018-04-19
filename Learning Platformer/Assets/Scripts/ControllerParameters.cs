using UnityEngine;
using System.Collections;


[System.Serializable]   //modifiable from the inspector
public class ControllerParameter {
    
        public enum JumpBehaviour   //list of possible jump senarios
        {
            canJumpOnGround,
            canJumpAnywhere,
            cantJump
        }

        public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);   // MaxVeclocity gets set to the MaxValue by default

        [Range(0, 90)]
        public float SlopeLimit = 30;

        public float Gravity = -25f;    //default y value(Gravity) is set to -25f

        public JumpBehaviour JumpRestriction;   

        public float JumpFrequency = 0.25f; //limits how often the character can jump

        public float JumpMagnitude = 12;

}
