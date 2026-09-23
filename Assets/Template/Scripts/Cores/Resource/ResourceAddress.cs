using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.Resource
{
    public readonly struct ResourceAddress
    {
        public readonly ResourceEnum Type;
        public readonly string Path;

        public ResourceAddress(ResourceEnum type, string path)
        {
            Type = type;
            Path = path;
        }
    }
}
