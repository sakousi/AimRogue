public static class BalloonPoints
{
  public static int GetPoints(BalloonType type)
  {
    switch (type)
    {
      case BalloonType.Red: return 100;
      case BalloonType.Blue: return 200;
      case BalloonType.Yellow: return 300;
      case BalloonType.Violet: return 500;
      case BalloonType.Black: return -200;
      default: return 0;
    }
  }
}