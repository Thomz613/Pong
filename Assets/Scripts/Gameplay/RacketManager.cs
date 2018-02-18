using UnityEngine;
using System.Collections;

/// <summary>
/// Class to handle the racket walls collisions using its controller.
/// The update of the racket position will be handled by the game manager.
/// The collisions with the ball will be handled by the ball manager.
/// </summary>
public class RacketManager : MonoBehaviour
{
    ControllerBase _racketController;
    private Vector3 _initialPosition;

    public ControllerBase RacketController
    {
        get { return _racketController; }
        private set { _racketController = value; }
    }

    public Vector3 InitialPosition
    {
        get { return _initialPosition; }
        private set
        {
            _initialPosition = value;
            _racketController.InitialPosition = value;
        }
    }

    void Awake()
    {
        _initialPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        _racketController.OnTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        _racketController.OnTriggerExit(other);
    }

    /// <summary>
    /// Set the controller used with this racket.
    /// </summary>
    /// <param name="controller">The controller used with this racket</param>
    public void SetController(ControllerBase controller)
    {
        _racketController = controller;
    }
}
