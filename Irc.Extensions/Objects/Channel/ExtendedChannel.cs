﻿using Irc.Constants;
using Irc.Enumerations;
using Irc.Extensions.Interfaces;
using Irc.Interfaces;
using Irc.IO;
using Irc.Objects;
using System.Text.RegularExpressions;

namespace Irc.Extensions.Objects.Channel;

public class ExtendedChannel : global::Irc.Objects.Channel.Channel, IExtendedChatObject
{
    private ChannelPropCollection _properties = new();
    public IPropCollection PropCollection => _properties;

    public ExtendedChannel(string name, IModeCollection modeCollection, IDataStore dataStore) : base(name,
        modeCollection, dataStore)
    {
        _properties.SetProp("NAME", name);
    }

    protected override IChannelMember AddMember(IUser user, EnumChannelAccessResult accessResult = EnumChannelAccessResult.NONE)
    {
        var member = new Member.ExtendedMember(user);

        if (accessResult == EnumChannelAccessResult.SUCCESS_OWNER) member.SetOwner(true);
        else if (accessResult == EnumChannelAccessResult.SUCCESS_HOST) member.SetHost(true);
        else if (accessResult == EnumChannelAccessResult.SUCCESS_VOICE) member.SetVoice(true);

        _members.Add(member);
        user.AddChannel(this, member);
        return member;
    }

    public override EnumChannelAccessResult GetAccess(IUser user, string key, bool IsGoto = false)
    {
        var operCheck = CheckOper(user);
        var keyCheck = CheckMemberKey(user, key);
        var hostKeyCheck = CheckHostKey(user, key);
        var inviteOnlyCheck = CheckInviteOnly();
        var userLimitCheck = CheckUserLimit(IsGoto);

        return (EnumChannelAccessResult)(new int[] {
            (int)operCheck,
            (int)keyCheck,
            (int)hostKeyCheck,
            (int)inviteOnlyCheck,
            (int)userLimitCheck
        }).Max();
    }


    protected EnumChannelAccessResult CheckHostKey(IUser user, string key)
    {
        if (PropCollection.GetProp("OWNERKEY").GetValue() == key) {
            return EnumChannelAccessResult.SUCCESS_OWNER;
        }
        else if (PropCollection.GetProp("HOSTKEY").GetValue() == key) {
            return EnumChannelAccessResult.SUCCESS_HOST;
        }
        return EnumChannelAccessResult.NONE;
    }

    public static new bool ValidName(string channel)
    {
        var regex = new Regex(Resources.IrcChannelRegex);
        return regex.Match(channel).Success;
    }
}