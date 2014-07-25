[<-- Home](https://github.com/hudl/HudlFFmpeg)

## Hudl.FFmpeg Sugar Syntax

The Hudl.FFmpeg sugar syntax is a library that extends the base functionality of Hudl.FFmpeg to make it even easier to use. Using the ```Hudl.FFmpeg.Sugar``` namespace add functionality to the base objects, allowing developers to interact with Hudl.FFmpeg in as few of lines as possible. 

The Sugar syntax is written in a builder based fashion, similar to LINQ. Sugar allowes you to 

* Create new FFmpeg commands.
* Select and modify media streams. 
* Add filters and settings to your commands. 
* Load container/stream metadata with one line using FFprobe.

### Load Metadata with FFprobe

You can only go so far working with video and audio streams without needing to know things like duration, frame rate, sample rate, and dimensions. That is why Hudl.FFmpeg has a built in FFprobe parser that will load the files you are working with and give you information about each stream. Any time you would like to load metadata for an ```IContainer``` do the following.

```csharp
//To load the container use the Resource.From("") construct, it will return an IContainer.
// - Sugar syntax extends the container object with a method known as LoadMetadata(), this
//   activates the FFprobe interaction and parser 

var foo = Resource.From("c:\foo\bar.mp4").LoadMetadata(); 

//The container will contain List<IStream> objects that will contain the metadata for each 
//type of stream in the file. 
// - VideoStream : IStream  - represents a video stream found in a container object.
// - AudioStream : IStream  - represents an audio stream found in a container object.

var bar = foo.Streams.OfType<VideoStream>()[0].Info;
```

For more information on streams and metadata, view the full [API Documentation](doc/api.md).

### Working with CommandStage

The basis of video editing is working with the video and audio streams in a command. The ```CommandStage``` object is the working area that brings the streams and the commands together. A ```CommandStage``` object acts as a staging area for developers. Sugar syntax allows you to select the streams that you would like to work with, by putting them into a ```CommandStage``` object. Below is an example of what a command stage can look like after streams have been selected.

```csharp
//command contains the following inputs: 
// - Input #0         -> c:\foo\bar-1.mp4
//   - stream:video
//   - stream:audio
// - Input #1          -> c:\foo\bar-2.mp4
//   - stream:video
//   - stream:audio

var foo = comand.Select<VideoStream>(0)
                .Select<VideoStream>(1);

//foo contains the following streams:
// - stream #0:video   -> c:\foo\bar-1.mp4
// - stream #1:video   -> c:\foo\bar-2.mp4
```

Now that these streams are selected, you can add filterchains and create output files from the selected streams.

For more information on streams, view the full [API Documentation](doc/api.md).

### Adding Inputs

We will say that now you have your command and you are ready to add your inputs. There are two ways to do this with the Sugar syntax. The first way we will talk about is the simple ```AddInput``` command. Any time that you add an input using the ```AddInput``` command it will automatically load the container and stream metadata. 

```csharp
//you can add input by specifying a file name
command.AddInput("c:\foo\bar-1.mp4");

//you can add input by specifying a file name, and an input SettingsCollection
var settings = SettingsCollection.ForInput(new StartAt(1));

command.AddInput("c:\foo\bar-2.mp4", settings);

//you can also add an input and tell Hudl.FFmpeg not to load metadata
command.AddInputNoLoad("c:\foo\bar-3.mp4", settings);
```

Hudl.FFmpeg has yet another way to add input that further simplifies working with the stream. This other method is with the ```WithInput<T>``` command. This command does everything that ```AddInput``` would do, but also selects the input stream for editing within a ```CommandStage``` object. The Type constraint ```T``` tells Hudl.FFmpeg which stream within the file you would like to work with. The options are ```VideoStream``` or ```AudioStream```. 

```csharp
//you can add input, and select the VideoStream for editing by specifying
var foo1 = command.WithInput<VideoStream>("c:\foo\bar-1.mp4");

//If you wanted to select more streams for editing, chain the commands together.
var foo2 = command.WithInput<VideoStream>("c:\foo\bar-2.mp4")
                  .WithInput<VideoStream>("c:\foo\bar-3.mp4")
                  .WithInput<VideoStream>("c:\foo\bar-4.mp4");

//command contains the following inputs: 
// - c:\foo\bar-1.mp4
// - c:\foo\bar-2.mp4
// - c:\foo\bar-3.mp4
// - c:\foo\bar-4.mp4

//foo1 contains the following:
// - stream #0:video   -> c:\foo\bar-1.mp4

//foo2 contains the following:
// - stream #0:video   -> c:\foo\bar-2.mp4
// - stream #1:video   -> c:\foo\bar-3.mp4
// - stream #2:video   -> c:\foo\bar-4.mp4
```

For more information on streams, view the full [API Documentation](doc/api.md).

### Selecting Video Streams

It is also possible to select video streams that have already been added to the command. Selecting streams adds them to a ```CommandStage``` object. This can be done a number of ways

```csharp
//by specifying the input index, the stream is intelligently selected.
var foo1 = command.Select(0); 

//foo1 contains the following:
// - stream #1:video   -> c:\foo\bar-1.mp4

//by specifying the input index, and stream type
var foo2 = command.Select<AudioStream>(0);

//foo2 contains the following:
// - stream #1:audio   -> c:\foo\bar-1.mp4

//by specifying the any number of StreamIdentifier objects
var foo3 = command.Select(streamId1, streamId2, streamId3);

//foo3 contains the following:
// - stream #0:video   -> streamId1
// - stream #1:video   -> streamId2
// - stream #2:video   -> streamId3

//by specifying that you want all input streams
var foo4 = command.SelectAll();

//foo4 contains the following:
// - stream #0:audio   -> c:\foo\bar-1.mp4
// - stream #0:video   -> c:\foo\bar-1.mp4
// - stream #1:audio   -> c:\foo\bar-2.mp4
// - stream #1:video   -> c:\foo\bar-2.mp4
// - stream #2:audio   -> c:\foo\bar-3.mp4
// - stream #2:video   -> c:\foo\bar-3.mp4
```

For more information on streams, view the full [API Documentation](doc/api.md).

### Adding Filters 

One of the most common things to do when you are working with video and audio is to add filters. That is where the ```CommandStage``` object and Sugar syntax really helps you out. Using the ```FilterEach``` and ```Filter``` constructs add an easy way build your Filterchians in line. 

```csharp
//foo contains the following:
// - stream #0:video   -> c:\foo\bar-1.mp4
// - stream #1:video   -> c:\foo\bar-2.mp4
// - stream #2:video   -> c:\foo\bar-3.mp4

var filterchain1 = Filterchain.FilterTo<VideoStream>(new Scale(852, 480));

var foo2 = foo.FilterEach(filterchain1);

//foo2 contains the following:
// - stream #0:video   -> c:\foo\bar-1.mp4 scaled to 852x480
// - stream #1:video   -> c:\foo\bar-2.mp4 scaled to 852x480
// - stream #2:video   -> c:\foo\bar-3.mp4 scaled to 852x480

var filterchain2 = Filterchain.FilterTo<VideoStream>(new Concat());

var foo3 = foo2.Filter(filterchain2);

//foo3 contains the following:
// - stream #0:video   -> stream #0:video + 
//                        stream #1:video + 
//                        stream #2:video 

//All of the above can also be chained into a single command by doing the following
var foo3 = foo.FilterEach(filterchain1)
              .Filter(filterchain2);
```

For more information on filterchains, view the full [API Documentation](doc/api.md).

### Creating Output

After adding all inputs and/or filters to the command, odds are you are going to want to create and output. Using the sugar Syntax all you have to do is use the ```.MapTo<T>()``` or ```To<T>``` constructs. The difference in these constructs is that ```To<T>``` will create the output file without Mapping the streams. The ```MapTo<T>``` construct will create the output file and Map all streams in the ```CommandStage``` object. 

```csharp
//foo3 contains the following:
// - stream #0:video   -> stream #0:video + 
//                        stream #1:video + 
//                        stream #2:video 

//(w/mapping) specifying an empty map object will generate a random named file in the output directory.
foo3.MapTo<Mp4>();

//(w/mapping) specify a file name.
foo3.MapTo<Mp4>("c:\foo\bar.mp4");

//(w/mapping) specify a file name, and settings
var settings = SettingsCollection.ForOutput(new CodecVideo("libx264"));

foo3.MapTo<Mp4>("c:\foo\bar.mp4", settings);

//(w/o mapping) specifying an empty map object will generate a random named file in the output directory.
foo3.To<Mp4>();

//(w/o mapping) specify a file name.
foo3.To<Mp4>("c:\foo\bar.mp4");

//(w/o mapping) specify a file name, and settings
var settings = SettingsCollection.ForOutput(new CodecVideo("libx264"));

foo3.To<Mp4>("c:\foo\bar.mp4", settings);
```

For more information on outputs, view the full [API Documentation](doc/api.md). 