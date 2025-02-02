﻿using AutoMapper;
using TASI.Backend.Domain.Suppliers.Dtos;
using TASI.Backend.Domain.Suppliers.Entities;
using TASI.Backend.Domain.Suppliers.Handlers;

namespace TASI.Backend.Domain.Suppliers.Mappers
{
    public class SupplierDomainMapperProfile : Profile
    {
        public SupplierDomainMapperProfile()
        {
            CreateMap<CreateSupplierCommand, Supplier>();
            CreateMap<EditSupplierDto, Supplier>()
                .ForMember(x => x.SupplierId, options => options.Ignore())
                .ForAllOtherMembers(options =>
                    options.Condition((_, _, member) => member != null));
        }
    }
}
