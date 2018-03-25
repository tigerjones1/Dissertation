using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public void Start()
    {

    }

    public void PlayerHitCheckpoint()
    {
        StartCoroutine(PlayerHitCheckpointCo(LevelManager.Instance.CurrentTimeBonus));
    }

    private IEnumerator PlayerHitCheckpointCo(int bonus)
    {
        FloatingText.Show("Checkpoint!", "CheckpointText", new CenteredTextPositioner(0.5f));
        yield return new WaitForSeconds(0.5f);
    }
    
    public void PlayerLeftCheckpoint()
    {

    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);
    }

    public void AssignObjectToCheckpoint()
    {

    }

}
