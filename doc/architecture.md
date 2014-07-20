[<-- Home](https://github.com/hudl/HudlFfmpeg)

## Architecture

Hudl.Ffmpeg is architected to closely resemble the structure of an Ffmpeg command. Ffmpeg commands are broken down in to four very basic parts

* Filtergraph 
* Settings
* Inputs
* Outputs

### Filtergraph

Hudl.Ffmpeg uses the same terminology as Ffmpeg in regards to filters. Filters are best described as linear actions to take against one or more input streams. These filters are reprented by the ```IFilter``` interface in Hudl.Ffmpeg. Collections of filters in any given sequence are known as ```Filterchain```s. Finally the ```Filtergraph``` is a term used to describe a one or more ```Filterchain```s in an Ffmpeg command. 

Hudl.Ffmpeg Filtergraph hierarchy:
 
* Filtergraph
 * List<Filterchain> 
   * List<IFilter>

For more information on constructing filterchains, view the full [API Documentation](doc/api.md).

### Settings

Settings in Hudl.Ffmpeg are used to tell encoders / decoders how to interpret or convert the stream. These settings are represented by the ```ISetting``` interface in Hudl.Ffmpeg. Collections of settings are separated into two groups: input and output settings. Input and output settings are identified in Hudl.Ffmpeg by the use the ```SettingsApplication``` attribute. 

Hudl.Ffmpeg Settings hierarchy:

* SettingsCollection 
 * List<ISetting>

For more information on creating custom settings, view our [Extending Hudl.Ffmpeg](doc/custom.md) documentation.

For more information on constructing settings collections, view the full [API Documentation](doc/api.md).

### Inputs

An Input is a pointer to a video or audio stream. An input file is loaded in to an ```IContainer``` interface object. These ```IContainer``` objects are class types that represent the input file type. The classes are named by file extension, which means that a *.mp4* file would be loaded into a type of ```Mp4```. We do this so that container can be validated by what is put in and taken out of them. 

Hudl.Ffmpeg Input heirarchy: 
* CommandInput
 * SettingsCollection (see above)
 * IContainer
  * List<IStream>
   * VideoStream 
     - OR -
   * AudioStream

For more information on creating custom containers, view our [Extending Hudl.Ffmpeg](doc/custom.md) documentation.

For more information on inputs and containers, view the full [API Documentation](doc/api.md).

### Outputs

An Output is exactly as it sounds, it is where we are going to render the files to. Each ```FfmpegCommand``` object is allowed multiple ```CommandOutput``` types. These command output types container references to an ```IContainer``` object, which is the format in which the files is to be saved. 

Hudl.Ffmpeg Output heirarchy: 
* CommandOutput
 * SettingsCollection (see above)
 * IContainer
  * List<IStream>
   * VideoStream 
     - OR -
   * AudioStream

For more information on outputs and containers, view the full [API Documentation](doc/api.md).
