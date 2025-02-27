using Address.API.Mapping;

namespace Address.API.DTOS.Address
{
    public class CreateAddressDTO : AddressAutoMapperProfile
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string AddressText { get; set; }

    }
}
