using System.Reflection;

namespace MailCheck.Common.Data.Migration.Util
{
    public interface IScriptAssemblyProvider
    {
        Assembly ScriptAssembly { get; }
    }

    public class ScriptAssemblyProvider : IScriptAssemblyProvider
    {
        public ScriptAssemblyProvider(Assembly scriptAssembly)
        {
            ScriptAssembly = scriptAssembly;
        }

        public Assembly ScriptAssembly { get; }
    }
}