using UnityEngine;
using System.Collections;

/// <summary>
/// Class to handle the racket walls collisions using its controller.
/// The update of the racket position will be handled by the game manager.
/// The collisions with the ball will be handled by the ball manager.
/// </summary>
public class RacketManager : MonoBehaviour {

    ControllerBase _racketController;

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
