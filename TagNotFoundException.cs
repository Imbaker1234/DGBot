namespace DGBot
{
    using System;

    public class TagNotFoundException : Exception
    {
        public string TagName { get; set; }
    }
}