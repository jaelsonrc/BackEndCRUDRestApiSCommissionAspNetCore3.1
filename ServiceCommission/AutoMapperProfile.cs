using AutoMapper;
using ServiceCommission.DTOs;
using ServiceCommission.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            AllowNullCollections = true;
            AllowNullDestinationValues = true;


            CreateMap<User, UserDTO>();

            CreateMap<CommissionDTO, Commission>();

            CreateMap<Commission, CommissionDTO>();


        }
    }
}
