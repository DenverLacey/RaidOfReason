using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
 * Author: Afridi Rahim
 *
 *  Summary:
 *  This Script is used for Kenrons Final ability that spawns an ethereal version of himself
*/

public class ChildKenron :  BaseCharacter
{
    [Tooltip("The Aethereal Kenron Spawned after Death")]
    //This Current Instance
    public ChildKenron Child;

    [SerializeField]
    [Tooltip("The Delay until the next Dash")]
    private float m_dashTime;

    [SerializeField]
    [Tooltip("Distance of Dash Attack")]
    private float m_maxDashDistance;

    [SerializeField]
    [Tooltip("How quickly Kenron dashes")]
    private float m_dashSpeed;

    [SerializeField]
    [Tooltip("Hit box for dash attack")]
    private BoxCollider m_dashCollider;

    [Tooltip("The Collider of Kenrons Sword")]
    public Collider swordCollider;

    [Tooltip("The Sword Kenron Is Using")]
    public GameObject Amaterasu;

    // Desired position to dash
    private Vector3 m_dashPosition;

    // A bool that checks if nashorn has give kenron his buff
    public bool nashornBuffGiven = false;

    // Checks if Kenron is Dashing or Not
    private bool isDashing;

    // Checks if a specific trigger on the controller is pressed
    private bool m_triggerDown;

    // Kenrons Rigid Body
    private Rigidbody m_childRigidBody;

    protected void Start()
    {
        m_childRigidBody = GetComponent<Rigidbody>();
        // set size of dash hit box
        Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, m_maxDashDistance);
        m_dashCollider.size = hitBoxSize;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
        // Right now this is a timer, but how it works is that once Main Kenron is revived, the Child Kenron will still be active
        if (GameManager.Instance.Kenron.m_currentHealth > 0.0f)
        {
            //Enables Main Kenron with Weakened stats and disables Child Kenron
            Child.gameObject.SetActive(false);
            GameManager.Instance.Kenron.m_controllerOn = true;
            GameManager.Instance.Kenron.SetSpeed(15);
            GameManager.Instance.Kenron.SetHealth(40);
            GameManager.Instance.Kenron.SetDamage(35);
        }

        // Uses his Dash
        DashAttack();
    }

    /// <summary>
    /// This function checks the status (health) of the Main Kenron in the game
    /// </summary>
    public void CheckStatus()
    {
        // If main kenrons health is 0
        if (GameManager.Instance.Kenron.m_currentHealth <= 0.0f)
        {
            // disable him (Should be replaced with down animation)
            GameManager.Instance.Kenron.SetSpeed(0);
            GameManager.Instance.Kenron.m_controllerOn = false;
            if (m_camera.m_targets.Count > 0)
                m_camera.m_targets.Remove(GameManager.Instance.Kenron.gameObject.transform);
            this.enabled = true;
            this.SetHealth(50);
            this.SetSpeed(20);
            this.SetDamage(60);
        }       
    }

    /// <summary>
    /// Kenrons Attack. By Pressing the Right Trigger, Kenron Dashes Forward Dealing Damage to Any Enemies in his direction
    /// </summary>
    public void DashAttack()
    {
        // dash attack
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f && m_controllerOn && !m_triggerDown)
        {
            // set boolean flags
            m_triggerDown = true;
            m_controllerOn = false;
            isDashing = true;
            m_dashCollider.enabled = true;

            // calculate desired dash position
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            int playerLayer = LayerMask.NameToLayer("Player");
            int layerMask = 0;
            layerMask |= enemyLayer;
            layerMask |= playerLayer;
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out hit, m_maxDashDistance, layerMask))
            {
                m_dashPosition = hit.point;
                m_dashPosition -= transform.forward * transform.lossyScale.x;
            }
            else
            {
                m_dashPosition = transform.position + transform.forward * m_maxDashDistance;
            }

            // size hit box
            float dashDistance = (m_dashPosition - transform.position).magnitude;
            Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, dashDistance);
            m_dashCollider.size = hitBoxSize;

            // rotate hit box
            m_dashCollider.transform.rotation = transform.rotation;

            // position hit box
            m_dashCollider.transform.position = transform.position + transform.forward * (dashDistance / 2f);
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) < 0.1f && !isDashing)
        {
            m_triggerDown = false;
        }

        if (isDashing)
        {
            transform.position = Vector3.Lerp(transform.position, m_dashPosition, m_dashSpeed * Time.deltaTime);

            // if completed dash
            if ((m_dashPosition - transform.position).sqrMagnitude <= 0.1f)
            {
                // reset boolean flags
                m_controllerOn = true;
                isDashing = false;
                m_dashCollider.enabled = false;
            }
        }
    }

    /// <summary>
    /// Dashes Kenron forward and sets a cooldown until the next dash
    /// </summary>
    /// <returns> The Wait time until the next dash </returns>
    IEnumerator Dash()
    {
        // If we arent currently dashing 
        if (!isDashing)
        {
            // Add a Sudden Force to kenron and dashes him based on direction he is facing 
            m_childRigidBody.AddForce(GetSpeed() * m_childRigidBody.transform.forward, ForceMode.Impulse);

            // Kenron Dashing is true
            isDashing = true;
        }

        // Waits until the dash cooldown has ended
        yield return new WaitForSeconds(m_dashTime);

        // Kenrons dash check is reset
        isDashing = false;
    }
}
