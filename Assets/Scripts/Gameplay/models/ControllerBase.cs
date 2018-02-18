using UnityEngine;
using System.Collections;

/// <summary>
/// Base class to handle racket behavior (i.e. how to control the racket)
/// </summary>
public abstract class ControllerBase
{
    static string LowerWallTag = "Lower Wall";
    static string UpperWallTag = "Upper Wall";

    #region Enums

    public enum ControllerType
    {
        AI = -1,
        Human
    }

    #endregion

    protected int _id;  // Used only by players to use the right axes in the input manager
    protected ControllerType _deviceType;
    protected float _speed;
	protected Vector3 _initialPosition;

    protected bool _canMoveDown;
    protected bool _canMoveUp;

    #region Properties

    public int Id
    {
        get { return _id; }
        protected set { _id = value; }
    }

    public ControllerType DeviceType
    {
        get { return _deviceType; }
        protected set { _deviceType = value; }
    }

    public float Speed
    {
        get { return _speed; }
        protected set { _speed = value; }
    }
	
	public Vector3 InitialPosition
	{
		get { return _initialPosition; }
		set { _initialPosition = value; }
	}

    #endregion

    public ControllerBase(int id, ControllerType deviceType, float speed, Vector3 initialPosition)
    {
        _id = id;
        _deviceType = deviceType;
        _speed = speed;
		_initialPosition = initialPosition;
		
        _canMoveDown = true;
        _canMoveUp = true;
    }

    /// <summary>
    /// Check the value returned by polling an input manager's axis to allow moving the racket vertically
    /// </summary>
    /// <param name="axisPollValue">The value of the polled axis</param>
    /// <returns>True if not blocked by a wall, no either.</returns>
    protected bool AllowMovement(float axisPollValue)
    {
        if (axisPollValue < 0f && _canMoveDown)
        {
            return true;
        }
        else if(axisPollValue > 0f && _canMoveUp)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Handles and control the racket. Should be called each frame.
    /// </summary>
    /// <param name="racket">The racket</param>
    public abstract void ControlRacket(Transform racket);

    /// <summary>
    /// Compute the vertical direction where the racket is moving
    /// </summary>
    /// <param name="racket">The racket</param>
    /// <returns></returns>
    public abstract float DirectionOfMovement(Transform racket);
	
	/// <summary>
    /// Reset the racket position to its initial value
    /// </summary>
	public abstract void ResetRacketPosition(Transform racket);

    public virtual void OnTriggerEnter(Collider other)
    {
        // The racket should never be able to move through the walls
        if(other.CompareTag(LowerWallTag))
        {
            _canMoveDown = false;
        }
        else if(other.CompareTag(UpperWallTag))
        {
            _canMoveUp = false;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        // The racket should never be able to move through the walls
        if (other.CompareTag(LowerWallTag))
        {
            _canMoveDown = true;
        }
        if(other.CompareTag(UpperWallTag))
        {
            _canMoveUp = true;
        }
    }
}
