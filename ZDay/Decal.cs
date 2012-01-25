﻿using System;
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
		public int Density = 1;
		public int Life = 0;

		public static Decal Generate(Prefabs prefab, Point position) {
			IEnumerable<Decal> q;
			q = from decal in Area.Current.Decals
				where decal.Position == position
				select decal;
			List<Decal> matches = q.ToList<Decal>();
			if (matches.Count == 1) {
				if (prefab == Prefabs.BloodDrops || prefab == Prefabs.BloodSplatter || prefab == Prefabs.BloodPool) {
					matches[0].Density++;
					switch (matches[0].Type) {
						case Types.BloodDrops:
							if (matches[0].Density < 3) return null;
							Area.Current.Decals.Remove(matches[0]);
							return Generate(Prefabs.BloodSplatter, position);
						case Types.BloodSplatter:
							if (matches[0].Density < 8) return null;
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
					d.ForegroundColor = TCODColor.Interpolate(TCODColor.red, new TCODColor(58, 5, 14), 0f);
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

		public new void Update() {
			if (Life < 500) Life++;
			switch (Type) {
				case Types.BloodDrops:
				case Types.BloodSplatter:
					ForegroundColor = TCODColor.Interpolate(TCODColor.red, new TCODColor(58, 5, 14), Math.Min(1f, (float)Life / 500f));
					break;
				case Types.BloodPool:
					BackgroundColor = TCODColor.Interpolate(TCODColor.red, new TCODColor(58, 5, 14), Math.Min(1f, (float)Life / 500f));
					break;
			}
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
