using UnityEngine;

namespace GridGame {
  [System.Serializable]
  public struct Coordinate {
    public int x;
    public int y;
    public Coordinate(int x, int y) {
      this.x = x;
      this.y = y;
    }

    public static bool operator ==(Coordinate first, Coordinate second) {
      return first.x == second.x && first.y == second.y;
    }
    public static bool operator !=(Coordinate first, Coordinate second) {
      return first.x != second.x || first.y != second.y;
    }

    public override bool Equals(object obj) {
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }
        return x == ((Coordinate)obj).x && y == ((Coordinate)obj).y;
    }

    public override int GetHashCode()
    {
      unchecked {
        return 2^x*3^y;
      }
    }

    public int ManhattanDistance(Coordinate other) {
      return Mathf.Abs(x - other.x) + Mathf.Abs(y - other.y);
    }
  }
}
