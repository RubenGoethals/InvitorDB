using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvitorDB.Models.Repositories
{
    public interface IEventRepo
    {
        Task<Event> Add(Event ev);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<int> GetAmountPersons(int eventId);
        Task<IEnumerable<Event>> GetAllEventsAsync(string search = null, string sortField = "Name");
        Task<PersonsEvents> AddEventToPerson(string personId, int eventId, bool reserve);
        Task<EvaluationForms> AddEvalutionToEvent(EvaluationForms evaluationForms);
        Task<Event> GetEventForIdAsync(int id);

        //UPDATE (Async)
        Task<Event> Update(Event ev);

        //DELETE (Async)
        Task Delete(int eventId);
        Task<IEnumerable<EvaluationForms>> GetEvaluationsForIdAsync(int value);
        Task<IEnumerable<PersonsEvents>> GetPersonInEvent(int value);
    }
}
