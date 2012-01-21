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
		public int LastLevel;

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
		public int AttackDie = 4;
		public int AttackMultiplier = 1;
		public int AttackModifier = 0;
		public int Defense = 8;
		public int Speed = 4;

		//public TCODMap Map;
		public TCODPath Pathfinder;

		public int Infection = 0;

		public Factions Faction;

		public Character() {
			LastLevel = Level;
			Pathfinder = new TCODPath(Area.Current.Map, 1.0f);
			Game.Current.Characters.Add(this);
		}

		public bool IsZombie {
			get {
				return (Infection == 100);
			}
		}

		public void Update() {
			if (this != Game.Current.Player) {
				if (Game.Current.Player != null && Pathfinder.isEmpty())
					Pathfinder.compute(Position.X, Position.Y, Game.Current.Player.Position.X, Game.Current.Player.Position.Y);
				Pathfinder.walk(ref Position.X, ref Position.Y, true);
				int x, y;
				Pathfinder.getDestination(out x, out y);
				Console.WriteLine(x.ToString() + ", " + y.ToString());
			}
			if (LastLevel < Level) {
				LevelUp();
				LastLevel = Level;
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
			int attackRoll = 1 + Game.Current.RNG.Next(20);
			int attackTotal = attackRoll + Character.StatToModifier(Strength);
			if (attackTotal > target.Defense) {
				int damage = 0;
				for (int i = 0; i < AttackMultiplier; i++) damage += (attackRoll == 20 ? damage = AttackDie + AttackDie / 2 : 1 + Game.Current.RNG.Next(AttackDie));
				damage += AttackModifier;
				if (attackRoll == 20) damage += damage / 2;
				Stamina = Math.Max(Stamina - 40, 0);
				target.HP -= damage;
				Console.WriteLine((attackRoll == 20 ? "Critical hit! " : "") + (this == Game.Current.Player ? "You do " : ToString() + " does ") + damage.ToString() + " damage to " + target.ToString() + (target.HP <= 0 ? ", killing it." : "."));
				if (target.HP <= 0) {
					XP += target.HP / 2;
					target.Kill();
					Kills++;
				}
				XP += damage;
			}
			else {
				Console.WriteLine((this == Game.Current.Player ? "You swing" : ToString() + " swings") + " at " + (this == Game.Current.Player ? target.ToString() : "you") + ", but " + (this == Game.Current.Player ? "miss." : "misses."));
			}
		}

		public void Kill() {
			Game.Current.Characters.Remove(this);
		}

		public void LevelUp() {
			if (this == Game.Current.Player) Console.WriteLine("You level up!");
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
					c.Strength = 14;
					c.Dexterity = 8;
					c.Constitution = 8;
					c.Intelligence = 4;
					c.AttackDie = 6;
					c.AttackMultiplier = 1;
					c.AttackModifier = 0;
					c.Defense = 5;
					c.Speed = 1;
					c.MaxHP = 15;
					c.HP = c.MaxHP;
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
