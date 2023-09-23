using Microsoft.AspNetCore.Http;
using Market.Core.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Market.Application.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                throw new Exception("*_*" + "Internal Server Error" + "*_*");

                //switch (error)
                //{

                //case AppException e:
                //    throw new BadHttpRequestException("*_*" + ((AppException)error).ErrorMessage + "*_*");
                //default:
                //    throw new Exception("*_*" + "Internal Server Error" + "*_*");
                //}
            }
        }
        public static string GetServerIp(HttpContext context)
        {
            try
            {
                IPAddress ipAddressString = context.Connection.LocalIpAddress;

                string result = "LocalIpAddress: "+ipAddressString.ToString();


                return result;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {

                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string GetUserExternalIp(HttpContext context)
        {
            try
            {
                IPAddress remoteIpAddress = context.Connection.RemoteIpAddress;
                string result = "";
                if (remoteIpAddress != null)
                {
                    // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                    // This usually only happens when the browser is on the same machine as the server.
                    if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
                            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    }
                    result = remoteIpAddress.ToString();
                }
                return result;
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
