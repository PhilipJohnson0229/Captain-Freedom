using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BNG;

public class HealingZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image healPowerBar;

    [Header("Settings")]
    [SerializeField] private int maxHealPower = 30;
    [SerializeField] private float healCooldown = 60f;
    [SerializeField] private float healTickRate = 1f;
    [SerializeField] private int coinsPerTick = 10;
    [SerializeField] private int healthPerTick = 10;

    private float remainingCooldown;
    private float tickTimer;

    private List<BNGPlayerController> playersInZone = new List<BNGPlayerController>();

    private int HealPower;





    private void OnTriggerEnter(Collider col)
    {

        BNGPlayerController player = col.GetComponent<BNGPlayerController>();
        
        if (player == null) { return; }

        playersInZone.Add(player);
    }

    private void OnTriggerExit(Collider col)
    {

        BNGPlayerController player = col.GetComponent<BNGPlayerController>();

        if (player == null) { return; }

        playersInZone.Remove(player);
    }

    private void Update()
    {

        if (remainingCooldown > 0f)
        {
            remainingCooldown -= Time.deltaTime;

            if (remainingCooldown <= 0f)
            {
                HealPower = maxHealPower;
            }
            else
            {
                return;
            }
        }

        tickTimer += Time.deltaTime;
        if (tickTimer >= 1 / healTickRate)
        {
            foreach (BNGPlayerController player in playersInZone)
            {
                if (HealPower == 0) { break; }

                //if (player.Health.CurrentHealth == player.Health.MaxHealth) { continue; }

                //if (player.Wallet.TotalCoins < coinsPerTick) { continue; }

                //player.Wallet.SpendCoins(coinsPerTick);
                //player.Health.RestoreHealth(healthPerTick);

                HealPower -= 1;

                if (HealPower == 0)
                {
                    remainingCooldown = healCooldown;
                }
            }

            tickTimer = tickTimer % (1 / healTickRate);
        }
    }

    private void HandleHealPowerChanged(int oldHealPower, int newHealPower)
    {
        healPowerBar.fillAmount = (float)newHealPower / maxHealPower;
    }

}
