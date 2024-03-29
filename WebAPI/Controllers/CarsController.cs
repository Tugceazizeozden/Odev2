﻿using Business;
using Business.Abstract;
using Business.Requests;
using Business.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarService _CarService; 

    public CarsController(ICarService CarService)
    {
        _CarService = CarService;
    }

    
    

    [HttpGet]  
    public GetCarListResponse GetList([FromQuery] GetCarListRequest request) 
    {
        GetCarListResponse response = _CarService.GetList(request);
        return response;
    }

    [HttpPost]  
    public ActionResult<AddCarResponse> Add(AddCarRequest request)
    {
        try
        {
            AddCarResponse response = _CarService.Add(request);

            return CreatedAtAction(nameof(GetList), response); 
        }
        catch (Core.CrossCuttingConcerns.Exceptions.BusinessException exception)
        {
            return BadRequest(
                new Core.CrossCuttingConcerns.Exceptions.BusinessProblemDetails()
                {
                    Title = "Business Exception",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = exception.Message,
                    Instance = HttpContext.Request.Path
                }
            );
        }
    }
}
