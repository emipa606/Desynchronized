﻿using System;

namespace Desynchronized.TNDBS;

[Obsolete("Ancient.", true)]
public static class TaleNewsTypeMapperUtility
{
    public static Type GetTypeForEnum(this TaleNewsTypeEnum typeEnum)
    {
        switch (typeEnum)
        {
            case TaleNewsTypeEnum.PawnDied:
                return typeof(TaleNewsPawnDied);
            default:
                return typeof(object);
        }
    }
}