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
				Utils.WriteLine("Usage: perceptron [train] [tune] [test]");
				return;
			}

			for (int i = 0; i < 3; i++) {
				if (!File.Exists(args[0])) {
					Utils.WriteLine("'" + args[i] + "' does not exist.");
				}
			}	

			try {

				double[] w;
				DataReader train = new DataReader(args[0]);
				train.readData();

				DataReader tune = new DataReader(args[1]);
				tune.readData();

				DataReader test = new DataReader(args[2]);
				test.readData();
				
				
				net.targetAccuracy = 0.72;
				Utils.Write("Start training...");
				
				w = net.initialWeights(train.getExamples()[0]);
				net.train(w, train.getExamples(), tune.getExamples());
				double tuneAcc = net.accuracy(w, tune.getExamples());

				Utils.WriteLine("done.");
				Utils.WriteLine("Accuracy for tuning set: {0:F4}", tuneAcc);
				Utils.WriteLine("");
				

				Utils.WriteLine("{0,-10} {1,-10} {2,-10} {3,6}", "Name", "Predicted", "Actual", "Error");
				Utils.WriteLine("-------------------------------------------");

				for (int i = 0; i < test.getExamples().Count; i++) {
					Instance inst = test.getExamples()[i];
					double p = net.predict(w, inst);
					int pl = p < 0 ? 0 : 1;
					string predicted = test.getLabels().names[pl];
					string actual = test.getLabels().names[inst.label];
					bool correct = (pl == inst.label);
					Utils.WriteLine("{0,-10} {1,-10} {2,-10} {3,6}", inst.name, predicted, actual, (predicted == actual) ? "" : "x");					
				}

				Utils.WriteLine("-------------------------------------------");

				double testAcc = net.accuracy(w, test.getExamples());				
				Utils.WriteLine("Correctly classified : " + net.correctCount());
				Utils.WriteLine("Miss classified      : " + (test.getExamples().Count - net.correctCount()));
				Utils.WriteLine("Total test samples   : " + test.getExamples().Count);
				Utils.WriteLine("Accuracy for test set: {0:F4}", testAcc);

				Utils.WriteLine("");
				Utils.WriteLine("Learned weights:");
				net.printWeight(w);

			}
			catch (Exception ex) {
				Utils.WriteLine("Error reading data file:'" + args[0] + "'.\nDetails:" + ex.Message);
			}
		}
	}
}