using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes; 
using Hudl.Ffmpeg.Resources.BaseTypes; 

namespace Hudl.Ffmpeg.BaseTypes
{
    public interface IProject<TypeO> : IFilterable
        where TypeO : IResource, new() 
    {
        IReadOnlyList<IResource> Resources { get; }

        IProject<TypeO> Add<TypeA>(TypeA resource)
            where TypeA : IResource;

        IProject<TypeO> Insert<TypeA>(TypeA resource, int index)
            where TypeA : IResource;

        IProject<TypeO> RemoveAt(int index);

        TimeSpan GetLength(); 

        TypeO Render<TypeP>()
            where TypeP : ICommandProcessor, new();
    }
}
