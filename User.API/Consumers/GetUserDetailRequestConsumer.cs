using MassTransit;
using Shared.Events.UserEvents;
using Users.API.Data;

namespace Users.API.Consumers
{
    public class GetUserDetailRequestConsumer(UsersDbContext usersDbContext) : IConsumer<GetUserDetailRequestEvent>
    {


        public async Task Consume(ConsumeContext<GetUserDetailRequestEvent> context)
        {
            // Gelen talepteki UserId'yi alıyoruz
            var userId = context.Message.UserId;

            // Kullanıcıyı veritabanından sorguluyoruz
            var user = await usersDbContext.Users.FindAsync(userId);

            // Eğer kullanıcı varsa, bilgileri içeren bir yanıt gönderiyoruz
            if (user != null)
            {
                // GetUserDetailResponseEvent mesajını oluşturuyoruz
                var response =new GetUserDetailResponseEvent(context.Message.CorrelationId)
                {
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    TelNo = user.TelNo,
                    Birthdate = user.Birthdate,
                };

                // Yanıtı gönderiyoruz
                await context.RespondAsync(response);
            }
            else
            {
                // Eğer kullanıcı bulunamazsa, hata mesajı dönebiliriz
                await context.RespondAsync(new
                {
                    Message = "Kullanıcı bulunamadı...",
                    CorrelationId = context.Message.CorrelationId // Hata mesajında da CorrelationId'yi gönderebiliriz
                });
            }
        }
    }
}
