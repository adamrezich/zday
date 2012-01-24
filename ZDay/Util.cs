using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZDay {
	public struct Point {
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

		/*public override bool Equals(object obj) {
			if (obj == null) return false;
			if (!(obj is Point)) return false;
			return (X == (obj as Point).X && Y == (obj as Point).Y);
		}*/

		public override bool Equals(object obj) {
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return X ^ Y;
		}

		public int DistanceTo(Point p2) {
			if (p2 == null) return -1;
			return (int)Math.Sqrt((X - p2.X) * (X - p2.X) + (Y - p2.Y) * (Y - p2.Y));
		}
	}

	public static class English {
		public static string NumberToWords(int number) {
			if (number == 0)
				return "zero";

			if (number < 0)
				return "negative " + NumberToWords(Math.Abs(number));

			string words = "";

			if ((number / 1000000) > 0) {
				words += NumberToWords(number / 1000000) + " million ";
				number %= 1000000;
			}

			if ((number / 1000) > 0) {
				words += NumberToWords(number / 1000) + " thousand ";
				number %= 1000;
			}

			if ((number / 100) > 0) {
				words += NumberToWords(number / 100) + " hundred ";
				number %= 100;
			}

			if (number > 0) {
				if (words != "")
					words += "and ";

				var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
				var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

				if (number < 20)
					words += unitsMap[number];
				else {
					words += tensMap[number / 10];
					if ((number % 10) > 0)
						words += "-" + unitsMap[number % 10];
				}
			}

			return words;
		}
		public static string SingularPronoun(string phrase) {
			if (phrase == "" || phrase == null) return "";
			switch (phrase.Substring(0, 1).ToLower()) {
				case "a":
				case "e":
				case "i":
				case "o":
				case "u":
					return "an";
				default:
					return "a";
			}
		}
	}
}
