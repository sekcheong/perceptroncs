using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace perceptron
{
	public class Feature
	{
		public string name;
		public List<string> values;

		public int indexOfValue(string valueName)
		{
			for (int i = 0; i < values.Count; i++) {
				if (values[i] == valueName) {
					return i;
				}
			}
			return -1;
		}
	}
}
