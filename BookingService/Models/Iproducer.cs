using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Models
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
