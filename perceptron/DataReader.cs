using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace perceptron
{
	public class DataReader
	{
		private enum State
		{
			READ_FEATURE_NUMBER,
			READ_FEATURE_VALUE,
			READ_CLASS_LABELS,
			READ_EXAMPLE_COUNT,
			READ_EXAMPLES
		}

		private StreamReader _buffReader;
		private Labels _labels;
		private List<Feature> _features;
		private List<Instance> _examples;

		public DataReader(string fileName)
		{
			try {
				FileStream freader = File.Open(fileName, FileMode.Open);   //new FileReader(fileName);
				_buffReader = new StreamReader(freader);

			}
			catch (Exception ex) {
				Console.WriteLine("Error:" + ex.Message);
			}
		}

		private Feature readOneFeature(string feature)
		{
			string[] parts = feature.Split(new char[] { '-' });

			if (parts.Length != 2) {
				throw new Exception("Invalid feature format.");
			}
			if (parts[0].Length == 0) {
				throw new Exception("Feature name is null.");
			}

			string[] features = parts[1].Split(new char[] { ' ' });
			List<string> values = new List<string>();

			for (int i = 0; i < features.Length; i++) {
				string s = features[i].Trim();
				if (s.Length > 0) {
					values.Add(s);
				}
			}

			Feature f = new Feature();
			f.name = parts[0];
			f.values = values;
			return f;
		}

		private Instance readOneInstance(String line, Labels lbls, List<Feature> features)
		{
			line = line.Trim();
			line = Regex.Replace(line, "\\s+", " ");

			string[] pieces = line.Split(new char[] { ' ' });
			Instance instance = new Instance();
			instance.name = pieces[0];
			instance.label = lbls.indexOfLabel(pieces[1]);
			// instance.features = new int[pieces.length - 2];
			int[] x = new int[pieces.Length - 2];
			for (int i = 0; i < x.Length; i++) {
				x[i] = features[i].indexOfValue(pieces[i + 2]);
			}
			instance.setFeatures(x);
			return instance;
		}

		public void readData()
		{
			string line = "";
			State readState = State.READ_FEATURE_NUMBER;

			int numFeature = 0;
			int numExamples = 0;
			int featuresRead = 0;
			int examplesRead = 0;

			Feature feature = null;
			Instance instance = null;

			List<Feature> features = new List<Feature>();
			Labels labels = new Labels();
			labels.names = new List<String>();
			List<Instance> examples = new List<Instance>();

			bool backup = false;
			bool done = false;

			try {
				while (!done) {

					if (!backup) {
						line = _buffReader.ReadLine();
						if (line == null) {
							break;
						}
					}

					// skip commented lines;
					if (line.Trim().StartsWith("//")) continue;

					line = line.Trim();
					if (line.Length == 0) continue;

					switch (readState) {

						case State.READ_FEATURE_NUMBER:
							numFeature = int.Parse(line);
							// System.out.println("# of feature" + numFeature);
							readState = State.READ_FEATURE_VALUE;
							break;

						case State.READ_FEATURE_VALUE:
							feature = readOneFeature(line);
							features.Add(feature);
							featuresRead++;
							if (featuresRead == numFeature) {
								readState = State.READ_CLASS_LABELS;
							}
							break;

						case State.READ_CLASS_LABELS:

							if (Regex.IsMatch(line, "^-?\\d+$")) {
								numExamples = int.Parse(line);
								readState = State.READ_EXAMPLES;
								break;
							}
							else {
								// System.out.println("label:" + line);
								labels.names.Add(line);
							}
							break;

						case State.READ_EXAMPLE_COUNT:

							break;

						case State.READ_EXAMPLES:
							if (examplesRead == numExamples) done = true;
							instance = readOneInstance(line, labels, features);
							examples.Add(instance);
							examplesRead++;
							break;

						default:
							break;

					}
				}

				this._features = features;
				this._labels = labels;
				this._examples = examples;

			}
			catch (Exception ex) {
				Console.WriteLine("Error:" + ex.Message);
			}
		}

		public List<Feature> getFeatures()
		{
			return this._features;
		}

		public Labels getLabels()
		{
			return this._labels;
		}

		public List<Instance> getExamples()
		{
			return this._examples;
		}

		void createTrainSet()
		{
			Dictionary<int, List<Feature>> classes = new Dictionary<int, List<Feature>>();
			for (int i = 0; i < this._labels.names.Count; i++) {
				classes.Add(i,  new List<Feature>());
			}
			// classes.get
			// Map <String, List<Feature>> classes = new Map<String,
			// List<Feature>>();
			// this._labels.names.size();
			// List<Feature> positive = new ArrayList<Feature>();
			// List<Feature> positive = new ArrayList<Feature>();
		}

	}
}
