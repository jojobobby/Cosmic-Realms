using System;

namespace common
{
    public enum LoginStatus
    {
        OK,
        AccountNotExists,
        InvalidCredentials,
        CodeNeeded,
        WrongCode
    }

    public enum RegisterStatus
    {
        OK,
        UsedName,
        IP
    }

    public enum GuildCreateStatus
    {
        OK,
        InvalidName,
        UsedName
    }

    public enum AddGuildMemberStatus
    {
        OK,
        NameNotChosen,
        AlreadyInGuild,
        InAnotherGuild,
        IsAMember,
        GuildFull,
        Error
    }

    public enum CreateStatus
    {
        OK,
        ReachCharLimit,
        SkinUnavailable,
        Locked
    }

    public static class StatusInfo
    {
        public static string GetInfo(this LoginStatus status)
        {
            switch (status)
            {
                case LoginStatus.InvalidCredentials:
                    return "Bad Login";
                case LoginStatus.AccountNotExists:
                    return "Bad Login";
                case LoginStatus.OK:
                    return "OK";
                case LoginStatus.CodeNeeded:
                    return "Code has been sent to your discord";
                case LoginStatus.WrongCode:
                    return "The code you entered is incorrect";
            }
            throw new ArgumentException("status");
        }

        public static string GetInfo(this RegisterStatus status)
        {
            switch (status)
            {
                case RegisterStatus.UsedName:
                    return "Duplicate Email"; // maybe not wise to give this info out...
                case RegisterStatus.OK:
                    return "OK";
                case RegisterStatus.IP:
                    return "You have already registered your account!";
            }
            throw new ArgumentException("status");
        }

        public static string GetInfo(this CreateStatus status)
        {
            switch (status)
            {
                case CreateStatus.ReachCharLimit:
                    return "Too many characters";
                case CreateStatus.OK:
                    return "OK";
            }
            throw new ArgumentException("status");
        }
    }
}
