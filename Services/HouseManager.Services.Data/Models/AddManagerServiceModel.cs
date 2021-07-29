﻿namespace HouseManager.Services.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddManagerServiceModel:IRoleToAddress
    {
        public string UserId { get ; set ; }

        public string Title { get; set; } = "Домоуправител";

        public IEnumerable<SelectListItem> Users { get; set ; }
    }
}
