using System;
using System.Collections.Generic;

#nullable disable

namespace ApiVacunacion.Models
{
    public partial class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Edad { get; set; }
        public int Peso { get; set; }
        public string Area { get; set; }
    }
}
