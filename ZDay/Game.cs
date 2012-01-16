﻿using System;
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
			Console.WriteLine("Welcome to Z-Day!");
			Console.WriteLine("Testing a really long line here that is so damn long, it's going to have to be wrapped.");
			Console.WriteLine("...Again, welcome to Z-Day!");
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
				case TCODKeyCode.Space:
					Console.WriteLine("TEST " + Convert.ToString(Console.Lines.Count));
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


			// bottom bar
			r.printFrame(0, windowHeight - 3, windowWidth - sidebarWidth, 3);


			// character box
			r.printFrame(vWidth, 0, windowWidth - vWidth, characterBoxHeight);
			r.print(vWidth + 2, 0, "CHARACTER");
			r.print(vWidth + 2, 2, "Adam");
			r.print(vWidth + 2, 3, "LVL: 1");
			r.print(vWidth + 2, 4, "SPD: 4");

			float barHP = ((float)8 / (float)10) * (windowWidth - vWidth - 4);
			float barStamina = ((float)67 / (float)100) * (windowWidth - vWidth - 4);
			float barXP = ((float)542 / (float)1000) * (windowWidth - vWidth - 4);

			r.setBackgroundFlag(TCODBackgroundFlag.Set);
			r.setBackgroundColor(TCODColor.darkGreen);
			r.rect(vWidth + 2, 6, (int)barHP, 1, false);
			r.setBackgroundColor(TCODColor.grey);
			r.printEx(vWidth + 2 + ((windowWidth - vWidth - 4) / 2), 6, TCODBackgroundFlag.Darken, TCODAlignment.CenterAlignment, " HP: 8/10 ");
			r.setBackgroundColor(TCODColor.darkBlue);
			r.rect(vWidth + 2, 7, (int)barStamina, 1, false);
			r.setBackgroundColor(TCODColor.grey);
			r.printEx(vWidth + 2 + ((windowWidth - vWidth - 4) / 2), 7, TCODBackgroundFlag.Darken, TCODAlignment.CenterAlignment, " STM: 67%% ");
			r.setBackgroundColor(TCODColor.darkYellow);
			r.rect(vWidth + 2, 8, (int)barXP, 1, false);
			r.setBackgroundColor(TCODColor.grey);
			r.printEx(vWidth + 2 + ((windowWidth - vWidth - 4) / 2), 8, TCODBackgroundFlag.Darken, TCODAlignment.CenterAlignment, " XP: 542 / 1000 ");
			r.setBackgroundColor(TCODColor.black);

			r.print(vWidth + 16, 2, "STR: 10");
			r.print(vWidth + 16, 3, "DEX: 10");
			r.print(vWidth + 24, 2, "CON: 10");
			r.print(vWidth + 24, 3, "INT: 10");
			r.print(vWidth + 16, 4, "ATK: 2d8 + 4");


			// console box
			r.printFrame(vWidth, characterBoxHeight, windowWidth - vWidth, windowHeight - weaponBoxHeight - characterBoxHeight);
			r.print(vWidth + 2, characterBoxHeight, "CONSOLE");
			int remainingLines = windowHeight - weaponBoxHeight - characterBoxHeight - 2;
			int i = Console.Lines.Count - 1;
			while (remainingLines > 0 && i > -1) {
				string text = Console.Lines[i].Text;
				Queue<string> lines = new Queue<string>();
				int maxLength = windowWidth - vWidth - 2;
				while (text.Length > maxLength) {
					lines.Enqueue(text.Substring(0, maxLength));
					text = " " + text.Substring(maxLength).Trim();
				}
				lines.Enqueue(text);
				remainingLines -= lines.Count;
				int j = 0;
				while (lines.Count > 0) {
					string line = lines.Dequeue();
					if (characterBoxHeight + 1 + remainingLines + j > characterBoxHeight) {
						r.print(vWidth + 1, characterBoxHeight + 1 + remainingLines + j, line);
					}
					j++;
				}
				
				i -= 1;
			}


			// weapon box
			r.printFrame(vWidth, 45, weaponBoxWidth, weaponBoxHeight);


			// time box
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
