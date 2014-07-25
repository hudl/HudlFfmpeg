[<-- Home](https://github.com/hudl/HudlFFmpeg)

## Installation & Setup

Installation is fairly minimal - just grab Hudl.FFmpeg from NuGet (www.nuget.org) using your Package Manager GUI or Console.

### Configuration

Configuration is non-trivial, Hudl.FFmpeg sits on top of whatever version of FFmpeg you choose to run. This means that all you have to do is let Hudl.FFmpeg know where it is. Just place this in your application startup, or before you create any FFmpeg commands. 

```csharp
using Hudl.FFmpeg;

...

var outputPath = "c:/foo/";
var FFmpegPath = "c:/foo/FFmpeg.exe";
var FFprobePath = "c:/foo/FFprobe.exe";

ResourceManagement.CommandConfiguration = CommandConfiguration.Create(outputPath, FFmpegPath, FFprobePath);
```

View the full [API Documentation](doc/api.md) to see all configurable values. 

## Creating Your First Command

*All code snippets are done using the Hudl.FFmpeg Sugar syntax, for more information on Hudl.FFmpeg Sugar go [here](doc/sugar.md)* 

### Creating The CommandFactory

Hudl.FFmpeg uses CommandFactorys to create and manage all FFmpeg commands. This means you can create multiple commands and render them all at the same time. To create a factory is simple, and it is required to render with Hudl.FFmpeg.

```csharp 
using Hudl.FFmpeg.Sugar;
using Hudl.FFmpeg.Command; 

...

var commandFactory = CommandFactory.Create();
```

### Creating an FFmpegCommand

The CommandFactory sorts all FFmpeg commands into two different types of rendered output: 

* Resource, which is output you intend on using as temporary inputs in other commands. 
* Output, which is output you intend on keeping around. 

To learn more about these command types, view the full [API Documentation](doc/api.md).

To create a new command and add it to the factory using Hudl.FFmpeg Sugar 

```csharp
var command = commandFactory.CreateOutputCommand();
```

### Resource Types 

If is important to know that Hudl.FFmpeg input resources work on static file classes. We do this so that we can restrict and manage the correct settings and filters to use by stream types (video, image, audio). So each file extension would equate to a object type. The *.mp4* file extension, when specified, would be loaded into an ```Mp4``` object. 

Hudl.FFmpeg loads its resource types by searching for all instances of ```IContainer``` implementations. It is also smart enough to search your applications for any custom ```IContainer```s that you have created. Creating these objects are done for you automatically everytime that you specify a file type string. For example, one way to load a resource in Hudl.FFmpeg is 

```csharp
var foo = Resource.From("c:\foo\bar.mp4");
```

To learn more about these resource types, view the full [API Documentation](doc/api.md).

### Adding Inputs

*If you specified an FFprobe path, Hudl.FFmpeg will load the Metadata for the input as you add it to your command*

Adding inputs to your command is also easy with Hudl.FFmpeg. Using the ```.WithInput``` construct you can add your files just by specifying the path. 

```csharp
//It is important to specify a VideoStream or AudioStream when using the WithInput contstruct
//this tells Hudl.FFmpeg which stream you are wanting to work with up front.
var foo = command.WithInput<VideoStream>("C:\foo\bar-1.mp4")
                 .WithInput<VideoStream>("C:\foo\bar-2.mp4");
```

To learn more about adding inputs to FFmpeg commands, view the full [API Documentation](doc/api.md).

### Adding Filters

Hudl.FFmpeg Sugar uses a chain builder based approach to contructing commands. Each time you add an input, or filterchain a CommandStage object is built. These objects maintain a reference to the media streams you have added or filtered to. 

For example, in the above example we added two inputs *bar-1.mp4* and *bar-2.mp4*. The resulting stage command would contain a reference to both streams. If we were then to concatenate these files (like in the example below) the stage command would contain a reference to the concatenated stream. 

Each FFmpeg command has a Filtergraph, which is made up of filterchains. So to create a filterchain we can use the ```Filterchain.FilterTo<TStreamType>()``` construct. 

```csharp
using Hudl.FFmpeg.Sugar; 
using Hudl.FFmpeg.Filters;

...

//Filterchain construct accepts the following arguments: 
// - type constructor:     a Stream type class, required to classify the output stream as video or audio.
// - param IFilter[]:      a list of IFilter objects, these objects are named after the available FFmpeg filters. 
var filterchain = Filterchain.FilterTo<VideoStream>(new Concat());

var bar = foo.Filter(filterchain);
```

To learn more about filters, filterchains, and filtergraphs, view the full [API Documentation](doc/api.md).

### Mapping Output & Adding Settings

After adding inputs and fiters now it is time to declare an output object. Outputs are created by mapping the stream references in your CommandStage object to a file. This is done using the ```.MapTo<>()``` Sugar extension.

Adding settings is done along side of creating outputs, Each output defined can contain SettingsCollection which is a collection of input or output settings such as codec, frame rate, sample rate, etc.. These Settings 

```csharp
using Hudl.FFmpeg.Sugar; 
using Hudl.FFmpeg.Settings;

...

//SettingsCollection construct accepts the following arguments: 
// - param ISetting[]:      a list of ISetting objects, these objects are named after the available FFmpeg settings. 
var settings = SettingsCollection.ForOutput(new CodecVideo("libx264"));

var output = bar.MapTo<Mp4>("c:\foo\bar-3.mp4", settings); 
``` 

To learn more about outputs, and settings, view the full [API Documentation](doc/api.md).

### Rendering Files

Rendering the files in your CommandFactory is done with a single call using the ```Render()``` method. This call will iterate through the FFmpeg commands in your factory object, and send them to FFmpeg for processing. 

```csharp
factory.Render();
```

### Putting It All Together

As mentioned earlier, Hudl.FFmpeg Sugar uses the builder based approach to creating commands. All of the above code that we have gone through can be combined and simplified like so.

```csharp
using Hudl.FFmpeg.Sugar; 
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Commands; 
using Hudl.FFmpeg.Settings; 

...

var filterchain = Filterchain.FilterTo<VideoStream>(new Concat());

var outputSettings = SettingsCollection.ForOutput(new CodecVideo("libx264")); 

var commandFactory = CommandFactory.Create(); 

commandFactory.CreateOutputCommand()
              .WithInput<VideoStream>("c:\foo\bar-1.mp4")
              .WithInput<VideoStream>("c:\foo\bar-2.mp4")
              .Filter(filterchain)
              .MapTo<Mp4>("c:\foo\bar-3.mp4"); 

commandFactory.Render(); 
```