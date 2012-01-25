using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	public class Character : Entity {

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

		public enum AITypes {
			None,
			Zombie
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
		public int AttackDie {
			get {
				return (Weapon == null ? 4 : Weapon.AttackDie);
			}
		}
		public int AttackMultiplier = 1;
		public int AttackModifier = 0;
		public int Defense = 8;
		public int Speed = 4;

		public Item Weapon;


		private int idleTimer = 0;

		public int ViewRadius = 22;

		public int TurnTimeout = 0;
		public Character Target;

		public AITypes AIType = AITypes.None;

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
			if (LastLevel < Level) {
				LevelUp();
				LastLevel = Level;
			}
			if (HP <= MaxHP / 3 && Game.Current.RNG.Next(5) == 0) BleedDirectly();
			if (TurnTimeout > 0) {
				TurnTimeout--;
				return;
			}
			if (AIType == AITypes.Zombie) {

				Point positionLast = new Point(Position.X, Position.Y);

				// Don't have a target? Let's find one!
				bool computedFOV = false;
				bool wandered = false;
				if (Target == null) {
					if (!computedFOV) {
						Area.Current.Map.computeFov(Position.X, Position.Y, ViewRadius, true, TCODFOVTypes.BasicFov);
						computedFOV = true;
					}
					if (Area.Current.Map.isInFov(Game.Current.Player.Position.X, Game.Current.Player.Position.Y)) Target = Game.Current.Player;
				}

				// Is our target dead or something?
				if (Target != null && Target.HP < 1) Target = null;

				// OK, so we're chasing a target... can we still see him?
				if (Target != null) {
					if (!computedFOV) {
						Area.Current.Map.computeFov(Position.X, Position.Y, ViewRadius, true, TCODFOVTypes.BasicFov);
						computedFOV = true;
					}
					if (Area.Current.Map.isInFov(Target.Position.X, Target.Position.Y)) {
						foreach (Character c in Area.Current.Characters) {
							if (c != this && c != Target) Area.Current.Map.setProperties(c.Position.X, c.Position.Y, true, false);
						}
						if (!Pathfinder.compute(Position.X, Position.Y, Target.Position.X, Target.Position.Y)) {
							foreach (Character c in Area.Current.Characters) {
								Area.Current.Map.setProperties(c.Position.X, c.Position.Y, true, true);
							}
							if (!Pathfinder.compute(Position.X, Position.Y, Target.Position.X, Target.Position.Y)) {
								if (!wandered) {
									Wander();
									wandered = true;
								}
							}
						}
						else {
							foreach (Character c in Area.Current.Characters) {
								Area.Current.Map.setProperties(c.Position.X, c.Position.Y, true, true);
							}
						}
					}
				}

				if (Pathfinder.size() > 1) {
					int x, x2 = Position.X;
					int y, y2 = Position.Y;
					Pathfinder.get(0, out x, out y);
					MoveToPosition(new Point(x, y));
					if (Position.X == x && Position.Y == y) Pathfinder.walk(ref x2, ref y2, true);
					else {
						Target = null;
						if (!wandered) {
							Wander();
							wandered = true;
						}
					}
					TurnTimeout += 5 - Speed;
				}
				int dx, dy;
				Pathfinder.getDestination(out dx, out dy);
				if (Pathfinder.size() < 2 && Target != null && Target.Position.X == dx && Target.Position.Y == dy) {
					MeleeAttack(Target);
					idleTimer = 0;
					TurnTimeout += 2;
				}

				if (Target == null && Pathfinder.size() < 2) {
					if (!wandered) {
						Wander();
						wandered = true;
					}
				}

				// Wander if you aren't attacking an enemy and have been standing still for too long
				if (positionLast == Position) idleTimer += 1;
				else idleTimer = 0;
				if (idleTimer > 3) {
					idleTimer = 0;
					if (!wandered) {
						Wander();
						wandered = true;
					}
				}

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
				target.Bleed();
				damage = Math.Max(damage + AttackModifier, 1);
				//if (attackRoll == 20) damage += damage / 2;
				Stamina = Math.Max(Stamina - 40, 0);
				target.HP -= damage;
				Console.WriteLine((attackRoll == 20 ? "Critical hit! " : "") + (this == Game.Current.Player ? "You do " : ToString() + " does ") + damage.ToString() + " damage to " + (target == Game.Current.Player ? "you" : target.ToString()) + (target.HP <= 0 ? ", killing " + (target == Game.Current.Player ? "you" : "it") + "." : "."));
				if (target.HP <= 0) {
					XP += target.HP / 2;
					target.Kill();
					Kills++;
				}
				XP += damage;
			}
			else {
				Console.WriteLine((this == Game.Current.Player ? "You swing" : ToString() + " swings") + " at " + (target == Game.Current.Player ? "you" : target.ToString()) + ", but " + (this == Game.Current.Player ? "miss." : "misses."));
			}
		}

		public void Wander() {
			Point dest = new Point(0, 0);
			switch (Game.Current.RNG.Next(9)) {
				case 0: dest = new Point(1, 0); break;
				case 1: dest = new Point(1, -1); break;
				case 2: dest = new Point(0, -1); break;
				case 3: dest = new Point(-1, -1); break;
				case 4: dest = new Point(-1, 0); break;
				case 5: dest = new Point(-1, 1); break;
				case 6: dest = new Point(0, 1); break;
				case 7: dest = new Point(1, 1); break;
			}
			MoveToPosition(new Point(Position.X + dest.X, Position.Y + dest.Y));
			TurnTimeout += 5 - Speed - 1 + Game.Current.RNG.Next(3);
		}

		public void Bleed() {
			Point p = Position;
			if (Game.Current.RNG.Next(3) == 0) p += new Point(-1 + Game.Current.RNG.Next(3), -1 + Game.Current.RNG.Next(3));
			Decal.Generate(Decal.Prefabs.BloodDrops, p);
		}

		public void BleedDirectly() {
			Decal.Generate(Decal.Prefabs.BloodDrops, Position);
		}

		public void Kill() {
			Pathfinder.Dispose();
			//Item item = Item.Generate(Item.Prefab.Corpse, Position);
			for (int i = 0; i < 2 + Game.Current.RNG.Next(7); i++) Bleed();
			GenerateCorpse();
			Game.Current.Characters.Remove(this);
		}

		public Item GenerateCorpse() {
			Item item = new Item();
			item.Symbol = '%';
			item.ForegroundColor = TCODColor.desaturatedCrimson;
			item.Class = (IsZombie ? "zombie " : "") + "corpse";
			item.Area = Area;
			item.Position = Position;
			return item;
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
					c.Name = "Adam";
					c.Symbol = '@';
					c.Infection = 0;
					c.Faction = Factions.Survivors;
					c.Strength = 16;
					c.Dexterity = 12;
					c.Constitution = 12;
					c.Intelligence = 12;
					c.AttackMultiplier = 1;
					c.AttackModifier = 4;
					c.Defense = 10;
					c.Speed = 2;
					c.MaxHP = 30;
					c.HP = c.MaxHP;
					c.ViewRadius = 23;
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
					c.Strength = 8;
					c.Dexterity = 6;
					c.Constitution = 6;
					c.Intelligence = 4;
					c.AttackMultiplier = 1;
					c.AttackModifier = -2;
					c.Defense = 10;
					c.Speed = 1;
					c.MaxHP = 10;
					c.HP = c.MaxHP;
					c.AIType = AITypes.Zombie;
					c.ViewRadius = 14;
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
