using UnityEngine;

public class BalloonTypeProvider : MonoBehaviour
{
  public BalloonType GetRandomBalloonType()
  {
    int roll = Random.Range(0, 100);

    if (roll < 35) return BalloonType.Red;
    if (roll < 60) return BalloonType.Blue;
    if (roll < 80) return BalloonType.Yellow;
    if (roll < 92) return BalloonType.Violet;

    return BalloonType.Black;
  }
}