using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [Range(5, 100)]
    public float NoteSpeed;
    [Range(5, 50)]
    public float ThresholdPerfect;
    [Range(5, 50)]
    public float ThresholdNormal;
    [Range(0f, 3f)]
    public float TimeBonus;
    public int PerfectScore = 200;
    public int NormalScore = 100;
    public int BadScore = 50;
    public int BonusScore = 50;
    [Range(-100f, 100f)]
    public float NoteEndPointCalibrate = 0.5f;
    [Range(0, 1.0f)]
    public float MinNoteInterval = 0.2f;

    [Range(5, 50)] public float TimeSongPlayed;
}
