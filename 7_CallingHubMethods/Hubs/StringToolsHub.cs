
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class StringToolsHub : Hub
{
    public string GetFullName(string firstName, string lastName){
        return $"{firstName} {lastName}";
    }
}