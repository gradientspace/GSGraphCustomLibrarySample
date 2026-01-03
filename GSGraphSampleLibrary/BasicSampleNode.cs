using Gradientspace.NodeGraph;

namespace MyGSGraphSampleLibrary
{
	public class MySampleNode : StandardNode
	{
		// this is the name of the Node in the UI. If it's not overridden, the class name is used.
		public override string GetDefaultNodeName() { return "SampleNode"; }
		// this is the namespace of the Node (ie similar to [NodeFunctionLibrary("SampleNodeLibrary")] for staticfunction node libraries)
		// If it's not provided, the class namespace will be used
		public override string? GetNodeNamespace() { return "SampleNodeLibrary"; }

		// These will be used as the names of the Input and Output Pins.
		// It's not strictly necessary to make them class members, but it is convenient in the code below
		// (they do not have to be static)
		public const string Input1Name = "StringPart";
		public const string Input2Name = "NumPart";
		public const string OutputName = "Combined";

		public MySampleNode()
		{
			// Standard nodes will have their pins configured in the constructor. The pins
			// can change later, but this can break the graph if it's not done carefully.

			// There are various standard INodeInput implementations that handle common cases.
			// The Name argument is used as the pin-name in the UI (note the INodeInput does not actually know this name!) 
			AddInput(Input1Name, new StandardStringNodeInput("base"));
			AddInput(Input2Name, new StandardNodeInputWithConstant<int>(7));

			// INodeOutputs are generally simpler as they don't have default values, so there
			// are only a few base classes and you aren't likely to need to create more
			AddOutput(OutputName, new StandardNodeOutput<string>());
		}

		// The Evaluate() function is called during graph evaluation. The Evaluator provides 
		// the various Input pin values in a NamedDataMap instance (this is basically a Dictionary).
		// Your Evaluate function needs to look up the necessary values (by Input pin name), then
		// do whatever your node is supposed to do based on those inputs, and then populate any
		// requested outputs
		public override void Evaluate(EvaluationContext EvalContext, ref readonly NamedDataMap DataIn, NamedDataMap RequestedDataOut)
		{
			// fetch Input data from NamedDataMap DataIn. There are various functions for finding values in the data map.
			string stringPart = DataIn.FindStringValueOrDefault(Input1Name, "", true);
			int numPart = 0;
			DataIn.FindItemValueStrict<int>(Input2Name, ref numPart, true);

			// compute the result of the node
			string result = $"{stringPart}_{numPart}";

			// Set the Output value(s). The code below is the "rightest" way to set an Output value,
			// which makes it clear that it may not have been requested (eg if the pin is not wired anywhere).
			// Note you could even consider this in your evaluation...
			int itemIndex = RequestedDataOut.IndexOfItem(OutputName);
			if (itemIndex >= 0)
				RequestedDataOut.SetItemValue(itemIndex, result);

			// this is a helper variant that will throw if the value is not found
			//RequestedDataOut.SetItemValueOrNull_Checked(OutputName, result);
			// this is another variant that won't throw
			//RequestedDataOut.SetItemValueOrNull_UnChecked(OutputName, result);
		}

	}
}
