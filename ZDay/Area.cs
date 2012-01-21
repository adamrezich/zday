﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZDay {
	class Area {
		public static Area Current;
		public List<Terrain> Terrain = new List<Terrain>();

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
			Character.Generate(Character.Prefab.Zombie, new Point(3, 3));
		}

		public void LoadFromFile(string file) {
			Point pos = new Point(0, 0);
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
						pos.X++;
					}
					pos.X = 0;
					pos.Y++;
				}
			}
		}
	}
}
