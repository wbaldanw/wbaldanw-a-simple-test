﻿namespace Last.Simple.App.Domain.DataObjects
{
    public class UserDO
    {
        public long Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
