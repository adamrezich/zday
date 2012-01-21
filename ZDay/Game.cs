using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Game {
		public static Game Current;
		public bool Over = false;
		public Character Player;
		public List<Character> Characters = new List<Character>();
		public Random RNG = new Random();

		public Game() {
		}

		public void Initialize() {
			Area.Current = new Area();
			Area.Current.Generate();
			Console.Lines = new List<ConsoleLine>();
			Player = Character.Generate(Character.Prefab.Player, new Point(4, 4));
		}

		public void Play() {
			Console.WriteLine("Welcome to Z-Day!");
			while (!Over && !TCODConsole.isWindowClosed()) {
				Draw();
				Update();
			}
		}

		public void Update() {
			if (Player.TurnTimeout == 0) {
				var key = TCODConsole.waitForKeypress(true);
				Point dest = new Point(Player.Position.X, Player.Position.Y);
				switch (key.KeyCode) {
					case TCODKeyCode.Escape:
						Over = true;
						return;
					case TCODKeyCode.KeypadFour:
					case TCODKeyCode.Left:
						dest.X--;
						break;
					case TCODKeyCode.KeypadEight:
					case TCODKeyCode.Up:
						dest.Y--;
						break;
					case TCODKeyCode.KeypadSix:
					case TCODKeyCode.Right:
						dest.X++;
						break;
					case TCODKeyCode.KeypadTwo:
					case TCODKeyCode.Down:
						dest.Y++;
						break;
					case TCODKeyCode.KeypadSeven:
						dest.X--;
						dest.Y--;
						break;
					case TCODKeyCode.KeypadNine:
						dest.X++;
						dest.Y--;
						break;
					case TCODKeyCode.KeypadOne:
						dest.X--;
						dest.Y++;
						break;
					case TCODKeyCode.KeypadThree:
						dest.X++;
						dest.Y++;
						break;
					case TCODKeyCode.KeypadDecimal:
					case TCODKeyCode.KeypadFive:
						if (Player.Stamina < Player.MaxStamina) Player.Stamina = Math.Min(Player.Stamina + 10, Player.MaxStamina);
						break;
					case TCODKeyCode.Enter:
						if (TCODConsole.isKeyPressed(TCODKeyCode.Alt)) TCODConsole.setFullscreen(!TCODConsole.isFullscreen());
						break;
				}
				if ((dest.X != Player.Position.X || dest.Y != Player.Position.Y) && Player.Stamina > 0) {
					Player.MoveToPosition(dest);
					Player.Stamina--;
					Player.TurnTimeout += 5 - Player.Speed;
				}
			}
			foreach (Character c in Area.Current.Characters) {
				c.Update();
			}
		}

		public void DrawHUD() {
			TCODConsole r = TCODConsole.root;
			int sidebarWidth = 33;
			int windowWidth = 80;
			int windowHeight = 50;
			int vWidth = 47;
			int vHeight = 47;
			int weaponBoxWidth = 16;
			int weaponBoxHeight = 4;
			int characterBoxHeight = 11;


			// bottom bar
			r.printFrame(0, windowHeight - 3, windowWidth - sidebarWidth, 3);


			// character box
			r.printFrame(vWidth, 0, windowWidth - vWidth, characterBoxHeight);
			r.print(vWidth + 2, 0, "CHARACTER");
			r.print(vWidth + 2, 2, "Adam");
			r.print(vWidth + 2, 3, "LVL: " + Convert.ToString(Player.Level));
			r.print(vWidth + 2, 4, "ATK: " + (Player.AttackMultiplier > 1 ? Player.AttackMultiplier.ToString() : "") + "d" + Player.AttackDie.ToString() + (Player.AttackModifier > 0 ? "+" + Player.AttackModifier.ToString() : ""));

			float barHP = ((float)Player.HP / (float)Player.MaxHP) * (windowWidth - vWidth - 4);
			float barStamina = ((float)Player.Stamina / (float)Player.MaxStamina) * (windowWidth - vWidth - 4);
			float barXP = ((float)(Player.XP - Character.LevelXP(Player.Level)) / (float)(Character.LevelXP(Player.Level + 1) - Character.LevelXP(Player.Level))) * (windowWidth - vWidth - 4);

			r.setBackgroundFlag(TCODBackgroundFlag.Set);
			r.setBackgroundColor(TCODColor.darkGreen);
			r.rect(vWidth + 2, 6, (int)barHP, 1, false);
			r.setBackgroundColor(TCODColor.grey);
			r.printEx(vWidth + 2 + ((windowWidth - vWidth - 4) / 2), 6, TCODBackgroundFlag.Darken, TCODAlignment.CenterAlignment, " HP: " + Convert.ToString(Player.HP) + "/" + Convert.ToString(Player.MaxHP) + " ");
			r.setBackgroundColor(TCODColor.darkBlue);
			r.rect(vWidth + 2, 7, (int)barStamina, 1, false);
			r.setBackgroundColor(TCODColor.grey);
			r.printEx(vWidth + 2 + ((windowWidth - vWidth - 4) / 2), 7, TCODBackgroundFlag.Darken, TCODAlignment.CenterAlignment, " STM: " + Convert.ToString(Math.Round((float)Player.Stamina / (float)Player.MaxStamina * 100)) + "%% ");
			r.setBackgroundColor(TCODColor.darkYellow);
			r.rect(vWidth + 2, 8, (int)barXP, 1, false);
			r.setBackgroundColor(TCODColor.grey);
			r.printEx(vWidth + 2 + ((windowWidth - vWidth - 4) / 2), 8, TCODBackgroundFlag.Darken, TCODAlignment.CenterAlignment, " XP: " + Convert.ToString(Player.XP) + " / " + Convert.ToString(Character.LevelXP(Player.Level + 1)) + " ");
			r.setBackgroundColor(TCODColor.black);

			r.print(vWidth + 16, 2, "STR: " + Convert.ToString(Player.Strength));
			r.print(vWidth + 16, 3, "DEX: " + Convert.ToString(Player.Dexterity));
			r.print(vWidth + 24, 2, "CON: " + Convert.ToString(Player.Constitution));
			r.print(vWidth + 24, 3, "INT: " + Convert.ToString(Player.Intelligence));
			r.print(vWidth + 16, 4, "DEF: " + Convert.ToString(Player.Defense));
			r.print(vWidth + 24, 4, "SPD: " + Convert.ToString(Player.Speed));



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
					string split = text.Substring(0, maxLength);
					if (text.Substring(maxLength, 1) != " ") split = split.Substring(0, split.LastIndexOf(" ")).TrimEnd();
					split = split.TrimEnd();
					lines.Enqueue(split);
					text = " " + text.Substring(split.Length).Trim();
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
			r.printFrame(vWidth, windowHeight - weaponBoxHeight, weaponBoxWidth, weaponBoxHeight);
			r.print(vWidth + 1, windowHeight - weaponBoxHeight + 1, "unarmed");


			// time box
			r.printFrame(vWidth + weaponBoxWidth, windowHeight - weaponBoxHeight, windowWidth - vWidth - weaponBoxWidth, weaponBoxHeight);
			string strKilled = Convert.ToString(Player.Kills) + " KILLED";
			string strTime = "DAY 1 00:00.00";
			r.print(80 - strKilled.Length - 1, 48, strKilled);
			r.print(80 - strTime.Length - 1, 47, strTime);
		}

		public void Draw() {
			TCODConsole r = TCODConsole.root;
			r.clear();

			// world box
			r.printFrame(0, 0, 47, 47);
			r.print(2, 0, "Z-DAY v0.01");
			Point offset = new Point(Player.Position.X - 23, Player.Position.Y - 23);
			Area.Current.Map.computeFov(Player.Position.X, Player.Position.Y, Player.ViewRadius, true, TCODFOVTypes.BasicFov);
			foreach (Terrain t in Area.Current.Terrain) {
				if (Area.Current.Map.isInFov(t.Position.X, t.Position.Y))
					t.Draw(TCODConsole.root, offset);
			}
			foreach (Character c in Area.Current.Characters) {
				//if (Area.Current.Map.isInFov(c.Position.X, c.Position.Y))
					c.Draw(TCODConsole.root, offset);
			}

			DrawHUD();
			TCODConsole.flush();
		}
	}
}
