using CreditKiosk.Models;
using System.Collections.Generic;
using System.ComponentModel;
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

        public List<Person> GetAll()
        {
            using (var context = new KioskDbContext())
            {
                return context.Persons.ToList();
            }
        }

        public List<Person> GetAllSorted()
        {
            using (var context = new KioskDbContext())
            {
                return context.Persons.OrderBy(p => p.LastName).ThenByDescending(p => p.FirstName).ToList();
            }
        }
    }
}
