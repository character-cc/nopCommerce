
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Discounts;
using Nop.Data;
using Nop.Plugin.Customer.Loyalty.Domain;
using Nop.Plugin.Customer.Loyalty.Factories;
using Nop.Plugin.Customer.Loyalty.Models;
using Nop.Plugin.Customer.Loyalty.Services;
using Nop.Services.Discounts;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Customer.Loyalty.Controller;

[AutoValidateAntiforgeryToken]
[AuthorizeAdmin] //confirms access to the admin panel
[Area(AreaNames.ADMIN)] //specifies the area containing a controller or action
public class LoyaltyController : BasePluginController
{

    #region Field

    private readonly ILoyaltyLevelService _loyaltyLevelService;
    private readonly ILoyaltyLevelModelFactory _loyaltyLevelModelFactory;
    protected readonly INotificationService _notificationService;
    private readonly IDiscountService _discountService;
    private readonly IRepository<Discount> _discountRepository;

    #endregion

    #region ctor

    public LoyaltyController(
        ILoyaltyLevelService loyaltyLevelService,
        ILoyaltyLevelModelFactory loyaltyLevelModelFactory,
        INotificationService notificationService,
        IDiscountService discountService,
        IRepository<Discount> discountRepository)
    {
        _loyaltyLevelService = loyaltyLevelService;
        _loyaltyLevelModelFactory = loyaltyLevelModelFactory;
        _notificationService = notificationService;
        _discountService = discountService;
        _discountRepository = discountRepository;
    }


    #endregion

