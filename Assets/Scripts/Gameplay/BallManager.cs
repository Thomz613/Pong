using UnityEngine;
using System.Collections;

/// <summary>
/// Class to handle the ball 
/// </summary>
public class BallManager : MonoBehaviour
{
    float _initialSpeed;
    float _speed;
    Vector3 _direction;


    void OnTriggerEnter(Collider other)
    {
        ResolveCollision(other);
    }


    /// <summary>
    /// Handle the ball racket collision by computing its new bounce direction.
    /// </summary>
    /// <param name="racket">The racket</param>
    void BounceOnRacket(Collider racket)
    {
        /*
         * COmpute the resulting z axis bounce direction.
         * Bouncing on the edges will send back the ball diagonally.
         * Bouncing on the center will send back the ball horizontally.
         */

        // Vector ball racket
        Vector3 racketBallPosition = (transform.position - racket.transform.position);
        // Bounce factor
        float ballOnRacketZ = racketBallPosition.z / racket.bounds.size.z;
        _direction.z = ballOnRacketZ;

        // Reverse horizontal direction
        _direction.x = -_direction.x;
        _direction = _direction.normalized;
    }

    /// <summary>
    /// Handle the ball wall collision by inverting its z direction.
    /// </summary>
    void BounceOnWall()
    {
        _direction.z = -_direction.z;
    }

    /// <summary>
    /// Handle a goal event.
    /// </summary>
    void GoalScored()
    {
        _speed = 0f;
    }

    /// <summary>
    /// Compute a random direction using the ball's relative position and orientation.
    /// </summary>
    /// <param name="angle">Angle value used to clamp the generator to [-angle, angle[</param>
    /// <param name="BallPosition">The ball position used to compute the random direction</param>
    /// <returns>The computed random direction</returns>
    public static Vector3 RandomDirection(float angle, Transform BallPosition)
    {
        float randomAngle = Random.Range(-angle, angle);

        // The rotation is computed on the Y axis to stay in a 2D XZ plane
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, BallPosition.up);
        // Apply rotation to x axis
        Vector3 randomDir = rotation * BallPosition.right;

        return randomDir.normalized;
    }

    /// <summary>
    /// Handle the collisions of the ball
    /// </summary>
    /// <param name="other">The collider on which the ball collided</param>
    void ResolveCollision(Collider other)
    {
        // TODO: Glitch? No "else if" to handle multiple collisions ?
        if (other.CompareTag("Upper Wall") || other.CompareTag("Lower Wall"))
        {
            BounceOnWall();
            // TODO: Play SFX
        }
        if (other.CompareTag("Racket"))
        {
            BounceOnRacket(other);
            // TODO: Play SFX
        }
        if (other.CompareTag("Goal"))
        {
            GoalScored();
            // TODO: Play SFX
        }
    }

    /// <summary>
    /// Serve (i.e. launch the ball to start a new round)
    /// </summary>
    /// <param name="servePoint">The position from the ball should be "launched"</param>
    /// <param name="direction">The direction towards the ball should be "launched"</param>
    public void Serve(Vector3 servePoint, Vector3 direction)
    {
        // Put the ball in the middle of the screen
        transform.position = servePoint;
        // Normalize the direction to avoid speed issues
        _direction = direction.normalized;
        // Serve by setting the speed
        _speed = _initialSpeed;
    }

    /// <summary>
    /// Set ball's properties
    /// </summary>
    /// <param name="speed">the new speed of the ball</param>
    public void SetBall(float speed)
    {
        _initialSpeed = speed;
        _speed = 0f;
        _direction = Vector3.zero;
    }

    /// <summary>
    /// Updates the ball position by translating it. Should be called each frame.
    /// </summary>
    public void UpdateBallPosition()
    {
        float speedNormalized = _speed * Time.deltaTime;
        Vector3 translationValue = speedNormalized * _direction;

        transform.Translate(translationValue);
    }
}
