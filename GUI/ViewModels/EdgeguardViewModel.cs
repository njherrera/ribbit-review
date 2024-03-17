using CommunityToolkit.Mvvm.ComponentModel;
using CSharpParser.Filters.Settings;
using CSharpParser.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace GUI.ViewModels
{
    public partial class EdgeguardViewModel : FilterViewModel
    {
        public override FilterSettingsBuilder Builder
        {
            get
            {
                return this._builder;
            }
        }

        private EdgeguardSettingsBuilder _builder = new EdgeguardSettingsBuilder();

        public override Filter Filter
        {
            get
            {
                return this._filter;
            }
        }

        private Edgeguards _filter = new Edgeguards(); 
    }
}
