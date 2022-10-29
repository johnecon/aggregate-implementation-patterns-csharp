namespace Domain.OOP.ES.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Shared;
    using Shared.Command;
    using Shared.Event;

    public class Customer3
    {
        private EmailAddress emailAddress;
        private Hash confirmationHash;
        private bool isEmailAddressConfirmed;
        private PersonName name;
        
        private Customer3()
        {
        }

        public static CustomerRegistered Register(RegisterCustomer command)
        {
            var registerCustomer = CustomerRegistered.Build(command.CustomerId, command.EmailAddress, command.ConfirmationHash, command.Name);
            return registerCustomer;
        }

        public static Customer3 Reconstitute(List<Event> events)
        {
            var customer = new Customer3();

            customer.Apply(events);

            return customer;
        }

        public List<Event> ConfirmEmailAddress(ConfirmCustomerEmailAddress command)
        {
            var eventList = new List<Event>();
            if (!isEmailAddressConfirmed) {
                if (command.ConfirmationHash != confirmationHash) {
                    var customerEmailAddressConfirmed = CustomerEmailAddressConfirmationFailed.Build(command.CustomerId);
                    eventList.Add(customerEmailAddressConfirmed);
                } else {
                    var customerEmailAddressConfirmed = CustomerEmailAddressConfirmed.Build(command.CustomerId);
                    eventList.Add(customerEmailAddressConfirmed);
                }
            }
            return eventList;
        }

        public List<Event> ChangeEmailAddress(ChangeCustomerEmailAddress command)
        {
            var eventList = new List<Event>();
            if (command.EmailAddress != emailAddress) {
                var customerEmailAddressChanged = CustomerEmailAddressChanged.Build(command.CustomerId,command.EmailAddress,  command.ConfirmationHash);
                eventList.Add(customerEmailAddressChanged);
            }
            return eventList;
        }

        private void Apply(List<Event> events)
        {
            foreach (var evt in events)
            {
                Apply(evt);
            }
        }

        public void Apply(Event evt)
        {
            switch (evt)
            {
                case CustomerRegistered e:
                    confirmationHash = e.ConfirmationHash;
                    emailAddress = e.EmailAddress;
                    name = e.Name;
                    break;
                case CustomerEmailAddressConfirmed e:
                    isEmailAddressConfirmed = true;
                    // TODO
                    break;
                case CustomerEmailAddressChanged e:
                    isEmailAddressConfirmed = false;
                    confirmationHash = e.ConfirmationHash;
                    emailAddress = e.EmailAddress;
                    // TODO
                    break;
            }
        }
    }
}