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
		public enum Prefabs {
			BloodDrops,
			BloodSplatter,
			BloodPool
		}

		public DrawModes DrawMode = DrawModes.Normal;

		public static Decal Generate(Prefabs prefab, Point position) {
			switch (prefab) {
				case Prefabs.BloodDrops:
					Decal d = new Decal();
					d.DrawMode = DrawModes.OnlyForegroundColor;
					d.ForegroundColor = TCODColor.desaturatedCrimson;
					d.Area = Area.Current;
					d.Position = position;
					Area.Current.Decals.Add(d);
					break;
			}
			return null;
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
						console.setCharBackground(Position.X - offset.X, Position.Y - offset.Y, ForegroundColor);
					}
					break;
			}
			
		}
	}
}
