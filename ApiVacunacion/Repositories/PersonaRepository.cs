using ApiVacunacion.Helpers;
using ApiVacunacion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiVacunacion.Repositories
{
    public class PersonaRepository : Repository<Persona>
    {

        public PersonaRepository(DbContext context) : base(context)
        {

        }

        public override void Insert(Persona entity)
        {
            

            //Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            entity.Nombre = Regex.Replace(entity.Nombre.
                Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            entity.Nombre = entity.Nombre.ToUpper();
            entity.Nombre = entity.Nombre.Trim();

            entity.Timestamp = DateTime.Now.ToMexicoTime();
        
            
            base.Insert(entity);
        }
        public override void Update(Persona entity)
        {
            entity.Nombre = Regex.Replace(entity.Nombre.
              Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            entity.Nombre = entity.Nombre.ToUpper();
            entity.Nombre = entity.Nombre.Trim();
            entity.Timestamp = DateTime.Now.ToMexicoTime();
            base.Update(entity);
        }
        public override void Delete(Persona entity)
        {
            if (entity.Eliminado == 0)
            {
                entity.Timestamp = DateTime.Now.ToMexicoTime();
                entity.Eliminado = 1;
                base.Update(entity);
            }
            byte TimeToLive = 5;

            var FechaPorEliminar = DateTime.Now.Subtract(TimeSpan.FromDays(TimeToLive));
            var porEliminar = base.GetAll().Where(x => x.Eliminado == 1
            && x.Timestamp < FechaPorEliminar);

          
            foreach (var item in porEliminar)
            {
                Context.Remove(item);
            }
            Context.SaveChanges();
        }
        public override IEnumerable<Persona> GetAll()
        {
            return base.GetAll().Where(x => x.Eliminado == 0).OrderBy(x => x.Nombre);
        }
        public IEnumerable<Persona> GetAllSinceDate(DateTime timestamp)
        {
            var cambiados = base.GetAll().Where(x => x.Eliminado == 0 && x.Timestamp > timestamp);
            var eliminados = base.GetAll().Where(x => x.Eliminado == 1 && x.Timestamp > timestamp)
                .Select(x => new Persona { Id = x.Id });

            return cambiados.Concat(eliminados);
        }

        public override bool IsValid(Persona entity, out List<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                validationErrors.Add("El nombre de la persona se encuentra vacío.");
            }
            if (Context.Set<Persona>().Any(x => x.Nombre == entity.Nombre && x.Eliminado == 0 && x.Id != entity.Id))
            {
                validationErrors.Add("El nombre de la persona ya se encuentra en la lista.");
            }

            if (string.IsNullOrWhiteSpace(entity.Edad))
            {
                validationErrors.Add("La edad  de la persona se encuentra vacía.");
            }

            if (entity.Edad!=null)
            {
                var convertirEdad = entity.Edad;
                if (convertirEdad != null)
                {
                    var convertido = float.Parse(convertirEdad);
                    double EsoNoEs = convertido % 1;
                    if (EsoNoEs != 0)
                    {
                        validationErrors.Add("La edad de la persona debe ser entera.");
                    }
                    if (convertido < 1 || convertido >= 100)
                    {
                        validationErrors.Add("La persona debe tener entre 1 y 99 años.");
                    }
                }
               
            }
            if (string.IsNullOrWhiteSpace(entity.Sexo))
            {
                validationErrors.Add("El sexo de la persona se encuentra vacío.");
            }
            if (string.IsNullOrWhiteSpace(entity.Vacuna))
            {
                validationErrors.Add("Especifique la vacuna aplicada.");
            }
            if (string.IsNullOrWhiteSpace(entity.Lote))
            {
                validationErrors.Add("Especifique el lote de la vacuna.");
            }
           
           



            return validationErrors.Count == 0;
        }

    }
}
