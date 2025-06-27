using MazeEscape;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniMapEffect", menuName = "ItemEffect/MiniMapEffect")]
public class MiniMapEffect : ItemEffect
{
    public float duration = 5f;

    public override void Apply(GameManager gameManager)
    {
        gameManager.MiniMapService.Show(duration);
    }
}
