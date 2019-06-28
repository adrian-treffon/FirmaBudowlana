
namespace FirmaBudowlana.Infrastructure.Services
{
    interface IEncrypter : IService
    {
        string GetSalt(string value);
        string GetHash(string value, string salt);

    }
}
