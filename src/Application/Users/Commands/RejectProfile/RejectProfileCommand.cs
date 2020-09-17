using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.Users.Commands.RejectProfile
{
    public class RejectProfileCommand : IRequest
    {
        public string UserId { get; set; }
    }
}
