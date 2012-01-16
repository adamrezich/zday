using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Game {
		public bool Over = false;
		List<Character> Characters = new List<Character>();
		Character player;

		public Game() {
			player = new Character();
			player.Symbol = '@';
			Characters.Add(player);
		}

		public void Play() {
			TCODConsole.root.clear();
			//TCODConsole.root.print(0, 0, "Hello, world");
			foreach (Character c in Characters) {
				c.Draw(TCODConsole.root);
			}
			TCODConsole.flush();
			var key = TCODConsole.checkForKeypress();

			if (key.KeyCode == TCODKeyCode.Escape) {
				Over = true;
			}

			switch (key.KeyCode) {
				case TCODKeyCode.KeypadFour:
				case TCODKeyCode.Left:
					player.Position.X--;
					break;
				case TCODKeyCode.KeypadEight:
				case TCODKeyCode.Up:
					player.Position.Y--;
					break;
				case TCODKeyCode.KeypadSix:
				case TCODKeyCode.Right:
					player.Position.X++;
					break;
				case TCODKeyCode.KeypadTwo:
				case TCODKeyCode.Down:
					player.Position.Y++;
					break;
			}
		}
	}
}
