using UnityEngine;
using System.Collections;

public class ControllerState {

    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }  //checks if player hit a ceiling
    public bool IsCollidingBelow { get; set; }  //checks if player is on the ground
    public bool IsMovingDownSlope { get; set; }
    public bool IsMovingUpSlope { get; set; }
    public bool IsGrounded { get { return IsCollidingBelow; } } 
    public float SlopeAngle { get; set; }

    public bool HasCollision { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }    //checks if colliding in any one of the four directions

    public void Reset() //resets controller state to default every frame
    {
        IsMovingUpSlope =
            IsMovingDownSlope =
            IsCollidingLeft =
            IsCollidingRight =
            IsCollidingAbove =
            IsCollidingBelow = false;

        SlopeAngle = 0;
    }
    
    public override string ToString() //used for debugging
    {
        return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope{4} up-slope: {5} angle: {6}",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingAbove,
            IsCollidingBelow,
            IsMovingDownSlope,
            IsMovingUpSlope,
            SlopeAngle);
    }
}
