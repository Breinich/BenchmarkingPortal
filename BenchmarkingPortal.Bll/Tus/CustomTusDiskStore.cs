using MediatR;
using tusdotnet.Stores;

namespace BenchmarkingPortal.Bll.Tus;

public class CustomTusDiskStore : TusDiskStore
{
    public CustomTusDiskStore(string path, IMediator mediator) : 
        base(path, true, TusDiskBufferSize.Default, new CustomGuidProvider(mediator))
    { }
}