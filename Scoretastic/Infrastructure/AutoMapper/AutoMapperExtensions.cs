﻿using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Scoretastic.Web.Controllers;

namespace Scoretastic.Web.Infrastructure.AutoMapper
{
    //https://github.com/ayende/RaccoonBlog/blob/master/RaccoonBlog.Web/Infrastructure/AutoMapper/AutoMapperExtensions.cs
    public static class AutoMapperExtensions
    {
        public static List<TResult> MapTo<TResult>(this IEnumerable self)
        {
            if (self == null)
                throw new ArgumentNullException();

            return (List<TResult>) Mapper.Map(self, self.GetType(), typeof (List<TResult>));
        }

        public static TResult MapTo<TResult>(this object self)
        {
            if (self == null)
                throw new ArgumentNullException();

            return (TResult)Mapper.Map(self, self.GetType(), typeof(TResult));
        }

		public static TResult MapTo<TResult>(this object self, TResult value)
		{
            if (self == null)
                throw new ArgumentNullException();

			return (TResult)Mapper.Map(self, value, self.GetType(), typeof(TResult));
		}

        public static TResult DynamicMapTo<TResult>(this object self)
        {
            if (self == null)
                throw new ArgumentNullException();

            return (TResult)Mapper.DynamicMap(self, self.GetType(), typeof(TResult));
        }
    }

    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<CompetitionCreateViewModel.ViewInput, Competition>()
                .ForMember(x => x.Id, o => o.Ignore())
                ;

            Mapper.CreateMap<CompetitionEditViewModel.ViewInput, Competition>();
            Mapper.CreateMap<Competition, CompetitionEditViewModel.ViewInput>();
        }
    }
}