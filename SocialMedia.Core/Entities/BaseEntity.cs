using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Entities
{
    public abstract class BaseEntity
    {
        //Se crea una entidad base asbtracta solo se colocan los datos que coincidan para generar los servicios por genericos
        public int Id { get; set; }
    }
}
