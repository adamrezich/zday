using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZDay {
	public class Console {
		public static List<ConsoleLine> Lines = new List<ConsoleLine>();

		public static void WriteLine(string text) {
			Lines.Add(new ConsoleLine(text));
		}
	}

	public class ConsoleLine {
		public string Text;

		public ConsoleLine(string text) {
			Text = text;
		}
	}
}
