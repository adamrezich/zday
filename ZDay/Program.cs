using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Program {
		static void Main(string[] args) {
			TCODConsole.initRoot(80, 50, "my game", false);
			TCODSystem.setFps(25);
			bool endGame = false;
			while (!endGame && !TCODConsole.isWindowClosed()) {
				TCODConsole.root.print(0, 0, "Hello, world");
				TCODConsole.flush();
				var key = TCODConsole.checkForKeypress();

				if (key.KeyCode == TCODKeyCode.Escape) {
					endGame = true;
				}
			}
		}
	}
}
