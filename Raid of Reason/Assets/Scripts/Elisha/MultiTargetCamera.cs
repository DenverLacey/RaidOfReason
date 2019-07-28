using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Elisha_Anagnostakis
 * Description: Handles how the camera operates within the game by making sure our main characters are all within the screen 
 *              at once, by adjusting its zoom and position based on how many players there are.
 */

// Requires camera component or else script will throw an error.
[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
    public float m_smoothTime;
    public float m_FOVMin;
    public float m_FOVMax;
    public float m_sizeNormaliser;
    public Vector3 m_offset;
    public List<Transform> m_targets;
    public Camera m_cam;

    private Vector3 m_moveVelocity;
    private float m_zoomVelocity;
    private float m_FOVScalar = 1f;

    void Awake()
    {
        m_cam = GetComponent<Camera>();
    }

    /// <summary>
    /// Physics update.
    /// </summary>
    void FixedUpdate()
    {
        m_targets.RemoveAll(target => !target);

        // If all players are dead from the array dont do anything.
        if (m_targets.Count == 0) { return; }

        var bounds = new Bounds(m_targets[0].position, Vector3.zero);

        Move(ref bounds);
        Zoom(bounds);
    }

    /// <summary>
    /// Moves the cameras transform accordingly.
    /// </summary>
    /// <param name="bounds"></param>
    void Move(ref Bounds bounds)
    {
        // Gets the centre point of the players.
        Vector3 centrePoint = GetCentrePoint(ref bounds);
        // Finds the new position of the players based off the offset.
        Vector3 newPos = centrePoint + m_offset;
        // Move camera to that position.
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref m_moveVelocity, m_smoothTime);
    }

    /// <summary>
    /// This function allows the camera to adjust to the centre point based on where the players are.
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns> Vector 3 value. </returns>
    Vector3 GetCentrePoint(ref Bounds bounds) {

        // Checks if theres only 1 player in the array of targets.
        if (m_targets.Count == 1) {
            // If so then return the position of the target left.
            return m_targets[0].position;
        }

        // For every transform in the array of targets.
        foreach (Transform t in m_targets) {
            // Grow the bounds of that targets position.
            bounds.Encapsulate(t.position);
        }

        if (m_targets[0] == null)
        {
            bounds.Encapsulate(m_targets[1].position);
        }

        return bounds.center;
    }

    /// <summary>
    /// This function zooms into the action based on whether 
    /// the players are exiting the screen space or not.
    /// </summary>
    /// <param name="bounds"></param>
    void Zoom(Bounds bounds) {

        // Checks if theres more than 1 player on the screen.
        if (m_targets.Count != 1) {
            // Scale according to the players on the screen.
            m_FOVScalar = Mathf.SmoothDamp(m_FOVScalar, bounds.size.magnitude / m_sizeNormaliser, ref m_zoomVelocity, m_smoothTime);
        }
        else {
            // Scale to only fit the 1 player thats alive of a bounds of 0.5 floats.
            m_FOVScalar = Mathf.SmoothDamp(m_FOVScalar, .5f, ref m_zoomVelocity, m_smoothTime);
        }
        // Lerps the cameras FOV according to the min and max thats been inputted in the inspector.
        m_cam.fieldOfView = Mathf.Lerp(m_FOVMin, m_FOVMax, m_FOVScalar);
    }
}