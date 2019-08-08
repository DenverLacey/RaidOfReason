using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Elisha_Anagnostakis
 * Description: This class handles the speed at which thea's projectiles 
 *              travel within the world when shot.
 */

public class ProjectileMove : MonoBehaviour {

    [SerializeField]
    private float m_projectileLife;
    [SerializeField]
    private float m_projectileSpeed;
    [SerializeField]
    private float m_healAmount;
    [SerializeField]
    private int m_damage;

    /// <summary>
    /// Sets the damage dealt by the projectile.
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(float damage)
    {
        this.m_damage = (int)damage;
    }

    // Update is called once per frame
    void Update () {

        // If projectile is active
		if(m_projectileSpeed != 0)
        {
            // Project the objects transform forward 
            transform.position += transform.forward * (m_projectileSpeed * Time.deltaTime);
            // Time at which the projectile lives for.
            m_projectileLife -= Time.deltaTime;
            if(m_projectileLife <= 0f)
            {
                // Destroy once time is 0.
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("no projectile speed");
        }
	}

    /// <summary>
    /// This function resloves what happens when Theas projectile collides 
    /// with enemies, allies or the enviornment.
    /// </summary>
    /// <param name="other"></param>
    public void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        // If the projectile hits an object with the tag 'Enemy'.
        if (tag == "Enemy")
        {
            // Create non member variable that holds all data to do with the enemy.
            EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
            // If True
            if (enemy)
            {
                // Enemy takes damage.
                enemy.TakeDamage(m_damage);
                
                //NASHORN ABILITY
                if (GameManager.Instance.Thea.nashornBuffGiven == true && GameManager.Instance.Nashorn.isTaunting == true)
                {
                    float randomValue = Random.value;
                    if (GameManager.Instance.Nashorn.stunChance < randomValue)
                    {
                        other.gameObject.GetComponent<StatusEffectManager>().ApplyStun(1.5f);
                    }
                }
                else if (GameManager.Instance.Nashorn.isTaunting == false)
                {
                    GameManager.Instance.Thea.nashornBuffGiven = false;
                }

                if (GameManager.Instance.Kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning") && GameManager.Instance.Kenron.isActive == true)
                {
                    other.gameObject.GetComponent<StatusEffectManager>().ApplyBurn(4);
                }

                // Enemy mesh colour changes to red.
                enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                // Change the enemy back to their original mesh colour after .2 seconds of being hit.
                StartCoroutine(ResetMaterialColour(enemy, .2f));
            }

        }
        else // If above statment is false.
        {
            // Check if the projectile hit ally game objects.
            switch (other.gameObject.tag)
            {
                case "Kenron":
                    Kenron m_kenron = other.gameObject.GetComponent<Kenron>();
                    // Heals Kenrons' current health.
                    m_kenron.m_currentHealth += m_healAmount;
                    break;
                case "Nashorn":
                    Nashorn m_nashorn = other.gameObject.GetComponent<Nashorn>();
                    // Heals Nashorns' current health.
                    m_nashorn.m_currentHealth += m_healAmount;
                    break;
                default:
                    Debug.Log("projectile missed player");
                    break;
            }
        }
        // Destroy projectile.
		Destroy(gameObject);
    }

    /// <summary>
    /// A corotine that resets the enemies mesh colour back to normal when called.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="delay"></param>
    /// <returns> Enemy data and float value. </returns>
    IEnumerator ResetMaterialColour(EnemyData enemy, float delay)
    {
        // Suspends the coroutine execution for the given amount of seconds.
        yield return new WaitForSeconds(delay);

        // If enemy gets returned.
        if (enemy)
        {
            // Change enemy mesh colour back to the original colour.
            enemy.GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }
}