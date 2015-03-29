using System;
using System.Reflection;

namespace CategorizeModule
{
    class CustomAssemblyLoader : MarshalByRefObject
    {
        private Assembly _assembly;

        public Assembly GetLoadedAssemblyOnAppDomain()
        {
            return _assembly;
        }

        public void Load(string path)
        {
            ValidatePath(path);

            _assembly = Assembly.Load(path);
        }

        public void LoadFrom(string path)
        {
            ValidatePath(path);

            _assembly = Assembly.LoadFrom(path);
        }

        private void ValidatePath(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (!System.IO.File.Exists(path))
                throw new ArgumentException(String.Format("path \"{0}\" does not exist", path));
        }
    }
}
