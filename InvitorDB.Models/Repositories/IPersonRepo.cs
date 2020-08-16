using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvitorDB.Models.Repositories
{
    public interface IPersonRepo
    {
        //READ 
        Task<IEnumerable<Person>> GetAllAsync(string roleName);

        Task<Person> GetForIdAsync(string id);

        Task<IEnumerable<Person>> GetByEventAsync(int eventId, string role);
        bool PersonExists(string id);

        //CREATE (Async)
        Task<Person> Add(Person person);

        //UPDATE (Async)
        Task<Person> Update(Person person);

        //DELETE (Async)
        Task Delete(string id);
        Task RemoveEvents(string id);
        Task PersonsToEvent(string id, string[] selectedEducationsString);
    }
}
