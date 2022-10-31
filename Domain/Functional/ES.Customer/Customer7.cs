namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared.Command;
    using Shared.Event;

    public class Customer7
    {
        public static CustomerRegistered Register(RegisterCustomer command)
        {
            return CustomerRegistered.Build(command.CustomerId, command.EmailAddress, command.ConfirmationHash, command.Name);
        }

        public static List<Event> ConfirmEmailAddress(CustomerState current, ConfirmCustomerEmailAddress command)
        {
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

        public static List<Event> ChangeEmailAddress(CustomerState current, ChangeCustomerEmailAddress command)
        {
            var eventList = new List<Event>();
            if (current.EmailAddress != command.EmailAddress) {
                var customerEmailAddressChanged = CustomerEmailAddressChanged.Build(command.CustomerId,command.EmailAddress,  command.ConfirmationHash);
                eventList.Add(customerEmailAddressChanged);
            }
            return eventList;
        }
    }
}