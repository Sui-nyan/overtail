using Overtail.PlayerModule;
using UnityEngine;

namespace Overtail.Camera
{
    /// <summary>
    /// Default camera to follow player. Just attach this to main camera and either
    /// a) Assign player GameObject to target field or...
    /// b) Add "Player"-Tag to player GameObject
    /// </summary>
    public class PlayerCamera : BasicCamera
    {
        private void Awake()
        {
            DefaultTarget = GameObject.FindObjectOfType<Player>()?.gameObject
                            ?? GameObject.FindGameObjectWithTag("Player");
            if (DefaultTarget == null)
                UnityEngine.Debug.LogError("[Camera] setup failed. No GameObject of type <Player> found.");
        }
    }
}
