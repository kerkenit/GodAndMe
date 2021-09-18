using System;

namespace GodAndMe.ViewModels
{
    public class MasterPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }

        public object args { get; internal set; }
    }
}