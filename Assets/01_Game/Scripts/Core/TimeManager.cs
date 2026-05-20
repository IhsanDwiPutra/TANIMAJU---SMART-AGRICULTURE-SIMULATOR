using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timeSpeed = 60f; // Kecepatan waktu virtual
    private float currentSecond;
    public int currentHour = 6; // Mulai game jam 6 pagi
    public int currentDay = 1;

    void Update()
    {
        // Menghitung detik virtual berdasarkan waktu nyata
        currentSecond += Time.deltaTime * timeSpeed;

        if (currentSecond >= 60f)
        {
            currentSecond = 0;
            currentHour++;
            Debug.Log("Jam Virtual saat ini:" + currentHour + ":00");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay ++;
                Debug.Log("Masuk ke Hari ke-" + currentDay);
            }
        }
    }
}
