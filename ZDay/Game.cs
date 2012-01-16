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
				Draw();
				Update();
				//Draw();
			}
		}

		public void Update() {
			var key = TCODConsole.waitForKeypress(true);

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

		public void DrawHUD() {
			TCODConsole r = TCODConsole.root;
			int sidebarWidth = 33;
			int windowWidth = 80;
			int windowHeight = 50;
			int vWidth = 47;
			int vHeight = 47;
			int weaponBoxWidth = 17;
			int weaponBoxHeight = 5;
			int characterBoxHeight = 11;

			r.printFrame(0, windowHeight - 3, windowWidth - sidebarWidth, 3);

			r.printFrame(vWidth, 0, windowWidth - vWidth, characterBoxHeight);
			r.print(vWidth + 2, 0, "CHARACTER");
			r.print(vWidth + 2, 2, "Adam");
			r.print(vWidth + 2, 3, "LVL: 1");
			r.print(vWidth + 2, 4, " XP: 1000");

			r.setBackgroundFlag(TCODBackgroundFlag.Set);
			r.setBackgroundColor(TCODColor.darkGreen);
			r.rect(vWidth + 2, 6, windowWidth - vWidth - 4, 1, false);
			r.setBackgroundColor(TCODColor.darkBlue);
			r.rect(vWidth + 2, 7, windowWidth - vWidth - 4, 1, false);
			r.setBackgroundColor(TCODColor.darkYellow);
			r.rect(vWidth + 2, 8, windowWidth - vWidth - 4, 1, false);
			r.setBackgroundColor(TCODColor.black);

			r.print(vWidth + 16, 2, "STR: 10");
			r.print(vWidth + 16, 3, "DEX: 10");
			r.print(vWidth + 24, 2, "CON: 10");
			r.print(vWidth + 24, 3, "INT: 10");
			r.print(vWidth + 16, 4, "ATK: 2d8 + 4");

			r.printFrame(vWidth, characterBoxHeight, windowWidth - vWidth, windowHeight - weaponBoxHeight - characterBoxHeight);
			r.print(vWidth + 2, characterBoxHeight, "CONSOLE");

			r.printFrame(vWidth, 45, weaponBoxWidth, weaponBoxHeight);

			r.printFrame(vWidth + weaponBoxWidth, 45, windowWidth - vWidth - weaponBoxWidth, weaponBoxHeight);
			string strKilled = "10000 KILLED";
			string strTime = "DAY 1 00:00.00";
			r.print(80 - strKilled.Length - 1, 48, strKilled);
			r.print(80 - strTime.Length - 1, 47, strTime);
		}

		public void Draw() {
			TCODConsole r = TCODConsole.root;
			r.clear();
			//TCODConsole.root.print(0, 0, "Hello, world");
			r.printFrame(0, 0, 47, 47);
			r.putChar(23, 23, '@');
			foreach (Character c in Characters) {
				c.Draw(TCODConsole.root);
			}
			DrawHUD();
			TCODConsole.flush();
		}
	}
}
