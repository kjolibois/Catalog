using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;

namespace Catalog.Api.RequestHelpers
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto,Product>();
        }
    }
}