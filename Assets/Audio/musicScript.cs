using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
        if (Object.FindObjectsByType<MusicManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}