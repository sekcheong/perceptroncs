using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace perceptron
{
	public class Labels
	{
		public List<string> names;

		public int indexOfLabel(string labelName) {
			for (int i = 0; i < names.Count; i++) {
				if (names[i]==labelName) {
					return i;
				}
			}
			return -1;
		}

	}
}
