using UnityEngine;

public class DayNightMusic : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public AudioSource dayMusic;
    public AudioSource nightMusic;

    private float fadeSpeed = 1f;

    private float dayMaxVolume = 0.06f;
    private float nightMaxVolume = 0.06f;

    void Start()
    {
        if (dayMusic != null)
        {
            dayMusic.loop = true;
            dayMusic.volume = 0f;
            dayMusic.Play();
        }

        if (nightMusic != null)
        {
            nightMusic.loop = true;
            nightMusic.volume = 0f;
            nightMusic.Play();
        }
    }

    void Update()
    {
        if (dayNightCycle == null || dayMusic == null || nightMusic == null)
        {
            return;
        }

        float targetDayVolume = dayNightCycle.isNight ? 0f : dayMaxVolume;
        float targetNightVolume = dayNightCycle.isNight ? nightMaxVolume : 0f;

        dayMusic.volume = Mathf.MoveTowards(dayMusic.volume, targetDayVolume, fadeSpeed * Time.deltaTime);
        nightMusic.volume = Mathf.MoveTowards(nightMusic.volume, targetNightVolume, fadeSpeed * Time.deltaTime);
    }
}