    #region methods
    public virtual IActionResult Configure()
    {
        var model = new LoyaltyLevelSearchModel();
        model.SetGridPageSize();

        return View("~/Plugins/Customer.Loyalty/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> List(LoyaltyLevelSearchModel searchModel)
    {
        var model = await _loyaltyLevelModelFactory.PrepareLoyaltyLevelListModelAsync(searchModel);
        return Json(model);
    }

    public virtual async Task<IActionResult> Create()
    {
        var model = await _loyaltyLevelModelFactory.PrepareLoyaltyLevelModelAsync();
        return View("~/Plugins/Customer.Loyalty/Views/Create.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> Create(LoyaltyLevelModel model, bool continueEditing)
    {
   
        if (ModelState.IsValid)
        {
            var loyaltyLevel = model.ToEntity<LoyaltyLevel>();
            await _loyaltyLevelService.InsertLoyaltyLevelAsync(loyaltyLevel);
            _notificationService.SuccessNotification("Loyalty level created successfully");
            if (continueEditing)
                return RedirectToAction("Edit", new { id = loyaltyLevel.Id });
            return RedirectToAction("Configure");
        }
        model = await _loyaltyLevelModelFactory.PrepareLoyaltyLevelModelAsync(model, null);
        return View("~/Plugins/Customer.Loyalty/Views/Create.cshtml", model);


    }

    public virtual async Task<IActionResult> Edit(int id)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(id);
        if (loyaltyLevel == null)
            return RedirectToAction("Configure");
        var model = await _loyaltyLevelModelFactory.PrepareLoyaltyLevelModelAsync(null, loyaltyLevel);

        return View("~/Plugins/Customer.Loyalty/Views/Edit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> Edit(LoyaltyLevelModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(model.Id);
            if (loyaltyLevel == null)
                return RedirectToAction("Configure");
            loyaltyLevel = model.ToEntity<LoyaltyLevel>();
            await _loyaltyLevelService.UpdateLoyaltyLevelAsync(loyaltyLevel);
            _notificationService.SuccessNotification("Loyalty level updated successfully");
            if (continueEditing)
                return RedirectToAction("Edit", new { id = loyaltyLevel.Id });
            return RedirectToAction("Configure");
        }
        model = await _loyaltyLevelModelFactory.PrepareLoyaltyLevelModelAsync(model, null);
        return View("~/Plugins/Customer.Loyalty/Views/Edit.cshtml", model);



    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(id);
        if (loyaltyLevel == null)
            return RedirectToAction("Configure");
        await _loyaltyLevelService.DeleteLoyaltyLevelAsync(loyaltyLevel);
        _notificationService.SuccessNotification("Loyalty level deleted suceesfully");
        return RedirectToAction("Configure");

    }


    [HttpPost]
    public virtual async Task<IActionResult> DiscountList(LoyaltyDiscountSearchModel loyaltyDiscountSearchModel)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(loyaltyDiscountSearchModel.LoyaltyLevelId) ??
            throw new ArgumentNullException("Loyalty level not found with specified id");
        var model = await _loyaltyLevelModelFactory.PrepareLoyaltyDicountListModelAsync(loyaltyDiscountSearchModel, loyaltyLevel);

        return Json(model);


    }

    public virtual async Task<IActionResult> AddDiscountPopup(int loyaltyLevelId)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(loyaltyLevelId) ??
            throw new ArgumentNullException("Loyalty level not found with specified id");
        var model = await _loyaltyLevelModelFactory.PrepareAddDiscountToLoyaltyLevelSearchModelAsync(null, loyaltyLevel);
        return View("~/Plugins/Customer.Loyalty/Views/AddDiscountPopup.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> AddDicountPopupList(AddOrDeleteDiscountToLoyaltyLevelSearchModel addDiscountToLoyaltyLevelSearchModel)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(addDiscountToLoyaltyLevelSearchModel.LoyaltyLevelId) ??
            throw new ArgumentNullException("Loyalty level not found with specified id");
        var model = await _loyaltyLevelModelFactory.PrepareAddDiscountToLoyaltyLevelListModelAsync(addDiscountToLoyaltyLevelSearchModel, loyaltyLevel);
        return Json(model);
    }

    [HttpPost]
    [FormValueRequired("save")]
    public virtual async Task<IActionResult> AddDiscountPopup(AddOrDeleteDiscountToLoyaltyLevelModel addDiscountToLoyaltyLevelModel)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(addDiscountToLoyaltyLevelModel.LoyaltyLevelId) ??
            throw new ArgumentNullException("Loyalty level not found with specified id");
        var mappedDiscountIds = await _loyaltyLevelService.GetMappedDiscountIds(loyaltyLevel.Id, addDiscountToLoyaltyLevelModel.SelectedDiscountIds);
        var discountIds = addDiscountToLoyaltyLevelModel.SelectedDiscountIds.Except(mappedDiscountIds).ToList();
        var discounts = await _discountService.GetDiscountsByIdsAsync(_discountRepository , discountIds);
        var loyaltyLevelDiscountMappings = discounts.Select(discount => new LoyaltyLevelDiscountMapping
        {
            DiscountId = discount.Id,
            LoyaltyLevelId = loyaltyLevel.Id,
            EnableInherit = addDiscountToLoyaltyLevelModel.EnableInheritIds.Contains(discount.Id)
        }).ToList();
        await _loyaltyLevelService.InsertLoyaltyLevelDiscountMappingsAsync(loyaltyLevelDiscountMappings);

        await _loyaltyLevelService.InsertLoyaltyLevelToDiscountRequirementAsync(loyaltyLevelDiscountMappings);

        _notificationService.SuccessNotification("Discounts added to loyalty level successfully");
        return RedirectToAction("Edit", new { id = loyaltyLevel.Id });

    }


    public virtual async Task<IActionResult> DeleteDiscountPopup(int loyaltyLevelId)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(loyaltyLevelId) ??
            throw new ArgumentNullException("Loyalty level not found with specified id");
        var model = await _loyaltyLevelModelFactory.PrepareAddDiscountToLoyaltyLevelSearchModelAsync(null, loyaltyLevel);
        return View("~/Plugins/Customer.Loyalty/Views/DeleteDiscountPopup.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> DeleteDicountPopupList(AddOrDeleteDiscountToLoyaltyLevelSearchModel addDiscountToLoyaltyLevelSearchModel)
    {
        var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByIdAsync(addDiscountToLoyaltyLevelSearchModel.LoyaltyLevelId) ??
            throw new ArgumentNullException("Loyalty level not found with specified id");
        var model = await _loyaltyLevelModelFactory.PrepareDeleteDiscountToLoyaltyLevelListModelAsync(addDiscountToLoyaltyLevelSearchModel, loyaltyLevel);
        return Json(model);
    }

    [HttpPost]
    [FormValueRequired("delete")]
    public virtual async Task<IActionResult> DeleteDiscountPopup(AddOrDeleteDiscountToLoyaltyLevelModel addDiscountToLoyaltyLevelModel)
    {
        var loyaltyLevelId = addDiscountToLoyaltyLevelModel.LoyaltyLevelId;
        var discountIds = addDiscountToLoyaltyLevelModel.SelectedDiscountIds;

        var loyaltyLevelDiscountMappings = discountIds.Select(discountId => new LoyaltyLevelDiscountMapping
        {
            DiscountId = discountId,
            LoyaltyLevelId = loyaltyLevelId
        }).ToList();
        await _loyaltyLevelService.DeleteLoyaltyLevelDiscountMappingsAsync(loyaltyLevelDiscountMappings);
        await _loyaltyLevelService.DeleteLoyaltyLevelToDiscountRequirementAsync(loyaltyLevelDiscountMappings);
        _notificationService.SuccessNotification("Discounts deleted to loyalty level successfully");
        return RedirectToAction("Edit", new { id = loyaltyLevelId });

    }



    #endregion
}
