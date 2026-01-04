This is a sample project that can give you a starting point for creating 
custom Node Libraries for the Gradientspace NodeGraph (GSGraphEditor/etc).

This sample has only (currently) been tested with Visual Studio 2022.
It may work with VSCode as well, but (currently) the csproj setup reads
from the Windows registry, and so on other platforms you would have to
change that to be an explicit path (instructions are below)



Project Code
============

The actual code to create custom nodes is pretty minimal. There are two
types of custom nodes - those created via static functions, and those
created by subclassing the base node class.

In SampleNodeFunctions.cs you will find a few examples of static-function Nodes.
This is the simplest way to create custom nodes, and probably the best option
unless you have a more complex use case that requires an actual Node class.

BasicSampleNode.cs contains a simple Node class (and is extensively commented).



Project Setup
=============

The idea with this project is you can build/run/debug your custom node library 
with an existing GSGraphEditor installation. To do this, the location of the 
installation is needed in the .csproj file. The default setup is to get this
path from the Windows registry (the installer writes a registry key there).

If you would like to hardcode the path, you can change the "GSGraphInstallDir"
variable value in the .csproj file (in a text editor). Note that this is
the top-level install folder (which defaults to C:\Program Files\Gradientspace\GSGraph\).
Do not include the 'GSGraphEditor' subdirectory in this path.

The dependent DLLs (found in the installation folder) are referenced based on
that csproj variable, so if they can't be found or won't load, likely the variable
is set incorrectly.


Running/Debugging
==================

To run with the installed GSGraphEditor.exe, you should be able to select the "GSGraphEditor"
Debug Launch Profile and simply do a Start Debugging (eg F5, green triangle in VS2022, etc). 
This also uses the $(GSGraphInstallDir) variable provided by the csproj to locate the EXE.
If it doesn't work, you can try editing the profile to hardcoded the EXE path.

The compiled NodeLibrary dll path (ie your code) is provided to GSGraphEditor.exe using a
command-line argument "-AddNodeLibraryPath=$(TargetDir)". It should be the case that TargetDir
is something like (path to .csproj)\bin\Debug\net8.0, and that's where the compiled C# DLL
will be placed by the build (eg MyGSGraphSampleLibrary.dll by default). 
GSGraphEditor will auto-load the NodeLibrary DLL given that path.

While running in debug like this, you will be able to edit your C# code and hot-reload.
Note however, if you change the Node definitions, or add/remove nodes, the changes will 
not be automatically picked up by hot-reload. You can use the 'Refresh Node Libraries' 
command inside GSGraphEditor to make that happen, currently (however in some cases 
you might encounter issues w/ duplicate nodes, etc - if weird stuff happens, just restart the app).


Publishing
===========

Every time you build, the built DLL is also copied to (SolutionDir)\CustomNodeLibraries\(ProjectName)\.
This is the folder meant to be used with GSGraphEditor when you *aren't* writing/debugging your custom
node library. Eg the subfolder could be copied to the 'AdditionalNodeLibraries' folder in the ProgramFiles
installation, or another location that can be added as permanent NodeLibrary Path in your GSGraphEditor
settings (eg via the Node Libraries dialog in the app).

You can also add the (SolutionDir)\CustomNodeLibraries\ as a NodeLibrary Path in the App (ie stored in the
NodeLibraryPaths list in the editor_config.json file). Then when you run the GraphEditor standalone, the
last-built DLL will be loaded from that location. When you run from this Debug project, the version specified
on the command-line will supercede that version (because those paths are checked first, and duplicate DLLs are
ignored). 

Note that any .gg files you save when working in Debug mode will have references to the /bin/Debug/ DLL paths.
The graph loader will attempt to load those assemblies, which will fail (due to the duplicate names). This 
should all work fine, but it will print some log spam.

If you want to disable this copying, comment-out the "PublishDlls" Target in the .csproj file
(or you can also change the target path there).

