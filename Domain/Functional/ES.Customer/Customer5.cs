namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Shared.Command;
    using Shared.Event;

    public class Customer5
    {
        public static CustomerRegistered Register(RegisterCustomer command)
        {
            return CustomerRegistered.Build(command.CustomerId, command.EmailAddress, command.ConfirmationHash, command.Name);
        }

        public static List<Event> ConfirmEmailAddress(List<Event> eventStream, ConfirmCustomerEmailAddress command)
        {
            var eventList = new List<Event>();
            bool isEmailAddressConfirmed = false;
            Hash confirmationHash = default;
            foreach (var evt in eventStream)
            {
                switch (evt)
                {
                    case CustomerRegistered e:
                        if (e.CustomerId == command.CustomerId) {
                            confirmationHash = e.ConfirmationHash;
                        }
                        break;
                    case CustomerEmailAddressConfirmed e:
                        if (e.CustomerId == command.CustomerId) {
                            isEmailAddressConfirmed = true;
                        }
                        break;
                    case CustomerEmailAddressChanged e:
                        if (e.CustomerId == command.CustomerId) {
                            isEmailAddressConfirmed = false;
                            confirmationHash = e.ConfirmationHash;
                        }
                        break;
                }
            }
            if (command.ConfirmationHash != confirmationHash) {
                var customerEmailAddressConfirmationFailed = CustomerEmailAddressConfirmationFailed.Build(command.CustomerId);
                eventList.Add(customerEmailAddressConfirmationFailed);
            } else {
                if (!isEmailAddressConfirmed)
                {
                    var customerEmailAddressConfirmed = CustomerEmailAddressConfirmed.Build(command.CustomerId);
                    eventList.Add(customerEmailAddressConfirmed);
                }
            }
            return eventList;
        }

        public static List<Event> ChangeEmailAddress(List<Event> eventStream, ChangeCustomerEmailAddress command)
        {
            var eventList = new List<Event>();
            EmailAddress emailAddress = default;
            foreach (var evt in eventStream)
            {
                switch (evt)
                {
                    case CustomerRegistered e:
                        if (e.CustomerId == command.CustomerId) {
                            emailAddress = e.EmailAddress;
                        }
                        break;
                    case CustomerEmailAddressChanged e:
                        if (e.CustomerId == command.CustomerId) {
                            emailAddress = e.EmailAddress;
                        }
                        break;
                }
            }
            if (emailAddress != command.EmailAddress) {
                var customerEmailAddressChanged = CustomerEmailAddressChanged.Build(command.CustomerId,command.EmailAddress,  command.ConfirmationHash);
                eventList.Add(customerEmailAddressChanged);
            }

            return eventList;
        }
    }
}