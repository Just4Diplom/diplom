using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using itransition_project.Models;
using AutoMapper;

namespace itransition_project.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Comix, ComixViewModel>();
            });


        }
    }
}