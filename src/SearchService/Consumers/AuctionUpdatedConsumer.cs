using AutoMapper;
using MassTransit;
using Contracts;
using MongoDB.Entities;
using MongoDB.Driver;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IMapper _mapper  { get; }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine("--> Consuming auction Updated: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);
        
      var result = await DB.Update<Item>()
                .Match(x => x.ID == context.Message.Id)
                .ModifyOnly(x => new 
                {
                    x.Color,
                    x.Make,
                    x.Model,
                    x.Year,
                    x.Mileage
                }, item)
                .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(AuctionUpdated), "Problem Updating mongodb");
    }

}