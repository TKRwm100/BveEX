﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Plugins.Scripting;
using AtsEx.PluginHost;
using AtsEx.PluginHost.LoadErrorManager;

namespace AtsEx.Plugins
{
    internal partial class PluginLoader
    {
        private class PluginLoadErrorQueue
        {
            private readonly PluginLoadErrorResolver LoadErrorManager;
            private readonly Queue<PluginException> ExceptionsToResolve = new Queue<PluginException>();

            public PluginLoadErrorQueue(ILoadErrorManager loadErrorManager)
            {
                LoadErrorManager = new PluginLoadErrorResolver(loadErrorManager);
            }

            public void OnFailedToLoadAssembly(Assembly assembly, Exception ex)
            {
                string assemblyFileName = Path.GetFileName(assembly.Location);

                Version pluginHostVersion = App.Instance.AtsExPluginHostAssembly.GetName().Version;
                Version referencedPluginHostVersion = assembly.GetReferencedPluginHost().Version;
                if (pluginHostVersion != referencedPluginHostVersion)
                {
                    string message = string.Format(Resources.Value.MaybeBecauseBuiltForDifferentVersion.Value, pluginHostVersion, App.Instance.ProductShortName);
                    BveFileLoadException additionalInfoException = new BveFileLoadException(message, assemblyFileName);

                    ExceptionsToResolve.Enqueue(new PluginException(assemblyFileName, additionalInfoException));
                }

                ExceptionsToResolve.Enqueue(new PluginException(assemblyFileName, ex));
            }

            public void OnFailedToLoadScriptPlugin(ScriptPluginPackage scriptPluginPackage, Exception ex)
            {
                ExceptionsToResolve.Enqueue(new PluginException(scriptPluginPackage.Title, ex));
            }

            public void Resolve()
            {
                while (ExceptionsToResolve.Count > 0)
                {
                    PluginException exception = ExceptionsToResolve.Dequeue();
                    try
                    {
                        LoadErrorManager.Resolve(exception.SenderName, exception.Exception);
                    }
                    catch
                    {
                        throw exception.Exception;
                    }
                }
            }


            private class PluginException
            {
                public string SenderName { get; }
                public Exception Exception { get; }

                public PluginException(string senderName, Exception exception)
                {
                    SenderName = senderName;
                    Exception = exception;
                }
            }
        }
    }
}
