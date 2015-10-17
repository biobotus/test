﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioBotApp.View.ModulePluginManager
{
    interface IModulePluginLoadedView
    {
        void LoadLoadedPlugin(List<string> pluginList);
    }
}
