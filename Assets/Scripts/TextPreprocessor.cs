using System.Text;

namespace Assets.Scripts
{
    public class TextPreprocessor
    {
        private struct PreprocessMethod
        {
            public string Prepend;
            public string Append;
        }

        private PreprocessMethod 
            italics = new PreprocessMethod {Prepend = "<i>", Append = "</i>"},
            bold = new PreprocessMethod {Prepend = "<b>", Append = "</b>"};

        public void FeedNextCharacter(ref StringBuilder sb)
        {

        }
    }
}