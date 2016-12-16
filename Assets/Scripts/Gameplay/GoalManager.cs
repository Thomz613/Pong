using UnityEngine;
using System.Collections;

/// <summary>
/// Handles ball goal collisions
/// </summary>
public class GoalManager : MonoBehaviour
{
    int _id;

    public int Id
    {
        get { return _id; }
        private set { _id = value; }
    }

    public void SetId(int newId)
    {
        _id = newId;
    }

    void OnTriggerEnter(Collider other)
    {
        // If it's a ball collision. The Game Manager should be informed a player scored.
        // TODO
        if (other.CompareTag("Ball"))
        {
            GameManager.Instance.PlayerScored(_id);
        }
    }
}
