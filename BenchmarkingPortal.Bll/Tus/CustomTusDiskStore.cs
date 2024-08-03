using tusdotnet.Stores;

namespace BenchmarkingPortal.Bll.Tus;

public class CustomTusDiskStore(string path)
    : TusDiskStore(path, true, TusDiskBufferSize.Default, new CustomGuidProvider());