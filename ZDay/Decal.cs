using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace ZDay {
	public class Decal : Entity {
		public enum DrawModes {
			Normal,
			OnlyForegroundColor,
			OnlyBackgroundColor
		}

		public enum Types {
			BloodDrops,
			BloodSplatter,
			BloodPool
		}

		public enum Prefabs {
			BloodDrops,
			BloodSplatter,
			BloodPool
		}

		public DrawModes DrawMode = DrawModes.Normal;
		public string Description;
		public Types Type;

		public static Decal Generate(Prefabs prefab, Point position) {
			IEnumerable<Decal> q;
			q = from decal in Area.Current.Decals
				where decal.Position == position
				select decal;
			List<Decal> matches = q.ToList<Decal>();
			if (matches.Count == 1) {
				if (prefab == Prefabs.BloodDrops || prefab == Prefabs.BloodSplatter || prefab == Prefabs.BloodPool) {
					switch (matches[0].Type) {
						case Types.BloodDrops:
							Area.Current.Decals.Remove(matches[0]);
							return Generate(Prefabs.BloodSplatter, position);
						case Types.BloodSplatter:
							Area.Current.Decals.Remove(matches[0]);
							return Generate(Prefabs.BloodPool, position);
						case Types.BloodPool:
							return null;
					}
				}
			}
			if (matches.Count > 1) throw new Exception("OMG!");
			Decal d = new Decal();
			switch (prefab) {
				case Prefabs.BloodDrops:
					d.Type = Types.BloodDrops;
					d.DrawMode = DrawModes.OnlyForegroundColor;
					d.ForegroundColor = TCODColor.desaturatedCrimson;
					d.Description = "There is some blood on the ground here.";
					break;
				case Prefabs.BloodSplatter:
					d.Type = Types.BloodSplatter;
					d.DrawMode = DrawModes.Normal;
					d.Symbol = (char)7;
					d.ForegroundColor = TCODColor.desaturatedCrimson;
					d.Description = "There is a splatter of blood here.";
					break;
				case Prefabs.BloodPool:
					d.Type = Types.BloodPool;
					d.DrawMode = DrawModes.OnlyBackgroundColor;
					d.BackgroundColor = TCODColor.desaturatedCrimson;
					d.Description = "A pool of blood covers the ground here.";
					break;
			}
			d.Area = Area.Current;
			d.Position = position;
			Area.Current.Decals.Add(d);
			return d;
		}

		public new void Draw(TCODConsole console, Point offset) {
			switch (DrawMode) {
				case DrawModes.Normal:
					base.Draw(console, offset);
					break;
					
				case DrawModes.OnlyForegroundColor:
					if (Position.X > offset.X && Position.X <= offset.X + 45 && Position.Y > offset.Y && Position.Y <= offset.Y + 45) {
						console.setCharForeground(Position.X - offset.X, Position.Y - offset.Y, ForegroundColor);
					}
					break;

				case DrawModes.OnlyBackgroundColor:
					if (Position.X > offset.X && Position.X <= offset.X + 45 && Position.Y > offset.Y && Position.Y <= offset.Y + 45) {
						console.setCharBackground(Position.X - offset.X, Position.Y - offset.Y, BackgroundColor);
					}
					break;
			}
			
		}
	}
}
