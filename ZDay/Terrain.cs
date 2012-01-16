using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	public enum TerrainType {
		Void,
		Floor,
		Wall
	}
	class Terrain : Entity {
		public TerrainType Type;

		public Terrain(Point position, TerrainType type) {
			Type = type;
			Position = position;
			switch (type) {
				case TerrainType.Floor:
					Symbol = '.';
					ForegroundColor = TCODColor.darkGrey;
					break;
				case TerrainType.Wall:
					Symbol = '#';
					break;
			}
		}
	}
}
