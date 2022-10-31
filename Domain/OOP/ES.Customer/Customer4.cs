namespace Domain.OOP.ES.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Shared.Command;
    using Shared.Event;

    public class Customer4
    {
        private EmailAddress emailAddress;
        private Hash confirmationHash;
        private bool isEmailAddressConfirmed;
        private PersonName name;

        public List<Event> RecordedEvents { get; }

        private Customer4()
        {
            RecordedEvents = new List<Event>();
        }

        public static Customer4 Register(RegisterCustomer command)
        {
            Customer4 customer = new Customer4();

            var evt = CustomerRegistered.Build(command.CustomerId, command.EmailAddress, command.ConfirmationHash, command.Name);
            customer.RecordThat(evt);
            customer.Apply(evt);

            return customer;
        }

        public static Customer4 Reconstitute(List<Event> events)
        {
            var customer = new Customer4();

            customer.Apply(events);

            return customer;
        }

        public void ConfirmEmailAddress(ConfirmCustomerEmailAddress command)
        {
            if (command.ConfirmationHash != confirmationHash) {
                    var customerEmailAddressConfirmationFailed = CustomerEmailAddressConfirmationFailed.Build(command.CustomerId);
                    RecordThat(customerEmailAddressConfirmationFailed);
            } else {
                if (!isEmailAddressConfirmed) {
                    var customerEmailAddressConfirmed = CustomerEmailAddressConfirmed.Build(command.CustomerId);
                    RecordThat(customerEmailAddressConfirmed);
                }
            }
        }

        public void ChangeEmailAddress(ChangeCustomerEmailAddress command)
        {
            if (command.EmailAddress != emailAddress) {
                var customerEmailAddressChanged = CustomerEmailAddressChanged.Build(command.CustomerId,command.EmailAddress,  command.ConfirmationHash);
                RecordThat(customerEmailAddressChanged);
            }
        }

        private void RecordThat(Event evt)
        {
            RecordedEvents.Add(evt);
        }

        private void Apply(List<Event> events)
        {
            foreach (var evt in events)
            {
                Apply(evt);
            }
        }

        private void Apply(Event evt)
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
                    break;
                case CustomerEmailAddressChanged e:
                    isEmailAddressConfirmed = false;
                    confirmationHash = e.ConfirmationHash;
                    emailAddress = e.EmailAddress;
                    break;
            }
        }
    }
}