﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace itransition_project.Models
{
    [Serializable]
    public class JsonReturnComixViewModel
    {
        public int Id { get; set; }
        public AuthorViewModel Author { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<TagText> Tags { get; set; }
        public List<Page> Pages { get; set; }
    }

    [Serializable]
    public class JsonComixViewModel
    {
        public string Name { get; set; }
        public List<string> Images { get; set; }
        public List<string> Tags { get; set; }
    }

    [Serializable]
    public class JsonPagesViewModel
    {
        public string Template { get; set; }
        public List<JsonFrameImageViewModel> FrameImages { get; set; }
    }

    [Serializable]
    public class JsonFrameImageViewModel
    {
        public string BackgroundImage { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public List<JsonBalloonsViewModel> Balloons { get; set; }
    }

    [Serializable]
    public class JsonBalloonsViewModel
    {
        public string Text { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class TagText
    {
        public string text { get; set; }
    }

    public class TagCloud
    {
        public string Text { get; set; }
        public int Usage { get; set; }
    }
}
