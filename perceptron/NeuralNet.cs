using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace perceptron
{
	public class NeuralNet
	{
		public double eta = 0.0001;
		public double targetAccuracy = 0.760;

		public double[] initialWeights(Instance instance)
		{

			int size = instance.x.Length;
			double w = Math.Sqrt(1 / ((double)size));

			Random rand = new Random();

			double[] weights = new double[size];

			for (int i = 0; i < weights.Length; i++) {
				weights[i] = rand.NextDouble() * w;
			}

			weights[0] = 1;
			return weights;
		}

		public double dotProduct(double[] w, double[] x)
		{
			double result = 0;
			for (int i = 0; i < w.Length; i++) {
				result = result + w[i] * x[i];
			}
			return result;
		}

		public void printWeight(double[] weights)
		{
			Utils.Write("w = [");
			for (int i = 0; i < weights.Length; i++) {
				Utils.WriteLine("{0:F8} ", weights[i]);
			}
			Utils.WriteLine("]");
		}

		private double signum(double f)
		{
			if (f == double.NaN) return f;
			if (f == 0) return 0;
			if (f < 0) return -1;
			if (f > 0) return 1;
			return f;
		}

		public void train(double[] weights, List<Instance> examples, List<Instance> tune)
		{
			Instance inst;
			double y_hat;
			//double delta_w;
			double fx;
			double y;
			double error;

			double maxAcc = 0;

			int maxloop = 100;
			int p = 0;
			int q = 0;
			bool stop = false;

			// save the best weights we have seen so far
			double[] bestWeights = new double[weights.Length];

			for (int n = 0; n < maxloop; n++) {
				for (int i = 0; i < examples.Count; i++) {
					inst = examples[i];

					// compute the predicted value
					y_hat = (dotProduct(weights, inst.x));

					fx = signum(y_hat);
					y = (inst.label == 0) ? -1.0 : 1.0;
					error = y - fx;

					// update the weight
					for (int k = 0; k < weights.Length; k++) {
						weights[k] = weights[k] + eta * error * inst.x[k];
					}

					// check the accuracy
					double acc = accuracy(weights, tune);
					if (acc > maxAcc) {
						p = n;
						q = i;
						maxAcc = acc;
						bestWeights = weights;
						if (maxAcc > this.targetAccuracy) {
							stop = true;
							break;					
						}
					}					
				}
				if (stop) break;
			}

			for (int i = 0; i < weights.Length; i++) {
				weights[i] = bestWeights[i];
			}
		}

		public double accuracy(double[] weights, List<Instance> test)
		{
			int correct = 0;
			for (int i = 0; i < test.Count; i++) {
				Instance inst = test[i];
				double y_hat = predict(weights, inst);
				double y = (inst.label == 0) ? -1.0 : 1.0;
				if (y == y_hat) {
					correct++;
				}
			}
			double rate = ((double)correct) / test.Count;
			return rate;
		}

		public double predict(double[] weights, Instance instance)
		{
			double y_hat = dotProduct(weights, instance.x);
			double fx = signum(y_hat);
			return fx;
		}

	}
}
