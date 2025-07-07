using MazeEscape;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniMapEffect", menuName = "ItemEffect/MiniMapEffect")]
public class MiniMapEffect : ItemEffect
{
    public float duration;

    public override void Apply(GameManager gameManager)
    {
        gameManager.MiniMapService.Show(duration);
    }
}
