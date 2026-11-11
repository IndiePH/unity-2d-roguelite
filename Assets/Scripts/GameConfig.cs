using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Game Config")]
public class GameConfig : ScriptableObject
{
    [Tooltip("How many waves a run has. The last wave is this value. Do not branch on 20.")]
    [SerializeField] int waveCount = 20;

    public int WaveCount => Mathf.Max(1, waveCount);

    void OnValidate()
    {
        if (waveCount < 1)
            waveCount = 1;
    }
}
