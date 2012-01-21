using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Character : Entity {

		public enum Prefab {
			Player,
			Survivor,
			Zombie
		}

		public enum Factions {
			Survivors,
			Undead,
			Military
		}

		public enum Attitude {
			Friendly,
			Hostile
		}

		public int Level {
			get {
				if (XP >= 1500) return 7;
				if (XP >= 1000) return 6;
				if (XP >= 600) return 5;
				if (XP >= 400) return 4;
				if (XP >= 200) return 3;
				if (XP >= 100) return 2;
				return 1;
			}
		}
		public int XP = 0;
		public int HP = 10;
		public int MaxHP = 10;
		public int Stamina = 2000;
		public int MaxStamina = 2000;
		public int Kills = 0;

		public int Strength = 10;
		public int Dexterity = 10;
		public int Constitution = 10;
		public int Intelligence = 10;
		public int Attack {
			get {
				return 4;
			}
		}
		public int Defense = 2;
		public int Speed = 4;

		public int Infection = 0;

		public Factions Faction;

		public Character() {
			Game.Current.Characters.Add(this);
		}

		public bool IsZombie {
			get {
				return (Infection == 100);
			}
		}

		public override string ToString() {
			if (Name != null) {
				if (IsZombie) return Name + "'s zombie";
				return Name;
			}
			if (IsZombie) return "a zombie";
			return "a survivor";
		}

		public Attitude AttitudeTowards(Character character) {
			switch (Faction) {
				case Factions.Survivors:
					switch (character.Faction) {
						case Factions.Survivors: return Attitude.Friendly;
						case Factions.Undead: return Attitude.Hostile;
						case Factions.Military: return Attitude.Friendly;
					}
					break;
				case Factions.Undead:
					switch (character.Faction) {
						case Factions.Survivors: return Attitude.Hostile;
						case Factions.Undead: return Attitude.Friendly;
						case Factions.Military: return Attitude.Hostile;
					}
					break;
				case Factions.Military:
					switch (character.Faction) {
						case Factions.Survivors: return Attitude.Friendly;
						case Factions.Undead: return Attitude.Hostile;
						case Factions.Military: return Attitude.Friendly;
					}
					break;
			}
			return Attitude.Friendly;
		}

		public void MoveToPosition(Point position) {
			if (Area.Current.SolidTerrainAt(position)) return;
			Character c = Area.Current.CharacterAt(position);
			if (c != null) {
				switch (AttitudeTowards(c)) {
					case Attitude.Friendly: return;
					case Attitude.Hostile:
						MeleeAttack(c);
						/*if (Game.Current.Characters.Contains(c))*/
						return;
						//break;
				}
			}
			Position = position;
		}

		public void MeleeAttack(Character target) {
			if (Game.Current.RNG.Next(20) + Character.StatToModifier(Strength) > target.Defense) {
				int damage = 1 + Game.Current.RNG.Next(Attack);
				Console.WriteLine((this == Game.Current.Player ? "You do " : ToString() + " does ") + damage.ToString() + " damage to " + target.ToString() + ".");
			}
			//character.Kill();
		}

		public void Kill() {
			Game.Current.Characters.Remove(this);
		}

		public static int StatToModifier(int stat) {
			return ((stat - 10) / 2);
		}

		public static Character Generate(Prefab prefab, Point position) {
			Character c = new Character();
			switch (prefab) {
				case Prefab.Player:
					c.Symbol = '@';
					c.Infection = 0;
					c.Faction = Factions.Survivors;
					break;
				case Prefab.Survivor:
					c.Symbol = '@';
					c.Infection = 0;
					c.Faction = Factions.Survivors;
					break;
				case Prefab.Zombie:
					c.Symbol = 'z';
					c.ForegroundColor = TCODColor.desaturatedGreen;
					c.Infection = 100;
					c.Faction = Factions.Undead;
					break;
			}
			c.Area = Area.Current;
			c.Position = position;
			return c;
		}

		public static int LevelXP(int level) {
			switch (level) {
				case 1: return 0;
				case 2: return 100;
				case 3: return 200;
				case 4: return 400;
				case 5: return 600;
				case 6: return 1000;
				case 7: return 1500;
			}
			return 0;
		}
	}
}
