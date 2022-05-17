using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicaJulio.Models
{
    //Acciones que se llevaran a cabo mediante el buffer
    public enum Estado { Agregado, Modificado, Eliminado }
    public class PersonasEntity
    {
        public Personas Personas { get; set; }
        public Estado Estado { get; set; }
    }
}
