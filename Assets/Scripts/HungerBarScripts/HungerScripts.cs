using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HungerScripts : MonoBehaviour
{
    public int curHunger;
    public int maxHunger = 100;

    public float hungerDecreaseInterval = 5f;
    public int hungerDecreaseAmount = 1;

    public Inventory inventory;
    public ItemData appleItem;
    public int appleRestoreAmount = 15;

    public HungerBarScript hungerBar;
    private float hungerTimer;
    private bool hasLost;

    private bool _cursorLocked;

    private void Start()
    {
        curHunger = maxHunger;
        hungerTimer = hungerDecreaseInterval;
        hasLost = false;

        if (hungerBar == null)
        {
            hungerBar = FindFirstObjectByType<HungerBarScript>();
        }

        if (inventory == null)
        {
            inventory = FindFirstObjectByType<Inventory>();
        }

        _cursorLocked = true;
        UpdateHungerBar();
    }

    private void Update()
    {
        if (hasLost)
        {
            return;
        }

        hungerTimer -= Time.deltaTime;

        if (hungerTimer <= 0f)
        {
            DeteriorateHunger(hungerDecreaseAmount);
            hungerTimer = hungerDecreaseInterval;
        }

        // current only way to get player eat the apple
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            EatApple();
        }
        
    }

    public void DeteriorateHunger(int amount)
    {
        if (hasLost || amount <= 0)
        {
            return;
        }

        curHunger -= amount;
        curHunger = Mathf.Clamp(curHunger, 0, maxHunger);
        UpdateHungerBar();

        if (curHunger <= 0)
        {
            LoseGame();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _cursorLocked = false;
        }
    }

    public void RestoreHunger(int amount)
    {
        if (hasLost || amount <= 0)
        {
            return;
        }

        curHunger += amount;
        curHunger = Mathf.Clamp(curHunger, 0, maxHunger);
        UpdateHungerBar();
    }

    public void OnEat(InputAction.CallbackContext context)
    {
        if (!context.performed || hasLost)
        {
            return;
        }

        EatApple();
    }

    public void EatApple()
    {
        if (inventory == null)
        {
            return;
        }

        if (appleItem == null)
        {
            return;
        }

        if (curHunger >= maxHunger)
        {
            Debug.Log("You are already full.");
            return;
        }

        if (inventory.GetItemCount(appleItem) <= 0)
        {
            Debug.Log("You do not have any apples.");
            return;
        }

        bool appleRemoved = inventory.RemoveItem(appleItem, 1);
        if (appleRemoved)
        {
            RestoreHunger(appleRestoreAmount);
        }
    }

    private void LoseGame()
    {
        if (hasLost)
        {
            return;
        }

        hasLost = true;

        GameUI.SetLoseResult();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void UpdateHungerBar()
    {
        if (hungerBar != null)
        {
            hungerBar.SetHunger(curHunger);
        }
    }
}