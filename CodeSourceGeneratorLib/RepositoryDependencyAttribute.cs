using System;

namespace CodeSourceGeneratorLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RepositoryDependencyAttribute:Attribute
    {
        public RepositoryDependencyAttribute()
        {
            
        }
    }
}