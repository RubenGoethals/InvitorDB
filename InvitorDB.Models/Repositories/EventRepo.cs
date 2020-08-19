using InvitorDB.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvitorDB.Models.Repositories
{
    public class EventRepo : IEventRepo
    {
        private readonly InvitorDBContext context;

        public EventRepo(InvitorDBContext context)
        {
            this.context = context;
        }

        public async Task<Event> Add(Event ev)
        {
            try
            {
                var result = context.Events.AddAsync(ev); //ChangeTracking
                await context.SaveChangesAsync();
                return ev;   //heeft nu een id (autoidentity)
                //Of: return result.Result.Entity;  
            }
            catch (Exception exc)
            {
                // cannot insert expliciet value for Id (0) when Identity Insert is OFF 
                Console.WriteLine(exc.InnerException.Message);
                return null;
            }
        }

        public async Task<int> GetAmountPersons(int eventId)
        {
            try
            {
                var result = await GetEventForIdAsync(eventId);
                return result.PersonsEvents.Count();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return 0;
            }
        }

        //tussentabel aanvullen. Let op de ANY bij WHERE
        public async Task<PersonsEvents> AddEventToPerson(string personId, int eventId, bool reserve)
        {
            try
            {
                var ev = new PersonsEvents()
                {
                    EventId = eventId,
                    PersonId = personId,
                    Reserve = reserve
                };

                var result = await context.PersonsEvents.AddAsync(ev);

                await context.SaveChangesAsync();
                return ev;
            }
            catch (Exception exc)
            {
                // cannot insert expliciet value for Id (0) when Identity Insert is OFF 
                Console.WriteLine(exc.InnerException.Message);
                return null;
            }
        }

        public async Task<EvaluationForms> AddEvalutionToEvent(EvaluationForms evaluationForms)
        {
            try
            {
                var result = await context.EvaluationForms.AddAsync(evaluationForms);
                await context.SaveChangesAsync();
                return evaluationForms;
            }
            catch (Exception exc)
            {
                // cannot insert expliciet value for Id (0) when Identity Insert is OFF 
                Console.WriteLine(exc.InnerException.Message);
                return null;
            }
        }

        public async Task Delete(int EventId)
        {
            try
            {
                Event ev = await GetEventForIdAsync(EventId);

                if (ev == null)
                {
                    return;
                }

                var result = context.Events.Remove(ev);
                ////doe hier een archivering van education ipv delete -> veiliger
                await context.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return;
        }
        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await context.Events
                .OrderBy(e => e.Name).ToListAsync<Event>();
        }
        public async Task<IEnumerable<Event>> GetAllEventsAsync(string search = null, string sortField = "Name")
        {
            IEnumerable<Event> result = null;

            //property ophalen 
            var propertyInfo = typeof(Event).GetProperty(sortField);

            if (string.IsNullOrEmpty(search))
            {
                result = await context.Events.ToListAsync();
            }
            else
            {
                var query = context.Events.Where(e => e.Name.Contains(search));
                result = await query.ToListAsync();
            }
            return result.OrderBy(e => propertyInfo.GetValue(e));
        }

        public async Task<Event> GetEventForIdAsync(int EventId)
        {
            //single returnt enigbestaande element of een exceptie, Default kan null returnen
            var result = await context.Events.SingleOrDefaultAsync<Event>(e => e.Id == EventId);
            // result = await context.Educations.FindAsync(EducationId); //ook OK - null
            return result;
        }
        public async Task<Event> Update(Event ev)
        {
            try
            {
                context.Events.Update(ev);
                await context.SaveChangesAsync();
                return ev;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
        }

        //Helpers of extra's --------------------------
        public async Task<bool> EducationExists(int id)
        {
            return await context.Events.AnyAsync(e => e.Id == id);
        }
        public async Task<Event> GetRandomEvent()
        {
            return await context.Events.OrderBy(e => Guid.NewGuid()).FirstAsync();
        }

        public async Task<IEnumerable<EvaluationForms>> GetEvaluationsForIdAsync(int value)
        {
            return await context.EvaluationForms.Where(e => e.EventId == (value)).ToListAsync();
        }

        public async Task<IEnumerable<PersonsEvents>> GetPersonInEvent(int value)
        {
            return await context.PersonsEvents.Where(e => e.EventId == (value)).ToListAsync();
        }
    }
}
