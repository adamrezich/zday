using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Entity {
		public Area Area;
		public char Symbol = '?';
		public TCODColor ForegroundColor = TCODColor.white;
		public TCODColor BackgroundColor;
		public bool OverrideBackgroundColor = false;
		public Point Position = new Point(0, 0);
		public bool Solid = false;
		public bool Transparent = true;
		public string Name;

		public void Draw(TCODConsole console, Point offset) {
			if (Position.X > offset.X && Position.X <= offset.X + 45 && Position.Y > offset.Y && Position.Y <= offset.Y + 45) {
				console.putCharEx(Position.X - offset.X, Position.Y - offset.Y, Symbol, ForegroundColor, BackgroundColor == null ? TCODColor.black : BackgroundColor);
			}
		}
	}
}
