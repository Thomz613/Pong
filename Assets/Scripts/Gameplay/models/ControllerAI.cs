using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Simple AI script. Will follow the ball at a fixed speed and distance
/// </summary>
public class ControllerAI : ControllerBase
{
    /// <summary>
    /// This coefficient is used to compute a ratio of the distance between the two rackets.
    /// Used to let the racket not always follow the ball.
    /// Its value should stay between 0 - 1.
    /// </summary>
    static float MaxDistanceCoefficient = 0.75f;
    #region Enums

    public enum Difficulty
    {
        Easy = 11,
        Normal = 12,
        Hard = 15
    }

    #endregion

    Transform _ball;
    float _maxTrackingDistance;


    public ControllerAI(int id, Difficulty difficulty, float racketsDistance, Vector3 initialPosition, Transform ball)
        : base(id, ControllerType.AI, (float)difficulty, initialPosition)
    {
        _ball = ball;
        _maxTrackingDistance = racketsDistance * MaxDistanceCoefficient;
    }

    /// <summary>
    /// Check if the ball in in range of the racket (i.e. in the max tracking distance range).
    /// </summary>
    /// <param name="racket">The racket</param>
    /// <returns>True if in range, else either</returns>
    bool BallInRange(Transform racket)
    {
        float ballDistance = _ball.position.x - racket.position.x;
        return Mathf.Abs(ballDistance) < _maxTrackingDistance;
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
        if(AllowMovement(translationValue) && BallInRange(racket))
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
	
	public override void ResetRacketPosition(Transform racket)
	{
		racket.position = _initialPosition;
	}
}
