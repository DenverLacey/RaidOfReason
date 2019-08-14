using UnityEngine;
using System.Collections;
// using UnityEditor; // Need this to access SerializedObject

public class ParticleModifier : MonoBehaviour
{

    // SerializedObject m_thisParticle; // This will be our modifiable particle system

    ParticleSystem.ShapeModule m_shape;

    bool m_isChanging; // Used as a flag for a coroutine

    //Set these values in the inspector. Will modify angle and radius of Particle System

    public float MAX_ANGLE, MIN_ANGLE, MAX_RADIUS, MIN_RADIUS, transitionSpeed;

    void Start()

    {

        /* Initialize and Assign thisParticle as a SerializedObject that takes properties

         * from the ParticleSystem attached to this game object. */

        //m_thisParticle = new SerializedObject(GetComponent<ParticleSystem>());

        //m_thisParticle.FindProperty("ShapeModule.radius").floatValue = MAX_RADIUS;

        //m_thisParticle.FindProperty("ShapeModule.angle").floatValue = MAX_ANGLE;

        //m_thisParticle.ApplyModifiedProperties(); // This basically updates the particles with any changes that have been made

        m_isChanging = false;

        m_shape = GetComponent<ParticleSystem>().shape;

    }

    void Update()

    {
        if (Input.GetKeyDown(KeyCode.R) && !m_isChanging)

            StartCoroutine(ChangeRadius());

        else if (Input.GetKeyDown(KeyCode.A) && !m_isChanging)

            StartCoroutine(ChangeAngle());

    }

    IEnumerator ChangeRadius()

    {

        m_isChanging = true; // set true so user can't spam the coroutine

        //This code will make the radius smaller if the radius is at its maximum already

        //if (m_thisParticle.FindProperty("ShapeModule.radius").floatValue >= MAX_RADIUS)

        //{

        //    while (m_thisParticle.FindProperty("ShapeModule.radius").floatValue > MIN_RADIUS)

        //    {

        //        //grab the radius value and subtract it

        //        m_thisParticle.FindProperty("ShapeModule.radius").floatValue -= Time.deltaTime * transitionSpeed;

        //        m_thisParticle.ApplyModifiedProperties(); // This is used to apply the new radius value

        //        yield return null;

        //    }

        //}

        if (m_shape.radius >= MAX_RADIUS)
        {
            while (m_shape.radius > MIN_RADIUS)
            {
                m_shape.radius -= Time.deltaTime * transitionSpeed;

                yield return null;
            }
        }

        //This code will make radius larger if radius is already at its minimum

        //else if (m_thisParticle.FindProperty("ShapeModule.radius").floatValue <= MIN_RADIUS)

        //{

        //    while (m_thisParticle.FindProperty("ShapeModule.radius").floatValue < MAX_RADIUS)

        //    {

        //        //grab the radius variable and increase it

        //        m_thisParticle.FindProperty("ShapeModule.radius").floatValue += Time.deltaTime * transitionSpeed;

        //        m_thisParticle.ApplyModifiedProperties(); // Apply new radius value

        //        yield return null;

        //    }

        //}

        else if (m_shape.radius <= MIN_RADIUS)
        {
            while (m_shape.radius < MAX_RADIUS)
            {
                m_shape.radius += Time.deltaTime * transitionSpeed;
                yield return null;
            }
        }

        m_isChanging = false; // set to false so user can input again.

        yield return null;

    }

    IEnumerator ChangeAngle()

    {

        m_isChanging = true;

        //This code will make the angle smaller if the angle is at its maximum already

        //if (m_thisParticle.FindProperty("ShapeModule.angle").floatValue >= MAX_ANGLE)

        //{

        //    while (m_thisParticle.FindProperty("ShapeModule.angle").floatValue > MIN_ANGLE)

        //    {

        //        //grab angle value and subtract it

        //        m_thisParticle.FindProperty("ShapeModule.angle").floatValue -= Time.deltaTime * (transitionSpeed * 2);

        //        m_thisParticle.ApplyModifiedProperties(); // apply new value to angle

        //        yield return null;

        //    }

        //}

        if (m_shape.angle >= MAX_ANGLE)
        {
            while (m_shape.angle > MIN_ANGLE)
            {
                m_shape.angle -= Time.deltaTime * (transitionSpeed * 2);
                yield return null;
            }
        }

        //This code will make angle larger if angle is already at its minimum

        //else if (m_thisParticle.FindProperty("ShapeModule.angle").floatValue <= MIN_ANGLE)

        //{

        //    while (m_thisParticle.FindProperty("ShapeModule.angle").floatValue < MAX_ANGLE)

        //    {

        //        // grab angle value and increase it

        //        m_thisParticle.FindProperty("ShapeModule.angle").floatValue += Time.deltaTime * (transitionSpeed * 2);

        //        m_thisParticle.ApplyModifiedProperties(); // apply new value to angle

        //        yield return null;

        //    }

        //}

        else if (m_shape.angle <= MIN_ANGLE)
        {
            while (m_shape.angle < MAX_ANGLE)
            {
                m_shape.angle += Time.deltaTime * (transitionSpeed * 2);
                yield return null;
            }
        }

        m_isChanging = false;

        yield return null;

    }

}