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
    public class AgendaRepository : Repository<Agenda>
    {
        public AgendaRepository(DbContext context) : base(context)
        {

        }

        public override void Insert(Agenda entity)
        {


            //Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            entity.Actividad=
           Regex.Replace(entity.Actividad.
                Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            entity.Actividad = entity.Actividad.ToUpper();
            entity.Actividad = entity.Actividad.Trim();
            entity.Timestamp = DateTime.Now.ToMexicoTime();
            base.Insert(entity);
        }
        public override void Update(Agenda entity)
        {
            entity.Actividad =
           Regex.Replace(entity.Actividad.
                Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            entity.Actividad = entity.Actividad.ToUpper();
            entity.Actividad = entity.Actividad.Trim();
            entity.Timestamp = DateTime.Now.ToMexicoTime();
            base.Update(entity);
        }
        public override void Delete(Agenda entity)
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
        public override IEnumerable<Agenda> GetAll()
        {
            return base.GetAll().Where(x => x.Eliminado == 0).OrderBy(x => x.Actividad);
        }
        public IEnumerable<Agenda> GetAllSinceDate(DateTime timestamp)
        {
            var cambiados = base.GetAll().Where(x => x.Eliminado == 0 && x.Timestamp > timestamp);
            var eliminados = base.GetAll().Where(x => x.Eliminado == 1 && x.Timestamp > timestamp)
                .Select(x => new Agenda { Id = x.Id });

            return cambiados.Concat(eliminados);
        }

        public override bool IsValid(Agenda entity, out List<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(entity.Actividad))
            {
                validationErrors.Add("La actividad de la agenda se encuentra vacío.");
            }
            if (Context.Set<Agenda>().Any(x => x.Actividad == entity.Actividad && x.Eliminado == 0 && x.Id != entity.Id))
            {
                validationErrors.Add("La actividad de la agenta ya se encuentra en la lista.");
            }
            if (string.IsNullOrWhiteSpace(entity.Importancia))
            {
                validationErrors.Add("La importancia de la actividad se encuentra vacía.");
            }
            if (string.IsNullOrWhiteSpace(entity.FechaDeRealizar))
            {
                validationErrors.Add("El día para realizar la actividad está vacío.");
            }






            return validationErrors.Count == 0;
        }
    }
}
