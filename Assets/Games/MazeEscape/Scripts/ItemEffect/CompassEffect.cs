using Breakout;
using UnityEngine;

[CreateAssetMenu(fileName = "CompassEffect", menuName = "ItemEffect/CompassEffect")]
public class CompassEffect : ItemEffect
{
    public float duration;

    public override void Apply(MazeEscape.GameManager gameManager)
    {
        gameManager.CompassService.Show(duration);
    }
}
