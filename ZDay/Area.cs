using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZDay {
	class Area {
		public List<Terrain> Terrain = new List<Terrain>();

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
