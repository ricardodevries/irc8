﻿using Irc.Enumerations;
using Irc.Objects;
using Irc.Resources;

namespace Irc.Modes.Channel;

public class Private : ModeRuleChannel
{
    public Private() : base(IrcStrings.ChannelModePrivate)
    {
    }

    public new EnumIrcError Evaluate(ChatObject source, ChatObject? target, bool flag, string? parameter)
    {
        var result = base.Evaluate(source, target, flag, parameter);
        if (result == EnumIrcError.OK)
        {
            var channel = (Objects.Channel)target;

            if (flag)
            {
                if (channel.Secret)
                {
                    channel.Secret = false;
                    DispatchModeChange(IrcStrings.ChannelModeSecret, source, target, false, string.Empty);
                }

                if (channel.Hidden)
                {
                    channel.Hidden = false;
                    DispatchModeChange(IrcStrings.ChannelModeHidden, source, target, false, string.Empty);
                }
            }

            SetChannelMode(source, (Objects.Channel)target, flag, parameter);
        }

        return result;
    }
}