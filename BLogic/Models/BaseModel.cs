﻿using System;

namespace BLogic.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
