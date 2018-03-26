using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    public Player Player;
    public float VolumeScale = 0.3f;
    public int VolumeFallOffDistance = 40;

    public void Awake()
    {
        Instance = this;
    }

    public void PlayClip3D(AudioClip clip, Vector2 effectPosition)
    {
        var volume = (1 - Vector2.Distance(Player.transform.position, effectPosition) / VolumeFallOffDistance) * VolumeScale;
        AudioSource.PlayClipAtPoint(clip, effectPosition, volume);
    }
}