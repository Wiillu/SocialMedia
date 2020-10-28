﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Api.Response
{
    public class ApiResponse<T>
    {
        //se crea respuestas
        public ApiResponse(T data)
        {
            Data = data;
        }
        public T  Data { get; set; }
    }
}
