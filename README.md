# Dart Parsing Tools
Originally this project was going to be a testing ground for writing a Dart compiler, hence the solution name.
I ended up wanting to go the route of making a general multipurpose program for working with dart parse trees.

## Using the program
You'll need the dotnet core runtime/sdk installed, which can be found [here](https://dotnet.microsoft.com/en-us/download).

After which, it _should_ be relatively simple to build the program by running the following commands:

```bash
# clone the repo
git clone git@github.com:thomasmaloney-wk/dart-parsing-tools.git

# cd into it
cd dart-parsing-tools

# run dotnet build
dotnet build
```

You can then run the program from the root of the repo with the command `dotnet run`.

### Example Use: mock file explosion
For example, if you have a file that contains a bunch of Dart mock class definitions defined at `~/some_dart_project/mocks.dart` and you wanted each one to be defined in its own dart file, you could use this program with the `--explode` flag like so:

```bash
dotnet run ~/some_dart_project/mocks.dart --explode
```
And the program will create a directory `~/mock_files/` and write each mock class to its own file in there.

If you'd like to specify an output directory for the mock files to be written to, you can do so by using the `-o` flag:

```bash
dotnet run ~/some_dart_project/mocks.dart --explode -o ~/some/other/directory
```


## Disclaimer
I wasn't initially expecting to put this on github and so documentation around using it or what each part does is not currently in an ideal state.
I'll get around to making it a bit more user friendly and updating documentation and whatnot shortly though.
