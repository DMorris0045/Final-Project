using UnityEngine;
using System.Collections;

public class GrowApples : MonoBehaviour
{
    public GameObject appleObject;
    public InteractableResources appleResource;
    public Collider[] appleCollider;

    public float growthTime = 60f;
    public bool startWithApples;

    private Coroutine growthCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startWithApples)
        {
            ApplesReady();
        }
        else
        {
            BeginGrowing();
        }
    }

    public void BeginGrowing()
    {
        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
        }

        SetApplesAvailable(false);

        growthCoroutine = StartCoroutine(GrowApplesRoutine());
    }

    private IEnumerator GrowApplesRoutine()
    {
        yield return new WaitForSeconds(growthTime);

        ApplesReady();
        growthCoroutine = null;
    }

    private void ApplesReady()
    {
        if (appleObject != null)
        {
            appleObject.SetActive(true);
        }

        SetApplesAvailable(true);
    }

    private void SetApplesAvailable(bool available)
    {
        if (appleObject != null)
        {
            appleObject.SetActive(available);
        }

        if (appleResource != null)
        {
            appleResource.enabled = available;
        }

        if (appleCollider != null)
        {
            foreach (Collider appleCollider in appleCollider)
            {
                if (appleCollider != null)
                {
                    appleCollider.enabled = available;
                }
            }
        }
    }
}
