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

		public int Level = 1;
		public int XP = 0;
		public int HP = 10;
		public int MaxHP = 10;
		public int Stamina = 83;
		public int MaxStamina = 100;

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
