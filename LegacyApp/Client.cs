
using System;

namespace LegacyApp
{
    public enum ClientType
    {
        NormalClient,
        VeryImportantClient,
        ImportantClient
    }

    public interface IClient
    {
        string Name { get; }
        int ClientId { get; }
        string Email { get; }
        string Address { get; }
        ClientType Type { get; }
    }

    public class Client : IClient
    {
        public string Name { get; internal set; }
        public int ClientId { get; internal set; }
        public string Email { get; internal set; }
        public string Address { get; internal set; }
        public ClientType Type { get; set; }
    }

    
    public interface IClientRepository
    {
        IClient GetById(int clientId);
    }
}