using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Models
{
    public class ReservaRepository
    {
        public List<Reserva> GetAll()
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Reserva.Include(r => r.Socio).Include(r => r.Actividad).ToList();
            }
        }

        public Reserva GetById(int id)
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Reserva.Include(r => r.Socio).Include(r => r.Actividad).FirstOrDefault(r => r.Id == id);
            }
        }

        public bool ValidarReserva(int socioId, int actividadId, DateTime fecha, int? reservaIdExcluir = null)
        {
            using (var context = new zenithzoneEntities())
            {
                var fechaSoloFecha = fecha.Date;
                
                var query = context.Reserva.Where(r => 
                    r.SocioId == socioId && 
                    DbFunctions.TruncateTime(r.Fecha) == fechaSoloFecha);

                if (reservaIdExcluir.HasValue)
                {
                    query = query.Where(r => r.Id != reservaIdExcluir.Value);
                }

                return !query.Any();
            }
        }

        public void Add(Reserva reserva)
        {
            using (var context = new zenithzoneEntities())
            {
                context.Reserva.Add(reserva);
                var result = context.SaveChanges();
                
                System.Diagnostics.Debug.WriteLine($"Reserva agregada con ID: {reserva.Id}, Rows affected: {result}");
            }
        }

        public void Update(Reserva reserva)
        {
            using (var context = new zenithzoneEntities())
            {
                var existing = context.Reserva.Find(reserva.Id);
                if (existing != null)
                {
                    existing.SocioId = reserva.SocioId;
                    existing.ActividadId = reserva.ActividadId;
                    existing.Fecha = reserva.Fecha;
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new zenithzoneEntities())
            {
                var reserva = context.Reserva.Find(id);
                if (reserva != null)
                {
                    context.Reserva.Remove(reserva);
                    context.SaveChanges();
                }
            }
        }

        public List<Socio> GetSociosActivos()
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Socio.Where(s => s.Activo).ToList();
            }
        }

        public List<Actividad> GetAllActividades()
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Actividad.ToList();
            }
        }
    }
}
