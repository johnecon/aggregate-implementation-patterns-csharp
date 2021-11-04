namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Shared.Event;

    public class CustomerState
    {
        EmailAddress emailAddress;
        Hash confirmationHash;
        PersonName name;
        bool isEmailAddressConfirmed;

        private CustomerState()
        {
        }

        public static CustomerState Reconstitute(List<Event> events)
        {
            var customer = new CustomerState();

            customer.Apply(events);

            return customer;
        }

        // TODO: This shouldn't be public
        public void Apply(List<Event> events)
        {
            foreach (var evt in events)
            {
                switch (evt)
                {
                    case CustomerRegistered e:
                        // TODO
                        break;
                    case CustomerEmailAddressConfirmed e:
                        // TODO
                        break;
                    case CustomerEmailAddressChanged e:
                        // TODO
                        break;
                }
            }
        }
    }
}