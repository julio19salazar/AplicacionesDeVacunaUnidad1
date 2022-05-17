using SQLite;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClinicaJulio.Models
{
    
    public class CatalogoPersonas
    {
        public SQLiteConnection Conexion { get; set; }

        public CatalogoPersonas()
        {


            //Se recomienda no colocar ninguna extensión ya que le puede llamar la atención a los usuarios, y pueden empezar a jugar con el
        //por esta ocación yo le pondré 
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/personas.db3";
            Conexion = new SQLiteConnection(ruta);

            Conexion.CreateTable<Personas>();
        }

        public IEnumerable<Personas> GetAll()
        {
            return Conexion.Table<Personas>().ToList().OrderBy(x => x.Nombre);
        }
        /// <summary>
        /// Recuperar persona mediante el id
        /// </summary>
        /// <param name="nombre">nombre de la persona que requiere buscar</param>
        /// <returns></returns>
        public IEnumerable<Personas> GetPersonaById(string nombre)
        {
          
            //    string query = "SELECT * FROM medicamentos1 WHERE nombre_medicamento LIKE '%' + @nombre + '%'";
            //    SqlCommand consulta = new SqlCommand(query, conexion);
            var x = GetAll();

            var coincidencias = (from p in x
                                 where p.Nombre.Contains(nombre)
                                 select new Personas()
                                 {
                                     Id=p.Id,
                                     Nombre = p.Nombre,
                                     Edad = p.Edad,
                                     Sexo = p.Sexo,
                                     Vacuna = p.Vacuna,
                                     Lote = p.Lote
                                 }).ToList();
            //if (persona==null)
            //{

            //    return null;
            //}
            
            return coincidencias;
           
            
           
        }
        public void InsertOrReplace(Personas c)
        {
            //Tomar la decisión:

            var contacto = Conexion.Find<Personas>(c.Id);

            //Insertar si no existia el id
            if (contacto == null)
            {
                //pero tiene nombre
                if (c.Nombre != null) Conexion.Insert(c);

                //si no tiene nombre, no hace nada
            }
            else if (c.Nombre != null)
            {
                //y tiene nombre, edito el nombre
                contacto.Nombre = c.Nombre;
                contacto.Edad = c.Edad;
                contacto.Sexo = c.Sexo;
                contacto.Vacuna = c.Vacuna;
                contacto.Lote = c.Lote;
                Conexion.Update(contacto);
            }
            else
            {
                //si no tiene nombre, lo elimino
                Conexion.Delete(contacto);

            }


        }

    }
}
