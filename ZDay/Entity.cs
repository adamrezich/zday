using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Entity {
		public char Symbol = '?';
		public TCODColor ForegroundColor = TCODColor.white;
		public TCODColor BackgroundColor = TCODColor.red;
		public bool OverrideBackgroundColor = false;
		public Point Position = new Point(0, 0);

		public void Draw(TCODConsole console, Point offset) {
			if (Position.X > offset.X && Position.X <= offset.X + 45 && Position.Y > offset.Y && Position.Y <= offset.Y + 45) {
				console.putCharEx(Position.X - offset.X, Position.Y - offset.Y, Symbol, ForegroundColor, TCODColor.black);
			}
		}
	}
}
