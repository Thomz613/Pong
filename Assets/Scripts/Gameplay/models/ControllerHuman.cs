using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Class to handle human racket behavior
/// </summary>
public class ControllerHuman : ControllerBase
{
    /// <summary>
    /// Used with the Input manager to find the right axes to use.
    /// </summary>
    static string PlayerSuffix = "_P";

    public ControllerHuman(int id, ControllerType deviceType, float speed)
        : base(id, deviceType, speed)
    {
    }

    /// <summary>
    /// Control the racket (using the input manager). Should be called each frame.
    /// </summary>
    /// <param name="racket">The racket</param>
    public override void ControlRacket(Transform racket)
    {

        float translationValue = DirectionOfMovement(racket) * _speed * Time.deltaTime;

        // Move the racket only if movement is allowed
        if(AllowMovement(translationValue))
        {
            Vector3 translationVector = new Vector3(0, 0, translationValue);

            racket.Translate(translationVector);
        }
    }

    /// <summary>
    /// Returns the value of the Input Manager's vertical axis polled
    /// </summary>
    /// <param name="racket">This parameter is actually not used here and can be null</param>
    /// <returns>The value found</returns>
    public override float DirectionOfMovement(Transform racket)
    {
        string verticalAxis = "Vertical" + PlayerSuffix + _id;
        return Input.GetAxis(verticalAxis);
    }
}
