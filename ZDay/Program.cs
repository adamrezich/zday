using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Program {
		static void Main(string[] args) {
			TCODConsole.initRoot(80, 50, "Z-Day", false);
			TCODSystem.setFps(30);
			TCODMouse.showCursor(false);
			Game game = new Game();
			game.Play();
		}
	}
}
