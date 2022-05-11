using CreditKiosk.Models;
using System.Collections.Generic;
using System.Linq;

namespace CreditKiosk.Managers
{
    class PersonManager
    {
        public void Add(Person person)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(person);
                context.SaveChanges();
            }
        }

        public void Remove(Person person)
        {
            using (var context = new KioskDbContext())
            {
                context.Persons.Remove(person);
                context.SaveChanges();
            }
        }

        public List<Person> GetAllSorted()
        {
            using (var context = new KioskDbContext())
            {
                return context.Persons.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
            }
        }
    }
}
