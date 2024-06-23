using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueDuck
{
    public delegate void TagChoiceDelegate(TagChoiceArgs e);
    public class TagChoiceArgs : EventArgs
    {
        public string Tag = "";
        public TagChoiceArgs(string tag)
        {
            this.Tag = tag;
        }
    }
    internal class Messenger
    {
        private static Messenger instance;
        public static Messenger Instance => instance ??= new Messenger();

        public static event TagChoiceDelegate TagChoiceHandler;

        public string Tag { get; set; }

        public void SetLearnTag(string tag)
        {
            Tag = tag;
            TagChoiceArgs e = new TagChoiceArgs(tag);
            TagChoiceHandler(e);
        }
    }
}
