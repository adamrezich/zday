using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZDay {
	public class Point {
		public int X;
		public int Y;

		public Point(int x, int y) {
			X = x;
			Y = y;
		}

		public override string ToString() {
			return string.Format("[{0},{1}]", X, Y); 
		}

		public static bool operator ==(Point p1, Point p2) {
			if ((object)p1 == null) return false;
			if ((object)p2 == null) return false;
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(Point p1, Point p2) {
			if ((object)p1 == null) return false;
			if ((object)p2 == null) return false;
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public static Point operator +(Point p1, Point p2) {
			return new Point(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point operator -(Point p1, Point p2) {
			return new Point(p1.X - p2.X, p1.Y - p2.Y);
		}

		public override bool Equals(object obj) {
			if (obj == null) return false;
			Point p = obj as Point;
			if ((System.Object)p == null) return false;
			return (X == p.X && Y == p.Y);
		}

		public override int GetHashCode() {
			return X ^ Y;
		}

		public int DistanceTo(Point p2) {
			if (p2 == null) return -1;
			return (int)Math.Sqrt((X - p2.X) * (X - p2.X) + (Y - p2.Y) * (Y - p2.Y));
		}
	}
}
