# Eliza+
A cross-platform (RF5) Rune Factory 5 Framework and Save Editor.
Forked from the original [Eliza project](https://github.com/RF5-Research/Eliza/).

## Building without Visual Studio
1. Install the [.NET 5.0+ SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Download the repository by clicking the ``Clone`` dropdownbox and clicking either ``Download Zip`` or ``Open with Github Desktop``.
3. Extract the raw contents of the repository. 
4. Open a console (command prompt, git bash, etc.) in the repository folder, and run ``dotnet publish [TargetPlatform] -c Release`` with ``[TargetPlatform]`` being either ``Eliza.Windows`` (Windows), ``Eliza.Linux`` (Linux), or ``Eliza.Mac`` (MacOS) to build the binary.

# Building with Visual Studio
1. Install the [.NET 5.0+ SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Git clone the repository
3. Open the solution file. Find the project for your OS under Platforms/. Right click the project and click 'Build'.

# Development
1. Git clone the repository.
2. Install the [Visual Studio](https://visualstudio.microsoft.com/) IDE.
3. From Visual Studio Installer, install the packages for ASPNET desktop development.
4. Install the [.NET 5.0+ SDK](https://dotnet.microsoft.com/download/dotnet/5.0) if you haven't already.
5. Finally, install the Avalonia for Visual Studio [extension](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaforVisualStudio). Avalonia is a cross-platform UI framework for .NET Core. Eliza uses Avalonia because Microsoft's [MAUI](https://github.com/dotnet/maui) targets .NET Core 6.0, and it's still very early in their release roadmap, so the API is unlikely to be stable.

* When creating new projects, make sure the target platform is .NET Core 5.0, and not .NET Framework. Targeting .NET Core is necessary for running multi-platform.
* Eliza.UI is built using the Avalonia UI framework. When run in debug mode, with the application in focus, hitting F12 will bring up Dev Tools.

# Unit tests
1. Just run all the tests in Eliza.Test.

In this project, the basis for implementing consistency and equality checks on complex types or objects, relies on serializing to the exact same file down to each byte. Therefore the serializers are most thoroughly-tested, because they are required for all other unit tests to work.

If a change breaks a test, the order to fix things is by making sure BaseSerializer --> BinaryDeserializer --> GraphDeserializer --> everything else works, in that order. Use the python utils to decrypt the output files from BinarySerializer and feed them into 010-Editor, and run through the files using the hex templates available in the Eliza.Test.


## Maintainers
- [SinsofSloth](https://github.com/SinsofSloth)

## Credits and Thanks
- [SPICA](https://github.com/gdkchan/SPICA) for base for serialization
- Blazagon for name data
- bluedart and other testers
