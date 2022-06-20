using MiAgenda.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiAgenda.Repositories
{
   public class AgendaRepository
    {
        public SQLite.SQLiteConnection Context { get; set; }

        public AgendaRepository()
        {
            var ruta = Environment
                .GetFolderPath(Environment.SpecialFolder.Personal) + "/chat";
            Context = new SQLite.SQLiteConnection(ruta);
            Context.CreateTable<AgendaModel>();
        }

        public IEnumerable<AgendaModel> GetAll()
        {

            var a = Context.Table<AgendaModel>()
                .OrderBy(x => x.Actividad);
            var y = a;
            return a;


        }

        public AgendaModel Get(int id)
        {
            var chat = Context.Table<AgendaModel>().FirstOrDefault(x=>x.Id==id);
            return chat;
        }

        public void Insert(AgendaModel c)
        {
           
            Context.Insert(c);
        }

        public void Update(AgendaModel c)
        {
            Context.Update(c);
        }
        public void Delete(AgendaModel c)
        {
            Context.Delete(c);
        }
    }
}
