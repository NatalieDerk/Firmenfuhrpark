using Microsoft.AspNetCore.SignalR; 
using Xunit.Sdk; 

namespace Backend.Hubs 
{ public class BookingHub: Hub 
    { public async Task SendNewBookingNotification(int boogingId) 
        { 
            await Clients.All.SendAsync("ReceiveNewBooking", boogingId); 
        } 
    } 
}   
