using UnityEngine;
using UnityEngine.UI;

public class HungerBarScript : MonoBehaviour
{

    public UnityEngine.UI.Slider hungerBar;
    public HungerScripts playerHunger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (hungerBar == null)
        {
            hungerBar = GetComponent<UnityEngine.UI.Slider>();
        }

        if (playerHunger == null)
        {
            playerHunger = FindFirstObjectByType<HungerScripts>();
        }

        hungerBar.minValue = 0;
        hungerBar.maxValue = playerHunger.maxHunger;
        hungerBar.value = playerHunger.curHunger;
    }

    public void SetHunger(int hunger)
    {
        if (hungerBar != null)
        {
            hungerBar.value = hunger;
        }
    }
}
