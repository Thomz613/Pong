using UnityEngine;
using System.Collections;
using System;

public class ControllerAI : ControllerBase
{
    #region Enums

    public enum Difficulty
    {
        Easy = 10,
        Normal = 15,
        Hard = 20
    }

    #endregion

    Transform _ball;


    public ControllerAI(int id, Difficulty difficulty, Transform ball)
        : base(id, ControllerType.AI, (float)difficulty)
    {
        _ball = ball;
    }

    /// <summary>
    /// AI behavior. The racket will follow the ball at a certain speed. Should be called each frame.
    /// </summary>
    /// <param name="racket">The transform of the racket</param>
    public override void ControlRacket(Transform racket)
    {
        // Compute the direction in which the racket should move
        // Positive is vertical upward
        // Negative is vertical downward
        int racketDirection = (int)DirectionOfMovement(racket);

        float translationValue = _speed * Time.deltaTime;

        if(racketDirection < 0f)
        {
            translationValue = -translationValue;
        }
        else if (racketDirection == 0f)
        {
            translationValue = 0f;
        }

        // Then move the racket if movement is allowed
        if(AllowMovement(translationValue))
        {
            Vector3 translationVector = new Vector3(0, 0, translationValue);
            racket.Translate(translationVector);
        }
    }

    /// <summary>
    /// AI Behavior: Compute the vector racket ball to find the vertical direction where the racket should move.
    /// </summary>
    /// <param name="racket">The racket</param>
    /// <returns></returns>
    public override float DirectionOfMovement(Transform racket)
    {
        return (_ball.position - racket.position).z;
    }
}
