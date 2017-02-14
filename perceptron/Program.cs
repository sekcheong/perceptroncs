using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace perceptron
{
	class Program
	{
		static void Main(string[] args)
		{
			NeuralNet net = new NeuralNet();

			if (args.Length < 3) {
				Console.WriteLine("Usage: perceptron [train] [tune] [test]");
				return;
			}

			for (int i = 0; i < 3; i++) {
				if (!File.Exists(args[0])) {
					Console.WriteLine("'" + args[i] + "' does not exist.");
				}
			}

			double[] bestWeight = null;
			double bestRate = 0;
			double[] w;

			try {
				DataReader train = new DataReader(args[0]);
				train.readData();

				DataReader tune = new DataReader(args[1]);
				tune.readData();

				DataReader test = new DataReader(args[2]);
				test.readData();

				int maxTry = 5;

				for (int i = 0; i < maxTry; i++) {
					w = net.initialWeights(train.getExamples()[0]);
					net.train(w, train.getExamples(), tune.getExamples());
					double trainAcc = net.accuracy(w, tune.getExamples());
					if (trainAcc > bestRate) {
						bestWeight = w;
						bestRate = trainAcc;
					}
					Console.WriteLine("Iteration {0} of {1} : Accuracy: {2,N4}", i + 1, maxTry, bestRate);
				}

				Console.WriteLine("{0,10} {1,10} {2,10} {3,10}\n", "Name", "Predicted", "Actual", "Error");
				Console.WriteLine("-------------------------------------------\n");
				for (int i = 0; i < test.getExamples().Count; i++) {
					Instance inst = test.getExamples()[i];
					double p = net.predict(bestWeight, inst);
					int pl = p < 0 ? 0 : 1;
					string predicted = test.getLabels().names[pl];
					string actual = test.getLabels().names[inst.label];
					bool correct = (pl == inst.label);
					Console.WriteLine("{0,10}{1,10}{2,10}{3,10}", inst.name, predicted, actual, (predicted == actual) ? "" : "x");
				}
				Console.WriteLine("-------------------------------------------");

				double acc = net.accuracy(bestWeight, test.getExamples());
				Console.WriteLine("Accuracy:" + acc);
				Console.WriteLine("Learned weights:");
				net.printWeight(bestWeight);
			}
			catch (Exception ex) {
				Console.WriteLine("Error reading data file:'" + args[0] + "'.\nDetails:" + ex.Message);
			}
		}
	}
}
