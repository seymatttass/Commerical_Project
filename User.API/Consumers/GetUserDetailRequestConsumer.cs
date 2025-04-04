using MassTransit;
using Shared.Events.UserEvents;
using Users.API.Data;

namespace Users.API.Consumers
{
    public class GetUserDetailRequestConsumer(UsersDbContext usersDbContext) : IConsumer<GetUserDetailRequestEvent>
    {


        public async Task Consume(ConsumeContext<GetUserDetailRequestEvent> context)
        {
            var userId = context.Message.UserId;

            var user = await usersDbContext.Users.FindAsync(userId);

            if (user != null)
            {
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

                await context.RespondAsync(response);
            }
            else
            {
                await context.RespondAsync(new
                {
                    Message = "Kullanıcı bulunamadı...",
                    CorrelationId = context.Message.CorrelationId 
                });
            }
        }
    }
}
