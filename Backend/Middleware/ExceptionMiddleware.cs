﻿using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using MixedDreams.Core.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Core.Responses.Errors;
using System.Net.Http;
using MixedDreams.Core.Exceptions;

namespace MixedDreams.WebAPI.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>()!;
            context.Response.ContentType = "application/json";
            var responseBody = new ErrorResponse();
            context.Response.StatusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            responseBody.StatusCode = context.Response.StatusCode;
            if (responseBody.StatusCode == 500)
            {
                _logger.LogError($"Something went wrong: {exception}");
                responseBody.Title = "An internal server error occured.";
            }
            else
            {
                _logger.LogWarning($"Something went wrong: {exception}");
                responseBody.Title = exception.Message;
            }
            await context.Response.WriteAsync(responseBody.ToString());
        }
    }
}
