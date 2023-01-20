using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.Notification;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Domain.Interfaces.Service.External;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFirebaseService _firebaseService;

        public NotificationService(IUnitOfWork uow,
            IFirebaseService firebaseService)
        {
            _uow = uow;
            _firebaseService = firebaseService;
        }

        public async Task<ResultResponse> SendAsync(NotificationSendDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync(c => c.Id == obj.IdUser);

                if (user != null)
                {
                    var registrationToken = await _uow.RegistrationToken.GetFirstOrDefaultAsync(c => c.UserId == obj.IdUser);

                    if (registrationToken != null)
                    {
                        response = await _firebaseService.SendNotificationAsync(registrationToken.Token, obj.Title, obj.Body);

                        if (response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            response.Message = "Notification sent successfully.";
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                response.Message = "Could not send notification.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
