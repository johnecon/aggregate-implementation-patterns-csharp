namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared.Command;
    using Shared.Event;

    public class Customer6
    {
        public static CustomerRegistered Register(RegisterCustomer command)
        {
            return CustomerRegistered.Build(command.CustomerId, command.EmailAddress, command.ConfirmationHash, command.Name);
        }

        public static List<Event> ConfirmEmailAddress(List<Event> eventStream, ConfirmCustomerEmailAddress command)
        {
            var current = CustomerState.Reconstitute(eventStream);
            var eventList = new List<Event>();
            if (command.ConfirmationHash != current.ConfirmationHash) {
                var customerEmailAddressConfirmationFailed = CustomerEmailAddressConfirmationFailed.Build(command.CustomerId);
                eventList.Add(customerEmailAddressConfirmationFailed);
            } else {
                if (!current.IsEmailAddressConfirmed)
                {
                    var customerEmailAddressConfirmed = CustomerEmailAddressConfirmed.Build(command.CustomerId);
                    eventList.Add(customerEmailAddressConfirmed);
                }
            }
            return eventList;
        }

        public static List<Event> ChangeEmailAddress(List<Event> eventStream, ChangeCustomerEmailAddress command)
        {
            var current = CustomerState.Reconstitute(eventStream);
            var eventList = new List<Event>();
            if (current.EmailAddress != command.EmailAddress) {
                var customerEmailAddressChanged = CustomerEmailAddressChanged.Build(command.CustomerId,command.EmailAddress,  command.ConfirmationHash);
                eventList.Add(customerEmailAddressChanged);
            }
            return eventList;
        }
    }
}