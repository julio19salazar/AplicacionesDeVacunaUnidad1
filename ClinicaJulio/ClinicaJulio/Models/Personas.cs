using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace ClinicaJulio.Models
{
    [Table("Personas")]
    public class Personas
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }
        [NotNull]
        public string Nombre { get; set; }
        [NotNull]
        public string Edad { get; set; }
        [NotNull]
        public string Sexo { get; set; }
        [NotNull]
        public string Vacuna { get; set; }
        [NotNull]
        public string Lote { get; set; }
    }
}
