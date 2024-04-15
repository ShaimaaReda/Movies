using BaseLibrary.Entities;
using Microsoft.AspNetCore.SignalR;

namespace MovieServer.Hubs
{
    public class ReviewHub : Hub
    {
        public async Task SendReview(Review review)
        {
            await Clients.All.SendAsync("ReceiveReview", review);
        }
    }
}
