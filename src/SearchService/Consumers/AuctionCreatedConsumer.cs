using System.Net.Mime;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IMapper _mapper  { get; }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming auction created: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        if (item.Model == "Foo")    throw new ArgumentException("Cannot sell cars with name Foo");
        
       await item.SaveAsync();
    }
}