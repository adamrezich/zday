using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	public class Item : Entity {
		public enum Prefab {
			Corpse
		}
		public int AttackDie = 4;

		public Item() {
			Game.Current.Items.Add(this);
		}

		public static Item Generate(Prefab prefab, Point position) {
			Item item = new Item();
			switch (prefab) {
				case Prefab.Corpse:
					item.Symbol = '%';
					item.ForegroundColor = TCODColor.desaturatedCrimson;
					break;
			}
			item.Area = Area.Current;
			item.Position = position;
			return item;
		}
	}

	public class Weapon : Item {
		public override string ToString() {
			return Class;
		}
	}
}
