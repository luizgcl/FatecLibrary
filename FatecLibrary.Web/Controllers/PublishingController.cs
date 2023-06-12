using FatecLibrary.Web.Models.Entities;
using FatecLibrary.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FatecLibrary.Web.Roles;


namespace FatecLibrary.Web.Controllers;

[Authorize(Roles = Role.Admin)]
public class PublishingController : Controller
{

    private readonly IPublishingService _publisingService;

    public PublishingController(IPublishingService publisingService)
    {
        _publisingService = publisingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublishingViewModel>>> Index()
    {
        var result = await _publisingService.GetAllPublishers(await GetAccessToken());
        if (result is null) return View("Error");
        return View(result);
    }

    // Criar a view CreatePublishing
    [HttpGet]
    public async Task<IActionResult> CreatePublishing()
    {
       return View();
    }
    

    [HttpPost]
    public async Task<IActionResult> CreatePublishing(PublishingViewModel publishingViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _publisingService.CreatePublishing(publishingViewModel, await GetAccessToken());

            if (result is not null) return RedirectToAction(nameof(Index));
        }
        else
            return BadRequest("Error");
        
        return View(publishingViewModel);
    }

    // Criar a view UpdatePublishing
    [HttpGet]
    public async Task<IActionResult> UpdatePublishing(int id)
    {
        var result = await _publisingService.FindPublishingById(id, await GetAccessToken());
        if (result is null) return View("Error");
        return View(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePublishing(PublishingViewModel publishingViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _publisingService.UpdatePublishing(publishingViewModel, await GetAccessToken());
            if (result is not null) return RedirectToAction(nameof(Index));
        }
        else
            return BadRequest("Error");

        return View(publishingViewModel);
    }

    // criar a view DeletePublishing
    [HttpGet]
    public async Task<ActionResult<PublishingViewModel>> DeletePublishing(int id)
    {
        var result = await _publisingService.FindPublishingById(id, await GetAccessToken());
        if (result is null) return View("Error");
        return View(result);
    }

    
    [HttpPost(), ActionName("DeletePublishing")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _publisingService.DeletePublishingById(id, await GetAccessToken());
        if (!result) return View("Error");
        return RedirectToAction("Index");
    }

    private async Task<string> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }
}

