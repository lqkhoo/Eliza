# Eliza+
A cross-platform (RF5) Rune Factory 5 Save Editor.
Forked from the original [Eliza project](https://github.com/RF5-Research/Eliza/) as the maintainer (Sloth) was busy with college work. I needed a robust save editor to work on the [Equipment Calculator](https://github.com/lqkhoo/rf-calculator).

I recently dug this old fork out of the bowels of my old mothballed workstation, so I'm archiving this in case anyone finds it useful. I put on a more usable GUI and fixed a couple of serialization errors and bugs. That's all I remember.

# Structure
1. `Eliza` contains core serialization, crypto, model, and data files.
2. `Eliza.Avalonia` is the UI component.
3. `Eliza.UI` contains legacy classes from the original Eliza.
4. `Eliza.Test` contains unit tests for the C# part of Eliza.
5. `Eliza.PythonUtils` contains standalone Python scripts for development work.

## Building without Visual Studio
1. Install the [.NET 5.0+ SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Download the repository by clicking the ``Clone`` dropdownbox and clicking either ``Download Zip`` or ``Open with Github Desktop``.
3. Extract the raw contents of the repository. 
4. Open a console (command prompt, git bash, etc.) in the repository folder, and run ``dotnet publish [TargetPlatform] -c Release`` with ``[TargetPlatform]`` being either ``Eliza.Windows`` (Windows), ``Eliza.Linux`` (Linux), or ``Eliza.Mac`` (MacOS) to build the binary.

# Building with Visual Studio
1. Install the [.NET 5.0+ SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Git clone the repository.
3. Open the solution file. Find the project for your OS under Platforms/. Right click the project and click 'Build'.

# Development
1. Git clone the repository.
2. Install the [Visual Studio](https://visualstudio.microsoft.com/) IDE.
3. From Visual Studio Installer, install the packages for ASPNET desktop development.
4. Install the [.NET 5.0+ SDK](https://dotnet.microsoft.com/download/dotnet/5.0) if you haven't already.
5. Finally, install the Avalonia for Visual Studio [extension](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaforVisualStudio). Avalonia is a cross-platform UI framework for .NET Core. Eliza uses Avalonia because Microsoft's [MAUI](https://github.com/dotnet/maui) targets .NET Core 6.0, and it's still very early in their release roadmap, so the API is unlikely to be stable.

* When creating new projects, make sure the target platform is .NET Core 5.0, and not .NET Framework. Targeting .NET Core is necessary for running multi-platform.
* Eliza.UI is built using the Avalonia UI framework. When run in debug mode, with the application in focus, hitting F12 will bring up Dev Tools.
* Image data is in the Eliza namespace. In order for Eliza.Avalonia to be able to access them, we need to declare images' build action as 'EmbeddedResource'. This is currently done via a declaration in the Eliza.csproj file that registers all png files as such. Then, from the Eliza.Avalonia axaml file, the path to the image would be `resm:Eliza.Data.Images.(pathToYourImage.png)?assembly=Eliza`.

Eliza.PythonUtils requires the following:
1. Python 3.5+ for type hint support. Regular cPython will do. Last tested with Anaconda 3.9.
2. [construct](https://github.com/construct/construct) for declarative low-level serialization.
3. [py3rijndael](https://github.com/meyt/py3rijndael) for crypto work.

# Running from Visual Studio
1. From the Solution Explorer, right click Eliza.Avalonia and set click 'Set as Startup Project'.
2. Then from the menu bar at the top, click the play button that should now be labeled as 'Eliza.Avalonia'.

# Unit tests
1. Just run all the tests in Eliza.Test.
In this project, the basis for implementing consistency and equality checks on complex types or objects, relies on serializing to the exact same file down to each byte. Therefore the serializers are most thoroughly-tested, because they are required for all other unit tests to work.
If a change breaks a test, the order to fix things is by making sure BaseSerializer --> BinaryDeserializer --> GraphDeserializer --> [everything else] works, in that order. Use the python utils to decrypt the output files from BinarySerializer and feed them into 010-Editor, and run through the files using the hex templates available in the Eliza.Test.

Python unit tests are in test.py.

## Maintainers
- [SinsofSloth](https://github.com/SinsofSloth)

## Credits and Thanks
- [SPICA](https://github.com/gdkchan/SPICA) for base for serialization
- Blazagon for name data
- bluedart and other testers
