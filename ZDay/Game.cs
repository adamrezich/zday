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
			while (!Over && !TCODConsole.isWindowClosed()) {
				Update();
				Draw();
			}
		}

		public void Update() {
			var key = TCODConsole.checkForKeypress();

			switch (key.KeyCode) {
				case TCODKeyCode.Escape:
					Over = true;
					return;
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
				case TCODKeyCode.KeypadSeven:
					player.Position.X--;
					player.Position.X--;
					break;
				case TCODKeyCode.KeypadNine:
					player.Position.X++;
					player.Position.Y--;
					break;
				case TCODKeyCode.KeypadOne:
					player.Position.X--;
					player.Position.Y++;
					break;
				case TCODKeyCode.KeypadThree:
					player.Position.X++;
					player.Position.Y++;
					break;

			}
		}

		public void Draw() {
			TCODConsole.root.clear();
			//TCODConsole.root.print(0, 0, "Hello, world");
			foreach (Character c in Characters) {
				c.Draw(TCODConsole.root);
			}
			TCODConsole.flush();
		}
	}
}
