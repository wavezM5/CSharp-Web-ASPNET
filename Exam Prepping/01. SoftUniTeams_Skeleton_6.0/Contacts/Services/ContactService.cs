﻿using Contacts.Data;
using Contacts.Data.Models;
using Contacts.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactsDbContext _context;

        public ContactService(ContactsDbContext data)
        {
            _context = data;
        }

        public async Task<ICollection<ContactViewModel>> GetAllContacts()
        {
            var contacts = await _context.Contacts
                .Select(c => new ContactViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    Address = c.Address,
                    Website = c.Website
                }).ToListAsync();

            return contacts;
        }

        public async Task AddContactAsync(ContactViewModel contactViewModel)
        {
            Contact contact = new Contact()
            {
                FirstName = contactViewModel.FirstName,
                LastName = contactViewModel.LastName,
                Email = contactViewModel.Email,
                Address = contactViewModel.Address,
                PhoneNumber = contactViewModel.PhoneNumber,
                Website = contactViewModel.Website
            };

            await _context.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<ContactViewModel> FindContactAsync(int id)
        {
            Contact? contactToFind = await _context.Contacts.FindAsync(id);

            if (contactToFind == null)
            {
                return null;
            }

            ContactViewModel contactViewModel = new ContactViewModel()
            {
                FirstName = contactToFind.FirstName,
                LastName = contactToFind.LastName,
                Email = contactToFind.Email,
                Address = contactToFind.Address,
                PhoneNumber = contactToFind.PhoneNumber,
                Website = contactToFind.Website
            };

            return contactViewModel;
        }

        public async Task EditContactAsync(int id, ContactViewModel contactViewModel)
        {
            Contact? contactInDataBase = await _context.Contacts.FindAsync(id);

            if (contactInDataBase != null)
            {
                contactInDataBase.FirstName = contactViewModel.FirstName;
                contactInDataBase.LastName = contactViewModel.LastName;
                contactInDataBase.Email = contactViewModel.Email;
                contactInDataBase.Address = contactViewModel.Address;
                contactInDataBase.PhoneNumber = contactViewModel.PhoneNumber;
                contactInDataBase.Website = contactViewModel.Website;

                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<IEnumerable<ContactViewModel>> GetAllTeamsAsync(string userId)
        {
            return await _context.Contacts
                .Where(au => au.ApplicationUserContacts.Any(v => v.ApplicationUserId == userId))
                .Select(c => new ContactViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    Address = c.Address,
                    Website = c.Website
                }).ToListAsync();
        }

        public async Task AddTeamAsync(int id, string userId)
        {
            if (_context.ApplicationUserContacts.Any(au => au.ApplicationUserId == userId && au.ContactId == id))
            {
                return;
            }   

            var contact = new ApplicationUserContact()
            {
                ApplicationUserId = userId,
                ContactId = id
            };

            await _context.ApplicationUserContacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromTeamAsync(int id, string userId)
        {
            var userContact = await _context.ApplicationUserContacts.FirstOrDefaultAsync(au =>
                au.ApplicationUserId == userId && au.ContactId == id);

            if(userContact == null)
                return;

            _context.ApplicationUserContacts.Remove(userContact);
            await _context.SaveChangesAsync();
        }
    }
}
