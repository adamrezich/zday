using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	class Area {
		public static Area Current;
		public List<Terrain> Terrain = new List<Terrain>();

		public int Height;
		public int Width;
		public TCODMap Map;

		public List<Character> Characters {
			get {
				IEnumerable<Character> q = from c in Game.Current.Characters
										 where c.Area == this
										 select c;
				return q.ToList<Character>();
			}
		}

		public bool SolidTerrainAt(Point position) {
			IEnumerable<Terrain> q = from f in Terrain
									 where f.Position.X == position.X && f.Position.Y == position.Y
									 select f;
			List<Terrain> matches = q.ToList<Terrain>();
			if (matches.Count == 0) return false;
			return matches[0].Solid;
		}

		public Character CharacterAt(Point position) {
			IEnumerable<Character> q = from c in Characters
									 where c.Position.X == position.X && c.Position.Y == position.Y
									 select c;
			List<Character> matches = q.ToList<Character>();
			if (matches.Count == 0) return null;
			return matches[0];
		}

		public void Generate() {
			LoadFromFile("Map01.txt");
			foreach (Terrain t in Terrain) {
				if (t.Solid == false && Game.Current.RNG.Next(40) == 0) Character.Generate(Character.Prefab.Zombie, new Point(t.Position.X, t.Position.Y));
			}
			/*Character.Generate(Character.Prefab.Zombie, new Point(3, 3));
			Character.Generate(Character.Prefab.Zombie, new Point(7, 7));
			Character.Generate(Character.Prefab.Zombie, new Point(7, 9));
			Character.Generate(Character.Prefab.Zombie, new Point(9, 9));
			Character.Generate(Character.Prefab.Zombie, new Point(2, 3));
			Character.Generate(Character.Prefab.Zombie, new Point(1, 3));*/
		}

		public void LoadFromFile(string file) {
			Point pos = new Point(0, 0);
			Width = 0;
			using (StreamReader r = new StreamReader(file)) {
				string line;
				while ((line = r.ReadLine()) != null) {
					foreach (char c in line.ToCharArray().ToList<char>()) {
						switch (c) {
							case '#':
								Terrain.Add(new Terrain(pos, TerrainType.Wall));
								break;
							case '.':
								Terrain.Add(new Terrain(pos, TerrainType.Floor));
								break;
						}
						Width = Math.Max(Width, pos.X);
						pos.X++;
					}
					pos.X = 0;
					pos.Y++;
				}
			}
			Width++;
			Height = pos.Y;
			Map = new TCODMap(Width, Height);
			foreach (Terrain t in Terrain) {
				Map.setProperties(t.Position.X, t.Position.Y, t.Transparent, !t.Solid);
			}
		}
	}
}
