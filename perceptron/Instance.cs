using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace perceptron
{
	public class Instance
	{
		public string name;
		public int label;

		public void setFeatures(int[] features)
		{
			x = new double[features.Length + 1];
			x[0] = 1;  //set the bias value
			for (int i = 0; i < features.Length; i++) {
				x[i + 1] = features[i];
			}
		}

		public double[] x = null;
	}
}
