using Gradientspace.NodeGraph;
using g3;

// the Namespace is only for your code, and is not used by the NodeGraph
// except indirectly via Type references. For static [NodeFunctionLibrary]
// classes, the node functions are referenced by the [NodeFunctionLibrary]
// name, not the C# namespace
namespace MyGSGraphSampleLibrary
{
	// A public static class with a [NodeFunctionLibrary] attribute define Node Libraries.
	// Classes with this attribute will be inspected to see if they contain any NodeFunctions
	// by the NodeGraph library loader.
	// The Nodes will appear under "SampleNodeLibrary" in the Graph Editor node lists
	// (the name is treated like a namespace, eg it could be SampleNodeLibrary.TestNodes, etc)
	[NodeFunctionLibrary("SampleNodeLibrary")]
	public static class MySampleNodeFunctions
    {
		// A public static function with a [NodeFunction] attribute defines a Node Function.
		// This function will appear as a placeable/executable Node in the Graph Editor.
		// The return-type and arguments of the function define the input/output pins of the node.
		// When the node is evaluated, this static function will be called (via C# reflection)
		[NodeFunction]
		public static void MessageLogTest(string LogString = "hello world!")
		{
			GlobalGraphOutput.AppendLine($"This is a log message: {LogString}!");
			//GlobalGraphOutput.AppendLog(  "    (this message only appears in the Log tab...)");
			//GlobalGraphOutput.AppendError("    (this message is an Error...)");
		}

		// [TODO] more examples with different NodeFunction

		[NodeFunction]
		public static void MeshOperationTest(ref DMesh3 Mesh, double TranslateX)
		{
			MeshTransforms.Translate(Mesh, TranslateX, 0, 0);
		}
	}
}
