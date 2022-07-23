﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IronPython.Hosting;

using Microsoft.Scripting.Hosting;

using Automatic9045.AtsEx.PluginHost;
using Automatic9045.AtsEx.PluginHost.Plugins;

namespace Automatic9045.AtsEx.Plugins.Scripting.IronPython2
{
    internal sealed class IronPython2Plugin : ScriptPluginBase
    {
        private IronPython2Plugin(ScriptPluginBuilder builder, PluginType pluginType) : base(builder, pluginType, true)
        {
        }

        public static IronPython2Plugin FromPackage(PluginBuilder builder, PluginType pluginType, ScriptPluginPackage package)
        {
            ScriptEngine engine = ScriptEngineProvider.CreateEngine(Path.GetDirectoryName(package.Location));
            ScriptScope scope = ScriptEngineProvider.CreateScope(engine);

            ScriptPluginBuilder newBuilder = new ScriptPluginBuilder(builder)
            {
                Location = package.Location,
                Title = package.Title,
                Version = package.Version,
                Description = package.Description,
                Copyright = package.Copyright,

                ConstructorScript = package.ConstructorScriptPath is null ? null : PluginScript<Globals>.LoadFrom(package.ConstructorScriptPath, engine, scope),
                DisposeScript = package.DisposeScriptPath is null ? null : PluginScript<Globals>.LoadFrom(package.DisposeScriptPath, engine, scope),
                OnScenarioCreatedScript = package.OnScenarioCreatedScriptPath is null ? null : PluginScript<ScenarioCreatedGlobals>.LoadFrom(package.OnScenarioCreatedScriptPath, engine, scope),
                OnStartedScript = package.OnStartedScriptPath is null ? null : PluginScript<StartedGlobals>.LoadFrom(package.OnStartedScriptPath, engine, scope),
                TickScript = package.TickScriptPath is null ? null : PluginScript<TickGlobals>.LoadFrom(package.TickScriptPath, engine, scope),
            };

            return new IronPython2Plugin(newBuilder, pluginType);
        }
    }
}
