using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Character : Entity {

		[Flags]
		public enum Types {
			Human,
			Zombie,
			Military
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
		public int Stamina = 400;
		public int MaxStamina = 400;
		public int Kills = 0;

		public int Strength = 10;
		public int Dexterity = 10;
		public int Constitution = 10;
		public int Intelligence = 10;
		public int Attack = 0;
		public int Defense = 2;
		public int Speed = 4;

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
