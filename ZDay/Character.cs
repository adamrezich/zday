using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Character {
		public char Symbol = '?';
		public TCODColor Color = TCODColor.white;
		public Vector2 Position = new Vector2(0, 0);

		public void Draw(TCODConsole console) {
			console.putCharEx(Position.X, Position.Y, Symbol, Color, TCODColor.black);
		}
	}
}
