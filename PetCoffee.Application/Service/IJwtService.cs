﻿using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Service;

public interface IJwtService
{
	string GenerateJwtToken(Account account);


}
