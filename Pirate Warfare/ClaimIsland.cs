using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// Script that handles island claiming
/// </summary>
public class ClaimIsland : MonoBehaviour
{  

    public float maxHealth = 100f;
    public float curHealth;

    public static event Action<ClaimIsland, GameObject> OnIslandDestroyed;
    public static event Action<int> OnPlayerIslandsClaimed;

    [SerializeField] private ResourseBonusIsland bonus;

    [SerializeField] private ResourseManager resourse;

    ///visuals for showing if the player or enemy claimed the island
    [SerializeField] private GameObject claimPlayer;
    [SerializeField] private GameObject claimEnemy;

    [SerializeField] private HealthBar health;

    ///assigned resource boost for island
    private GameObject assignedChild;
    ///amount of islands the player has claimed
    private static int playerIslandsClaimed;

    private void Start()
    {
        playerIslandsClaimed = 0;
        curHealth = maxHealth;
        health = GetComponentInChildren<HealthBar>();
        bonus = assignedChild.GetComponent<ResourseBonusIsland>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    ///assign a resource boost to each island
    public void AssignChild(GameObject child)
    {
        assignedChild = child;
        bonus = assignedChild.GetComponent<ResourseBonusIsland>();
        Debug.Log($"{child.name} assigned to {this.name}");
    }
    
    ///recieve damage and check who the attacker was
    public void Damage(float amount, GameObject attacker)
    {
        curHealth -= amount;
        health.UpdateHealthBar(curHealth, maxHealth);

        if (curHealth <= 0)
        {
            curHealth = 0;
            Die(attacker);
        }
    }

    ///Decide what to do if health is zero
    ///If a player ship attack the island activate its bonus resource and add it to the list of claimed islands
    ///If an enemy ship claimed the island deactivate its bonus resource and remove it from the list of claimed islands
    private void Die(GameObject attacker)
    {
        OnIslandDestroyed?.Invoke(this, attacker);
        curHealth = maxHealth;
        health.UpdateHealthBar(curHealth, maxHealth);

        if (attacker.CompareTag("Player"))
        {
            claimEnemy.SetActive(false);
            claimPlayer.SetActive(true);
            gameObject.tag = "islandPlayer";
            bonus.Activate();
            playerIslandsClaimed++;
            OnPlayerIslandsClaimed?.Invoke(playerIslandsClaimed);
            if (playerIslandsClaimed == 3)
            {
                Debug.Log("Player has claimed 3 islands.");
                SceneManager.LoadSceneAsync(0);
            }
        }
        else if (attacker.CompareTag("Enemy"))
        {
            claimPlayer.SetActive(false);
            claimEnemy.SetActive(true);
            gameObject.tag = "Enemy";
            playerIslandsClaimed--;
            bonus.DeActivate();
        }

        if (assignedChild != null)
        {
            Debug.Log($"{assignedChild.name} was assigned to this island.");
        }
    }

    ///Refill island health and remove 200 gold
    public void RefillHealth()
    {
        if (this.gameObject.CompareTag("islandPlayer") && resourse.curGold >= 200)
        {
            resourse.curGold -= 200;
            curHealth = maxHealth;
            health.UpdateHealthBar(curHealth, maxHealth);
        }
    }
}